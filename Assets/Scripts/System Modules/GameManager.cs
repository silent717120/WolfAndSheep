using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [Serializable]
    public struct StartUnitNum
    {
        public int SheepNum;
        public int GrassNum;
        public int WolfNum;
        public int DogNum;
    }
    [Serializable]
    public struct MaxUnitNum
    {
        public int SheepNum;
        public int GrassNum;
        public int WolfNum;
        public int DogNum;
    }
    [Header("��l���ƶq�]�w")]
    public StartUnitNum startUnitNum;
    [Header("��l���ƶq�]�w")]
    public MaxUnitNum maxUnitNum;
    [Header("����w�s��")]
    public GameObject[] characterPrebfabs;
    [Header("�p�ϲ��ͦ^�X")]
    public int lambRoundNum = 0;
    [Header("�T���ͦ^�X")]
    public int wolfRoundNum = 5;
    [Header("�۰ʤU�@�^�X")]
    public Toggle AutoToggle;
    [Header("�C���t��")]
    public SpeedState speedState = SpeedState.normal;
    public UnityEvent speeEvent = new UnityEvent();
    [Header("�T�X�{����")]
    public AudioData wolfShowSound;

    public enum SpeedState
    {
        normal,
        fast,
    }

    public bool IsMoving = false; //���ʵ���
    public bool IsRoundEnd = true; //�^�X����

    WaitUntil waitMoveEnd; //�O�_���ʵ���
    WaitForSeconds waitForNextUnitTime; //�U�@�Ӷ}�l���ʶ��j
    WaitForSeconds waitForNextRoundTime; //�۰ʤU�@�^�X���j

    protected override void Awake()
    {
        base.Awake();
        waitMoveEnd = new WaitUntil(() => !IsMoving);
        waitForNextUnitTime = new WaitForSeconds(0.6f);
        waitForNextRoundTime = new WaitForSeconds(1f);
    }

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        speeEvent.RemoveAllListeners();
    }

    /// <summary>
    /// ��l�Ʀa�ϡA�^�X�ơA�ͦ��Ĥ@�^�X�����
    /// </summary>
    private void Init()
    {
        GridMeshCreate.Instance.gridEvent = GridEvent;
        GridMeshCreate.Instance.CreateMesh(); //���ͦa��

        GameLog.Instance.roundNum = 1;
        for (int Num = 0; Num < startUnitNum.SheepNum; Num++)
        {
            SpawnUnit(0).GetComponent<Sheet>().SetStatus(2);
        }
        for (int Num = 0; Num < startUnitNum.GrassNum; Num++)
        {
            SpawnUnit(1);
        }
        for (int Num = 0; Num < startUnitNum.WolfNum; Num++)
        {
            SpawnUnit(2);
        }
        for (int Num = 0; Num < startUnitNum.DogNum; Num++)
        {
            SpawnUnit(3);
        }
    }

    /// <summary>
    /// ��@�����l�ơA��Action�I�s
    /// </summary>
    private void GridEvent(GameObject go, int row, int column)
    {
        Grid grid = go.GetComponent<Grid>();
        grid.ChangeColor(GridMeshCreate.Instance.gridColor);
        grid.isHinder = false;
        grid.posX = row;
        grid.posY = column;
    }

    /// <summary>
    /// ����^�X�ʧ@
    /// </summary>
    public void NextRound()
    {
        if (!IsRoundEnd) return;
        StartCoroutine(nameof(NextRoundCoroutine));
    }
    IEnumerator NextRoundCoroutine()
    {
        Debug.Log("��"+ GameLog.Instance.roundNum+"�^�X�Ұ�");

        IsRoundEnd = false;

        //----------����----------//

        //���ϲ���
        for (int i = 0; i < TargetManager.Instance.sheepList.Count; i++)
        {
            Debug.Log("��"+i+" �}�l����");
            IsMoving = true;
            TargetManager.Instance.sheepList[i].StartMove();
            yield return waitMoveEnd;
            Debug.Log("���ʵ���");
            yield return waitForNextUnitTime;
            GridMeshCreate.Instance.ResetMeshType();
        }
        //�T����
        for (int i = 0; i < TargetManager.Instance.wolfList.Count; i++)
        {
            Debug.Log("�T" + i + " �}�l����");
            IsMoving = true;
            TargetManager.Instance.wolfList[i].StartMove();
            yield return waitMoveEnd;
            Debug.Log("���ʵ���");
            yield return waitForNextUnitTime;
            GridMeshCreate.Instance.ResetMeshType();
        }
        //������
        for (int i = 0; i < TargetManager.Instance.dogList.Count; i++)
        {
            Debug.Log("��" + i + " �}�l����");
            IsMoving = true;
            TargetManager.Instance.dogList[i].StartMove();
            yield return waitMoveEnd;
            Debug.Log("���ʵ���");
            yield return waitForNextUnitTime;
            GridMeshCreate.Instance.ResetMeshType();
        }

        //----------���ͷs���----------//

        //������ϤΤp���`��< 10���ɨC�^�X�H����5�ӯ�
        if (TargetManager.Instance.sheepList.Count < 10)
        {
            GameLog.Instance.UpdateEventLog("�ͦ�5�ӯ�");
            for (int Num = 0; Num < 5; Num++)
            {
                SpawnUnit(1);
            }
        }
        //������ϤΤp���`��>=10���ɨC�^�X�H������3�ӯ�
        else if (TargetManager.Instance.sheepList.Count >= 10)
        {
            GameLog.Instance.UpdateEventLog("�ͦ�3�ӯ�");
            for (int Num = 0; Num < 3; Num++)
            {
                SpawnUnit(1);
            }
        }

        //�j5�^�X�~�i�ͦ��@��
        lambRoundNum--;
        if (lambRoundNum <= 0)
        {
            //������ϤΤp���`��<=10���ɨC5�^�X�|�b�a�ϤW�H���h2���p��
            if (TargetManager.Instance.sheepList.Count <= 10)
            {
                GameLog.Instance.UpdateEventLog("�ͦ�2���p��");
                for (int Num = 0; Num < 2; Num++)
                {
                    SpawnUnit(0).GetComponent<Sheet>().SetStatus(1);
                }
            }
            //������ϤΤp���`��> 10���ɨC5�^�X�|�b�a�ϤW�H���h1���p��
            else if (TargetManager.Instance.sheepList.Count > 10)
            {
                GameLog.Instance.UpdateEventLog("�ͦ�1���p��");
                for (int Num = 0; Num < 1; Num++)
                {
                    SpawnUnit(0).GetComponent<Sheet>().SetStatus(1);
                }
            }
            lambRoundNum = 5;
        }

        //�T���b���W�~�i�˼�
        if(TargetManager.Instance.wolfList.Count == 0)
        {
            wolfRoundNum--;
        }
        if (wolfRoundNum <= 0)
        {
            AudioManager.Instance.PlaySFX(wolfShowSound);
            GameLog.Instance.UpdateEventLog("�T�X�{�F");
            SpawnUnit(2);
            wolfRoundNum = UnityEngine.Random.Range(6,10);
            Debug.Log("�U���X�{�T�A�X�^�X��:"+wolfRoundNum);
        }

        IsRoundEnd = true;

        GameLog.Instance.AddNum(0, 1);

        //�۰ʤU�@�^�X
        if (AutoToggle.isOn)
        {
            yield return waitForNextRoundTime;
            NextRound();
        }
    }

    /// <summary>
    /// �ʴ������ƪ���m��A�ͦ��ӳ��  0��  1��  2�T  3��
    /// </summary>
    /// <param name="PrebfabIndex">�ݲ��ͪ����s��</param>
    public GameObject SpawnUnit(int PrebfabIndex)
    {
        //�˴���m�O�_�����F
        Grid NewGrid = null;
        for(int CheckIndex = 0; CheckIndex < 500; CheckIndex++) {
            if (NewGrid != null) break;
            NewGrid = GridMeshCreate.Instance.RandomGetGrid();
            for (int i = 0; i < TargetManager.Instance.AllList.Count; i++)
            {
                if (TargetManager.Instance.AllList[i].GridPosition == NewGrid)
                {
                    NewGrid = null;
                    break;
                }
            }
        }
        //���Ⱚ�שT�w�b1
        Vector3 NewPos = new Vector3(NewGrid.transform.position.x,1, NewGrid.transform.position.z);
        //�]�w���⪬�A
        GameObject NewChar = PoolManager.Release(characterPrebfabs[PrebfabIndex], NewPos);
        NewChar.GetComponent<Character>().GridPosition = NewGrid;
        return NewChar;
    }

    /// <summary>
    /// ��s�ثe�L�k��������A�u���ؼи򥻨�����b��
    /// </summary>
    public void UpdateHinderGird(Grid myGrid,Grid TargetGrid)
    {
        for (int i = 0; i < TargetManager.Instance.AllList.Count; i++)
        {
            if (TargetManager.Instance.AllList[i].GridPosition == myGrid || TargetManager.Instance.AllList[i].GridPosition == TargetGrid) continue;
            TargetManager.Instance.AllList[i].GridPosition.isHinder = true;
            //TargetManager.Instance.AllList[i].GridPosition.ChangeColor(Color.red);
        }
    }

    /// <summary>
    /// �����t�סA�ݭn���t�ת��a��|�۰ʨ��o���A�P�_
    /// </summary>
    public void ChangeSpeed(int State)
    {
        if(State == 1)
        {
            speedState = SpeedState.normal;
            waitForNextUnitTime = new WaitForSeconds(0.6f);
            waitForNextRoundTime = new WaitForSeconds(1f);
        }
        else if(State == 2)
        {
            speedState = SpeedState.fast;
            waitForNextUnitTime = new WaitForSeconds(0.1f);
            waitForNextRoundTime = new WaitForSeconds(0.2f);
        }
        //Ĳ�o�Ҧ��ݭn��t�ת��ƥ�
        speeEvent.Invoke();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
