using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLog : Singleton<GameLog>
{
    //回合
    public int roundNum { get; set; }
    //草被吃的次數
    public int eatGrassNum { get; set; }
    //小羊吃滿草的次數
    public int lambGrowUpNum { get; set; }
    //羊吃滿草的次數
    public int sheetGrowUpNum { get; set; }
    //狼吃掉小羊的次數
    public int wolfEatLambNum { get; set; }
    //狼吃掉羊的次數
    public int wolfEatsheetNum { get; set; }
    //犬攻擊狼的次數
    public int dogAttackWolfNum { get; set; }
    //總共獲得金幣
    public int coinNum { get; set; }

    [Header("統計UI")]
    public Text roundText;
    public Text eatGrassText;
    public Text lambGrowUpText;
    public Text sheetGrowUpText;
    public Text wolfEatLambText;
    public Text wolfEatsheetText;
    public Text dogAttackWolfText;
    public Text coinNumText;
    [Header("事件UI")]
    public Text[] eventLogText;

    /// <summary>
    /// 多種記錄共用，加入記錄
    /// </summary>
    /// <param name="Type">目標記錄</param>
    ///     /// <param name="Num">增加數量</param>
    public void AddNum(int Type,int Num)
    {
        switch (Type)
        {
            case 0:
                roundNum += Num;
                roundText.text = "第"+ roundNum.ToString() + "回合";
                UpdateEventLog("進入"+ roundText.text+" !");
                break;
            case 1:
                eatGrassNum += Num;
                eatGrassText.text = eatGrassNum.ToString() + "次";
                break;
            case 2:
                lambGrowUpNum += Num;
                lambGrowUpText.text = lambGrowUpNum.ToString() + "次";
                break;
            case 3:
                sheetGrowUpNum += Num;
                sheetGrowUpText.text = sheetGrowUpNum.ToString() + "次";
                break;
            case 4:
                wolfEatLambNum += Num;
                wolfEatLambText.text = wolfEatLambNum.ToString() + "次";
                break;
            case 5:
                wolfEatsheetNum += Num;
                wolfEatsheetText.text = wolfEatsheetNum.ToString() + "次";
                break;
            case 6:
                dogAttackWolfNum += Num;
                dogAttackWolfText.text = dogAttackWolfNum.ToString() + "次";
                break;
            case 7:
                coinNum += Num;
                coinNumText.text = coinNum.ToString() + "枚";
                break;
            default:
                break;
        }
    }

    int LogIndex = 0;

    /// <summary>
    /// 更新事件顯示記錄，訊息會由下往上替換
    /// </summary>
    /// <param name="NewLog">新事件記錄</param>
    public void UpdateEventLog(string NewLog)
    {
        for(int i = eventLogText.Length-1; i > 0; i--)
        {
            eventLogText[i].text = eventLogText[i-1].text;
        }
        eventLogText[0].text = NewLog;
    }
}
