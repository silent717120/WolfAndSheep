using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLog : Singleton<GameLog>
{
    //�^�X
    public int roundNum { get; set; }
    //��Q�Y������
    public int eatGrassNum { get; set; }
    //�p�ϦY���󪺦���
    public int lambGrowUpNum { get; set; }
    //�ϦY���󪺦���
    public int sheetGrowUpNum { get; set; }
    //�T�Y���p�Ϫ�����
    public int wolfEatLambNum { get; set; }
    //�T�Y���Ϫ�����
    public int wolfEatsheetNum { get; set; }
    //�������T������
    public int dogAttackWolfNum { get; set; }
    //�`�@��o����
    public int coinNum { get; set; }

    [Header("�έpUI")]
    public Text roundText;
    public Text eatGrassText;
    public Text lambGrowUpText;
    public Text sheetGrowUpText;
    public Text wolfEatLambText;
    public Text wolfEatsheetText;
    public Text dogAttackWolfText;
    public Text coinNumText;
    [Header("�ƥ�UI")]
    public Text[] eventLogText;

    /// <summary>
    /// �h�ذO���@�ΡA�[�J�O��
    /// </summary>
    /// <param name="Type">�ؼаO��</param>
    ///     /// <param name="Num">�W�[�ƶq</param>
    public void AddNum(int Type,int Num)
    {
        switch (Type)
        {
            case 0:
                roundNum += Num;
                roundText.text = "��"+ roundNum.ToString() + "�^�X";
                UpdateEventLog("�i�J"+ roundText.text+" !");
                break;
            case 1:
                eatGrassNum += Num;
                eatGrassText.text = eatGrassNum.ToString() + "��";
                break;
            case 2:
                lambGrowUpNum += Num;
                lambGrowUpText.text = lambGrowUpNum.ToString() + "��";
                break;
            case 3:
                sheetGrowUpNum += Num;
                sheetGrowUpText.text = sheetGrowUpNum.ToString() + "��";
                break;
            case 4:
                wolfEatLambNum += Num;
                wolfEatLambText.text = wolfEatLambNum.ToString() + "��";
                break;
            case 5:
                wolfEatsheetNum += Num;
                wolfEatsheetText.text = wolfEatsheetNum.ToString() + "��";
                break;
            case 6:
                dogAttackWolfNum += Num;
                dogAttackWolfText.text = dogAttackWolfNum.ToString() + "��";
                break;
            case 7:
                coinNum += Num;
                coinNumText.text = coinNum.ToString() + "�T";
                break;
            default:
                break;
        }
    }

    int LogIndex = 0;

    /// <summary>
    /// ��s�ƥ���ܰO���A�T���|�ѤU���W����
    /// </summary>
    /// <param name="NewLog">�s�ƥ�O��</param>
    public void UpdateEventLog(string NewLog)
    {
        for(int i = eventLogText.Length-1; i > 0; i--)
        {
            eventLogText[i].text = eventLogText[i-1].text;
        }
        eventLogText[0].text = NewLog;
    }
}
