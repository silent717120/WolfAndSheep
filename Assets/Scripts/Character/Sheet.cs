using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sheet : Character
{
    public int Status = 1; //1是小羊  2是大羊
    int grassNum = 0;
    [SerializeField] Sprite[] sheepImgs;
    [SerializeField] AudioData eatGrassSound;
    [SerializeField] AudioData coinSound;

    /// <summary>
    /// 設定羊的類型
    /// </summary>
    public void SetStatus(int StNum)
    {
        if(StNum == 1)
        {
            transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = sheepImgs[0];
            ShowText.text = "小羊";
        }
        else
        {
            transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = sheepImgs[1];
            ShowText.text = "羊";
        }
        Status = StNum;
        ActionNum = StNum;
    }

    /// <summary>
    /// 羊吃到草後，執行事件
    /// </summary>
    protected override void Arrive()
    {
        Debug.Log("羊要吃草囉 !");
        base.Arrive();
        TargetCha.Die();

        grassNum++;
        GameLog.Instance.AddNum(1,1);
        if(Status == 1)
        {
            GameLog.Instance.UpdateEventLog("小羊吃了草");
        }
        else if(Status == 2)
        {
            GameLog.Instance.UpdateEventLog("羊吃了草");
        }
        //吃到三個草才會觸發事件
        if (grassNum < 3)
        {
            AudioManager.Instance.PlaySFX(eatGrassSound);
            transform.GetChild(0).GetChild(2).GetComponent<Text>().text = grassNum.ToString();
            return;
        }

        AudioManager.Instance.PlaySFX(coinSound);
        grassNum = 0;
        transform.GetChild(0).GetChild(2).GetComponent<Text>().text = grassNum.ToString();
        if (Status == 1) //小羊
        {
            GameLog.Instance.AddNum(2,1);
            GameLog.Instance.AddNum(7, 10);
            SetStatus(2);
            GameLog.Instance.UpdateEventLog("小羊長大了，賺取10枚金幣");
        }
        else if(Status == 2) //大羊
        {
            GameLog.Instance.AddNum(3, 1);
            GameLog.Instance.AddNum(7, 5);
            GameLog.Instance.UpdateEventLog("羊吃了很多草，賺取5枚金幣");
        }
    }

    public override void Die()
    {
        base.Die();
    }
}
