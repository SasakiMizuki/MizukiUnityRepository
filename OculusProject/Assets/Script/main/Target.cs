using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : ObjectBase
{
    // メンバ
    #region Member
    [SerializeField]
    ParticleSystem destroyPart;
    #endregion Member

    // 定数
    #region Constant

    #endregion Constant

    // メソッド
    #region Method
    void Start()
    {
        m_OrderNumber = 0;
        ObjectManager.Instance.RegistrationList(this, m_OrderNumber);
    }

    public override void Execute(float deltaTime)
    {

    }

    public override void LateExecute(float deltaTime)
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag != "frisbee") return;
        //destroyPart.Play();
        /*
         * Score.Instance.addScore(100);
         */
        StartCoroutine(death(0.0f));
    }

    public IEnumerator death(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        gameObject.SetActive(false);
    }
    #endregion Method
}
