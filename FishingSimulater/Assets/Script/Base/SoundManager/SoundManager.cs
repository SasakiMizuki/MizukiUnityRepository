using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サウンド管理クラス
/// </summary>
public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
	[SerializeField] GameObject m_AudioPrefab = null;
    //オーディオファイルのパス
    private const string BGM_PATH = "Audio/BGM";
    private const string SE_PATH = "Audio/SE";

    // 音量
    public float m_fBGMVolume;   //SE
    public float m_fSEVolume;    //BGM

    //BGMがフェードするのにかかる時間
    public const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;
    public const float BGM_FADE_SPEED_RATE_LOW = 0.3f;
    private float m_fBgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;

    //次流すBGM名、SE名
    private string m_NextBGMName;
    private string m_NextSEName;

	// 再生箇所を変えるよう
	private float m_StartTime = 0;

    //BGMをフェードアウト中か
    private bool m_bFadeOut = false;

    //BGM用、SE用に分けてオーディオソースを持つ
    private AudioSource m_BgmSource;
    private List<AudioSourceInfo> m_SeSourceList = new List<AudioSourceInfo>();
    private const int SE_SOURCE_NUM = 20;  //SEの数

    //音データを保存用
    private Dictionary<string, AudioClip> BGMDictionary;
    private Dictionary<string, AudioClip> SEDictionary;

    //カメラの初期位置保存用のKey
    const string m_SEValueKey  = "Key_SEValue";
    const string m_BGMValueKey = "Key_BGMValue";
    const string m_ValueSetKey = "Key_ValueSet";

    //Sound情報まとめたもの
    public class AudioSourceInfo
    {
        public AudioSource SESource;
        public GameObject ParentObj;
        public GameObject SourceObj;
        public string SEName;
        public bool isFadeOut;
        public float seFadeSpeedRate;
        public AudioSourceInfo()
        {
            SESource = null;
            ParentObj = null;
        }
    };


    /// <summary>
    /// 開始時処理
    /// </summary>
    protected override void Awake()
    {
        DontDestroyOnLoad(gameObject);
        base.Awake();
        
        //音量設定
        if ( PlayerPrefs.GetInt(m_ValueSetKey) == 0 )
        {
            m_fBGMVolume = 1.0f;
            m_fSEVolume  = 1.0f;
        }
        else
        {
            m_fBGMVolume = PlayerPrefs.GetFloat(m_BGMValueKey);
            m_fSEVolume  = PlayerPrefs.GetFloat(m_SEValueKey);
        }


        //SEとBGMのGameObjectを作成して子に設定
        for ( int i = 0; i < SE_SOURCE_NUM + 1; i++ )
        {
            AudioSourceInfo audioInfo = new AudioSourceInfo();
			GameObject soundObj;
			if(m_AudioPrefab == null) {
				soundObj = new GameObject("AudioObj" + i);
				soundObj.AddComponent<AudioSource>();
			}else {
				soundObj = Instantiate(m_AudioPrefab);
			}

			audioInfo.SESource  = soundObj.AddComponent<AudioSource>();
            audioInfo.ParentObj = null;
            audioInfo.SESource.playOnAwake = false;
            audioInfo.SourceObj = soundObj;
            soundObj.transform.SetParent(this.gameObject.transform);

            if ( i == 0 )
            {//BGM
                audioInfo.SESource.spatialBlend = 0.0f;
                audioInfo.SESource.loop = true;
                audioInfo.SESource.volume = m_fBGMVolume;
                m_BgmSource = audioInfo.SESource;
            }
            else
            {//SE
                audioInfo.SESource.minDistance = 1.0f;
                audioInfo.SESource.spatialBlend = 1.0f;
                audioInfo.SESource.loop = false;
                audioInfo.SESource.volume = m_fSEVolume;
                m_SeSourceList.Add(audioInfo);
            }
        }

        //ResourceフォルダからSEとBGMをファイルを読み込み
        BGMDictionary = new Dictionary<string, AudioClip>();
        SEDictionary = new Dictionary<string, AudioClip>();

        //Resourceフォルダから読み込み
        Object[] BgmList = Resources.LoadAll(BGM_PATH);
        Object[] SEList  = Resources.LoadAll(SE_PATH);

        //Dictionaryに格納
        foreach (AudioClip Bgm in BgmList)
            BGMDictionary[Bgm.name] = Bgm;
        foreach (AudioClip SE in SEList)
            SEDictionary[SE.name] = SE;
    }

    /// <summary>
    /// 指定したSE再生(3D音源版)
    /// </summary>
    /// <param name="seName"></param>
    /// <param name="position"></param>
    /// <param name="minDistance"></param>
    /// <param name="parentObj"></param>
    /// <param name="Loop"></param>
    public void PlaySE(string seName, Vector3 position, float minDistance = 1.0f, GameObject parentObj = null, bool Loop = false)
    {
        if ( !SEDictionary.ContainsKey(seName) )
        {
            Debug.Log(seName + "というSEが見つかりません");
            return;
        }

        foreach ( AudioSourceInfo audioSourceInfo in m_SeSourceList )
        {
            if ( !audioSourceInfo.SESource.isPlaying )
            {
                audioSourceInfo.SESource.minDistance = minDistance;
                audioSourceInfo.SESource.volume = m_fSEVolume;
                audioSourceInfo.SourceObj.transform.position = position;
                audioSourceInfo.ParentObj = parentObj;
                audioSourceInfo.SESource.spatialBlend = 1.0f;
                audioSourceInfo.SESource.loop = Loop;
                audioSourceInfo.isFadeOut = false;
                audioSourceInfo.SEName = seName;
                if ( Loop )
                {
                    audioSourceInfo.SESource.clip = SEDictionary[seName] as AudioClip;
                    audioSourceInfo.SESource.Play();
                }
                else
                {
                    audioSourceInfo.SESource.PlayOneShot(SEDictionary[seName] as AudioClip);
                }
                return;
            }
        }
    }


    /// <summary>
    /// 指定したSEを再生(通常版)
    /// </summary>
    /// <param name="seName"></param>
    /// <param name="Loop"></param>
    public void PlaySE(string seName, float Pitch = 1.0f, bool Loop = false, float Time = 0.0f) {

		if(!SEDictionary.ContainsKey(seName)) {
			Debug.Log(seName + "というSEが見つかりません");
			return;
		}

		foreach ( AudioSourceInfo audioSourceInfo in m_SeSourceList )
        {
            //使用してないAudioSource検索
            if ( !audioSourceInfo.SESource.isPlaying )
            {
                audioSourceInfo.SESource.volume = m_fSEVolume;
                audioSourceInfo.SESource.spatialBlend = 0.0f;
                audioSourceInfo.SESource.pitch = Pitch;
                audioSourceInfo.SESource.loop = Loop;
                audioSourceInfo.isFadeOut = false;
                audioSourceInfo.SEName = seName;
				audioSourceInfo.SESource.time = Time;

                //ループ設定
                if ( Loop )
                {
                    audioSourceInfo.SESource.clip = SEDictionary[seName] as AudioClip;
                    audioSourceInfo.SESource.Play();
                }
                else
                {
                    audioSourceInfo.SESource.PlayOneShot(SEDictionary[seName] as AudioClip);
                }
                return;
            }
        }
    }


    /// <summary>
    /// 指定した名前のBGMを再生
    /// </summary>
    /// <param name="BgmName"></param>
    /// <param name="FadeRate"></param>
    public float PlayBGM(string BgmName,float StartTime = 0.0f, float Pitch = 1.0f, float FadeRate = BGM_FADE_SPEED_RATE_HIGH)
    {
		float EndTime = 0;
        //指定ファイルが無かった場合
        if (BGMDictionary.ContainsKey(BgmName) == false)
        {
            Debug.Log(BgmName + "というファイルはありません");
            return 0;
        }

        //再生中はそのまま流す
        if ( !m_BgmSource.isPlaying )
        {
            //ファイル名初期化
            m_NextBGMName = "";
            m_BgmSource.spatialBlend = 0.0f;
            m_BgmSource.clip = BGMDictionary[BgmName] as AudioClip;
			m_BgmSource.time = StartTime;
            //フェードで音を流す場合
            if ( FadeRate > 0 ) 
            {
                m_BgmSource.volume = m_fBGMVolume;
            } 
            else if ( FadeRate < 0 )
            {
                m_BgmSource.volume = 0.0f;
            }
            m_BgmSource.Play();
        }
        else if ( m_BgmSource.clip.name != BgmName ) {//次BGMに設定
			m_NextBGMName = BgmName;
            EndTime = FadeOutBGM(FadeRate);
        }

        //ピッチ変更
        m_BgmSource.pitch = Pitch;

		m_StartTime = StartTime;
		return EndTime;
    }

    /// <summary>
    /// BGM停止
    /// </summary>
    public void StopBGM()
    {
        m_BgmSource.Stop();
    }

    /// <summary>
    /// SE停止
    /// </summary>
    /// <param name="seName"></param>
    public void StopSE(string seName)
    {
        foreach ( AudioSourceInfo audioSourceInfo in m_SeSourceList )
        {
            if ( audioSourceInfo.SESource.isPlaying && audioSourceInfo.SEName == seName )
            {
                audioSourceInfo.SESource.Stop();
                return;
            }
        }
    }

    /// <summary>
    /// フェードアウトでBGM停止
    /// </summary>
    /// <param name="fadeSpeedRate"></param>
    public float FadeOutBGM(float fadeSpeedRate = BGM_FADE_SPEED_RATE_LOW)
    {
        m_fBgmFadeSpeedRate = fadeSpeedRate;
        m_bFadeOut = true;
		return m_BgmSource.time;
    }


    /// <summary>
    /// フェードアウトでSE停止
    /// </summary>
    /// <param name="seName"></param>
    /// <param name="fadeSpeedRate"></param>
    public void FadeOutSE(string seName, float fadeSpeedRate = BGM_FADE_SPEED_RATE_LOW)
    {
        foreach ( AudioSourceInfo audioSourceInfo in m_SeSourceList )
        {
            if ( audioSourceInfo.SESource.isPlaying && audioSourceInfo.SEName == seName )
            {
                audioSourceInfo.seFadeSpeedRate = fadeSpeedRate;
                audioSourceInfo.isFadeOut = true;
                return;
            }
        }
    }


    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        for(int i = 0; i < m_SeSourceList.Count; i++ )
        {
            //親オブジェクトがある場合その場所に移動
            if ( m_SeSourceList[i].SESource.isPlaying && m_SeSourceList[i].ParentObj != null )
                m_SeSourceList[i].SourceObj.transform.position = m_SeSourceList[i].ParentObj.transform.position;

            //SEのフェードアウト
            if ( m_SeSourceList[i].SESource.isPlaying && m_SeSourceList[i].isFadeOut )
            {
                m_SeSourceList[i].SESource.volume -= Time.deltaTime * m_SeSourceList[i].seFadeSpeedRate;
                
                //SEの音量が0になったら停止
                if ( m_SeSourceList[i].SESource.volume <= 0 )
                {
                    m_SeSourceList[i].SESource.Stop();
                    m_SeSourceList[i].isFadeOut = false;
                }
            }
        }

        //BGMのフェードアウト処理
        if ( !m_bFadeOut )
            return;

        //音量を下げていく
        m_BgmSource.volume -= Time.deltaTime * m_fBgmFadeSpeedRate;
        if ( m_BgmSource.volume <= 0 )
        {
            m_BgmSource.Stop();
            m_BgmSource.volume = m_fBGMVolume;
            m_bFadeOut = false;

            //次が設定してあったら再生
            if ( !string.IsNullOrEmpty(m_NextBGMName) )
            {
                PlayBGM(m_NextBGMName, m_StartTime);
            }
        }
    }

    /// <summary>
    /// BGMとSEの音量変更
    /// </summary>
    /// <param name="BGMVolum"></param>
    /// <param name="SEVolum"></param>
    public void ChangeVolume(float BgmVolum, float SeVolum)
    {
        if ( !m_bFadeOut )
        {
            m_BgmSource.volume = BgmVolum;
        }
        foreach ( AudioSourceInfo audioSourceInfo in m_SeSourceList )
        {
            if ( !audioSourceInfo.isFadeOut )
                audioSourceInfo.SESource.volume = SeVolum;
        }

        m_fBGMVolume = BgmVolum;
        m_fSEVolume = SeVolum;
    }


    /// <summary>
    /// ピッチ変更
    /// </summary>
    /// <param name="Pitch"></param>
    public void ChangeBGMPitch(float Pitch)
    {
        m_BgmSource.pitch = Pitch;
    }


    /// <summary>
    /// 音量変更
    /// </summary>
    /// <param name="_bgmVolume"></param>
    /// <param name="_seVolume"></param>
    public void ChangeVolumeRate(float _bgmVolume, float _seVolume)
    {
        if(!m_bFadeOut)
        {
            m_BgmSource.volume *= _bgmVolume;
        }
        foreach ( AudioSourceInfo audioSourceInfo in m_SeSourceList )
        {
            if ( !audioSourceInfo.isFadeOut )
                audioSourceInfo.SESource.volume *= _seVolume;
        }
    }


    /// <summary>
    /// ボリューム保存
    /// </summary>
    public void SaveVolume()
    {
        PlayerPrefs.SetFloat(m_BGMValueKey, m_fBGMVolume);
        PlayerPrefs.SetFloat(m_SEValueKey, m_fSEVolume);
        PlayerPrefs.SetInt(m_ValueSetKey, 1);
    }
}
