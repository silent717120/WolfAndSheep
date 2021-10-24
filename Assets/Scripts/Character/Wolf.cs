using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Character
{
    [SerializeField] AudioData wolfSound;
    /// <summary>
    /// 狼吃到羊後，執行事件
    /// </summary>
    protected override void Arrive()
    {
        base.Arrive();
        AudioManager.Instance.PlaySFX(wolfSound);
        if (TargetCha.GetComponent<Sheet>().Status == 1) //小羊
        {
            GameLog.Instance.AddNum(4, 1);
            GameLog.Instance.UpdateEventLog("狼吃掉小羊 !");
        }
        else if (TargetCha.GetComponent<Sheet>().Status == 2) //大羊
        {
            GameLog.Instance.AddNum(5, 1);
            GameLog.Instance.UpdateEventLog("狼吃掉羊 !");
        }
        TargetCha.Die();
    }

    public override void Die()
    {
        base.Die();
    }
}
