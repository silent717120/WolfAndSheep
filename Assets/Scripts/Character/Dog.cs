using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Character
{
    [SerializeField] AudioData dogSound;

    /// <summary>
    /// �����]�T��A����ƥ�
    /// </summary>
    protected override void Arrive()
    {
        base.Arrive();
        AudioManager.Instance.PlaySFX(dogSound);
        GameLog.Instance.UpdateEventLog("�����]�F�T !");
        GameLog.Instance.AddNum(6, 1);
        TargetCha.Die();
    }
}
