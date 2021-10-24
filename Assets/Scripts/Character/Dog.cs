using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Character
{
    [SerializeField] AudioData dogSound;

    /// <summary>
    /// 犬趕跑狼後，執行事件
    /// </summary>
    protected override void Arrive()
    {
        base.Arrive();
        AudioManager.Instance.PlaySFX(dogSound);
        GameLog.Instance.UpdateEventLog("犬趕跑了狼 !");
        GameLog.Instance.AddNum(6, 1);
        TargetCha.Die();
    }
}
