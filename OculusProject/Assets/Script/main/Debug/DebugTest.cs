//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
//	DebugTest.cs
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

public class DebugTest : SingletonMonoBehaviour<DebugTest> {
    // メンバ
    #region Member
    [SerializeField]
    Text accText;
    [SerializeField]
    Text lastAccText;
	#endregion Member

	// 定数
	#region Constant
	
	#endregion Constant

	// メソッド
	#region Method
	void Start () {
		m_OrderNumber = 0;
		ObjectManager.Instance.RegistrationList(this, m_OrderNumber);
        lastAccText.text = "";
    }
	
	public override void Execute(float deltaTime) {
        //accText.text = "accAccText:" + OVRInput.GetLocalControllerAngularVelocity(OVRInput.GetActiveController()).magnitude.ToString();
        accText.text = "";
    }

    public override void LateExecute(float deltaTime) {
		
	}

    public void SetLastAcc(float power) {
        lastAccText.text = "";
    }
	#endregion Method
}
