//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
//	UnityChanTest.cs
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

public class UnityChanTest : ObjectBase {
    // メンバ
    #region Member
    [SerializeField]
    Animator anim;
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
        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) {
            anim.SetTrigger("Rest_trigger");
        }
	}

	public override void LateExecute(float deltaTime) {
		
	}
	#endregion Method
}
