//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
//	TargetCreator.cs
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

public class TargetCreator : ObjectBase {
    // メンバ
    #region Member
    ObjectUsingChecker m_targetList;
    [SerializeField]
    GameObject m_targetPre;
    [SerializeField]
    float m_intervalTime = 0.5f;
    [SerializeField]
    float m_lifeTime = 1.0f;
    [SerializeField]
    Vector3[] m_createPoints;
    float m_timer;
	#endregion Member

	// 定数
	#region Constant
	
	#endregion Constant

	// メソッド
	#region Method
	void Start () {
		m_OrderNumber = 0;
		ObjectManager.Instance.RegistrationList(this, m_OrderNumber);
        m_targetList = new ObjectUsingChecker();
	}
	
	public override void Execute(float deltaTime) {
        m_timer -= deltaTime;
        if(m_timer <= 0.0f) {
            int posInd = Random.Range(0, m_createPoints.Length);
            var target = m_targetList.GetNewObj(m_targetPre);
            target.ObjBody.transform.position = m_createPoints[posInd];
            //StartCoroutine(target.AutoDelete(5.0f));
            m_timer = m_intervalTime;
        }
	}

	public override void LateExecute(float deltaTime) {
		
	}
	#endregion Method
}
