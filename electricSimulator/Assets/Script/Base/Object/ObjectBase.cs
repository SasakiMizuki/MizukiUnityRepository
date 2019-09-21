
//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
//	ObjectBase.cs
//	
//	作成者:佐々木瑞生
//==================================================
//	概要
//	オブジェクトのベース
//
//	
//==================================================
//	作成日：2017/03/12
//
//==================================================
//  更新
//  2018/05/08：停止時更新
//	
//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase : MonoBehaviour {
	protected int m_OrderNumber = 0;
    protected int m_ListToNumber = 0;
    protected float m_DeltaTime = 0;    // 他の関数で使いたいときに入れる用

    public int ListToNumber {
        get
        {
            return m_ListToNumber;
        }

        set
        {
            m_ListToNumber = value;
        }
    }

    // Use this for initialization
    void Start () {
        ObjectManager.Instance.RegistrationList(this,m_OrderNumber);
	}
	
	/// <summary>
	/// 更新
	/// </summary>
	/// <param name="deltaTime">デルタタイム</param>
    public virtual void Execute(float deltaTime) {

    }

    /// <summary>
    /// 後更新
    /// </summary>
    /// <param name="deltaTime">デルタタイム</param>
    public virtual void LateExecute(float deltaTime) {

    }
    
    /// <summary>
    /// ポーズ時更新
    /// </summary>
    /// <param name="deltaTime">デルタタイム</param>
    public virtual void PauseExecute(float unscaledDeltaTime) {

    }

    /// <summary>
    /// ポーズになったタイミングで実行
    /// </summary>
    public virtual void OnPause() {

    }

    /*
	public void ChangeOrderNumber(int OrderNumber) {

	}
	*/
}
