//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
//	ObjectCatcher.cs
//	
//	作成者:佐々木瑞生
//==================================================
//	概要
//	オブジェクトのキャッチ
//
//	
//==================================================
//	作成日：2017/10/25
//	
//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCatcher : ObjectBase {

	// Use this for initialization
	void Start() {
		m_OrderNumber = 0;
		ObjectManager.Instance.RegistrationList(this, m_OrderNumber);

	}

	public override void Execute(float deltaTime) {

	}

	public override void LateExecute(float deltaTime) {

	}

	public void OnCollisionEnter(Collision obj) {
		if(obj.gameObject.tag != "CatchedItem") return;

		obj.transform.parent = gameObject.transform;
	}
}
