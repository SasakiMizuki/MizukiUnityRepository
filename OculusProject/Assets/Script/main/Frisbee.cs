//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
//	Frisbee.cs
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

public class Frisbee : ObjectBase {
    // メンバ
    #region Member
    [SerializeField]
    Transform m_throwPosition;
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
		
	}

	public override void LateExecute(float deltaTime) {
		
	}

    public void init() {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<CapsuleCollider>().enabled = false;
    }

    public void Throw(Vector3 throwVector) {
        DebugTest.Instance.SetLastAcc(throwVector.magnitude);
        transform.parent = null;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().AddForce(throwVector);
    }

    private IEnumerator autoDelete() {
        yield return new WaitForSeconds(10.0f); 
    }
	#endregion Method
}
