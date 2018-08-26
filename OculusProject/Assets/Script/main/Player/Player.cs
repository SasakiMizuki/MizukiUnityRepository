//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
//	Player.cs
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

public class Player : SingletonMonoBehaviour<Player> {
    // メンバ
    #region Member
    [SerializeField]
    float m_throwPower = 1.0f;
    [SerializeField]
    GameObject m_eyeObj;
    [SerializeField]
    GameObject m_handObj;
    [SerializeField]
    Gauge m_gaugeObj;
    ObjectUsingChecker.ObjectData m_currentFrisbee;
    public ObjectUsingChecker.ObjectData CurrentFrisbee {
        get
        {
            return m_currentFrisbee;
        }
        set
        {
            m_currentFrisbee = value;
        }
    }
    OVRInput.Controller m_usingController;
    [SerializeField]
    float m_throwCheckPower = 7.5f;
    [SerializeField]
    GameObject m_frisbeePre;
    ObjectUsingChecker m_frisbeeList;
	#endregion Member

	// 定数
	#region Constant
	
	#endregion Constant

	// メソッド
	#region Method
	void Start () {
		m_OrderNumber = 0;
		ObjectManager.Instance.RegistrationList(this, m_OrderNumber);

        m_frisbeeList = new ObjectUsingChecker();
        m_frisbeeList.SetObjectParent(m_handObj);
        m_currentFrisbee = m_frisbeeList.GetNewObj(m_frisbeePre);
	}
	
	public override void Execute(float deltaTime) {
        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) {
            m_gaugeObj.IsSet = true;
        }
        if(OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger)) {
            m_gaugeObj.IsSet = false;
            Vector3 acc = OVRInput.GetLocalControllerAngularVelocity(OVRInput.GetActiveController());
            if(acc.magnitude >= m_throwCheckPower) {
                m_gaugeObj.GaugePropotion = 0.01f;
                StartCoroutine(m_currentFrisbee.AutoDelete(2.0f));
                m_currentFrisbee.ObjBody.GetComponent<Frisbee>().Throw(m_eyeObj.transform.forward * acc.magnitude * m_gaugeObj.GaugePropotion * m_throwPower);
                m_currentFrisbee = m_frisbeeList.GetNewObj(m_frisbeePre);
                m_currentFrisbee.ObjBody.GetComponent<Frisbee>().init();
            }
        }
	}

	public override void LateExecute(float deltaTime) {
		
	}
	#endregion Method
}
