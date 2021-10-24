using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Character
{
    [SerializeField] AudioData wolfSound;
    /// <summary>
    /// �T�Y��ϫ�A����ƥ�
    /// </summary>
    protected override void Arrive()
    {
        base.Arrive();
        AudioManager.Instance.PlaySFX(wolfSound);
        if (TargetCha.GetComponent<Sheet>().Status == 1) //�p��
        {
            GameLog.Instance.AddNum(4, 1);
            GameLog.Instance.UpdateEventLog("�T�Y���p�� !");
        }
        else if (TargetCha.GetComponent<Sheet>().Status == 2) //�j��
        {
            GameLog.Instance.AddNum(5, 1);
            GameLog.Instance.UpdateEventLog("�T�Y���� !");
        }
        TargetCha.Die();
    }

    public override void Die()
    {
        base.Die();
    }
}
