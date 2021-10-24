using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sheet : Character
{
    public int Status = 1; //1�O�p��  2�O�j��
    int grassNum = 0;
    [SerializeField] Sprite[] sheepImgs;
    [SerializeField] AudioData eatGrassSound;
    [SerializeField] AudioData coinSound;

    /// <summary>
    /// �]�w�Ϫ�����
    /// </summary>
    public void SetStatus(int StNum)
    {
        if(StNum == 1)
        {
            transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = sheepImgs[0];
            ShowText.text = "�p��";
        }
        else
        {
            transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = sheepImgs[1];
            ShowText.text = "��";
        }
        Status = StNum;
        ActionNum = StNum;
    }

    /// <summary>
    /// �ϦY����A����ƥ�
    /// </summary>
    protected override void Arrive()
    {
        Debug.Log("�ϭn�Y���o !");
        base.Arrive();
        TargetCha.Die();

        grassNum++;
        GameLog.Instance.AddNum(1,1);
        if(Status == 1)
        {
            GameLog.Instance.UpdateEventLog("�p�ϦY�F��");
        }
        else if(Status == 2)
        {
            GameLog.Instance.UpdateEventLog("�ϦY�F��");
        }
        //�Y��T�ӯ�~�|Ĳ�o�ƥ�
        if (grassNum < 3)
        {
            AudioManager.Instance.PlaySFX(eatGrassSound);
            transform.GetChild(0).GetChild(2).GetComponent<Text>().text = grassNum.ToString();
            return;
        }

        AudioManager.Instance.PlaySFX(coinSound);
        grassNum = 0;
        transform.GetChild(0).GetChild(2).GetComponent<Text>().text = grassNum.ToString();
        if (Status == 1) //�p��
        {
            GameLog.Instance.AddNum(2,1);
            GameLog.Instance.AddNum(7, 10);
            SetStatus(2);
            GameLog.Instance.UpdateEventLog("�p�Ϫ��j�F�A�Ȩ�10�T����");
        }
        else if(Status == 2) //�j��
        {
            GameLog.Instance.AddNum(3, 1);
            GameLog.Instance.AddNum(7, 5);
            GameLog.Instance.UpdateEventLog("�ϦY�F�ܦh��A�Ȩ�5�T����");
        }
    }

    public override void Die()
    {
        base.Die();
    }
}
