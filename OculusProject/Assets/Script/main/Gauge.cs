//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
//	gauge.cs
//	
//	作成者:
//==================================================
//	概要
//	
//  
//	
//==================================================
//	作成日：yyyy/mm/dd
//	
//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : ObjectBase {
    // メンバ
    #region Member
    [SerializeField]
    private float gaugeUpRate;
    private float gaugePropotion = 0.01f;
    public float GaugePropotion {
        get
        {
            return gaugePropotion;
        }
        set
        {
            gaugePropotion = value;
        }
    }
    [SerializeField]
    private Image gaugeUpImage;

    private bool isSet;
    public bool IsSet {
        get
        {
            return isSet;
        }
        set
        {
            isSet = value;
        }
    }
	#endregion Member

	// 定数
	#region Constant
	
	#endregion Constant

	// メソッド
	#region Method
	void Start () {
		m_OrderNumber = 0;
		ObjectManager.Instance.RegistrationList(this, m_OrderNumber);
	}
	
	public override void Execute(float deltaTime) {
        if(isSet) {
            gaugePropotion += ( gaugePropotion * gaugeUpRate ) * deltaTime;
            if(gaugePropotion >= 1.0f) {
                gaugePropotion = 0.01f;
            }
            gaugeUpImage.fillAmount = gaugePropotion;
        }
	}

	public override void LateExecute(float deltaTime) {
		
	}
	#endregion Method
}
