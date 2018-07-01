
//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
//	ObjectManager.cs
//	
//	作成者:佐々木瑞生		
//==================================================
//	概要
//	全オブジェクトの管理、更新
//	
//==================================================
//	作成日：2017/03/12
//	
//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : SingletonMonoBehaviour<ObjectManager> {
	[SerializeField]
	private List<OrderControl> m_OrderList;
	[SerializeField]
	private OrderControl m_OrderControlPrefab;
    private bool m_isPause = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        int i;
        if(m_isPause) {
            float deltaTime = Time.unscaledDeltaTime;
            for(i = 0; i < m_OrderList.Count; i++) {
                m_OrderList[i].PauseExecute(deltaTime);
            }
        } else {
            float deltaTime = Time.deltaTime;
            for(i = 0; i < m_OrderList.Count; i++) {
                m_OrderList[i].Execute(deltaTime);
            }
            for(i = 0; i < m_OrderList.Count; i++) {
                m_OrderList[i].LateExecute(deltaTime);
            }
        }
	}

    /// <summary>
	/// オブジェクトの追加
	/// </summary>
	/// <param name="newObj">追加するオブジェクト</param>
	/// <param name="OrderNumber">オーダーの番号、デフォルトは0</param>
    public void RegistrationList(ObjectBase newObj, int OrderNumber = 0) {
		if(m_OrderList.Count <= OrderNumber) {
			for(int i = m_OrderList.Count; i <= OrderNumber; i++) {
				OrderControl tmp = Instantiate(m_OrderControlPrefab);
				m_OrderList.Add(tmp);
				tmp.transform.parent = transform;
			}
		}
		m_OrderList[OrderNumber].AddList(newObj);
    }

    /// <summary>
    /// ポーズの設定
    /// </summary>
    /// <param name="pause"></param>
    public void SetPause(bool pause) {
        if(!m_isPause && pause) {
            for(int i = 0; i < m_OrderList.Count; i++) {
                m_OrderList[i].OnPauseExecute();
            }
            m_isPause = true;
            Time.timeScale = 0;
        }else if(!pause) {
            m_isPause = false;
            Time.timeScale = 1.0f;
        }
    }
}
