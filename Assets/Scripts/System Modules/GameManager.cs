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
    [Header("初始單位數量設定")]
    public StartUnitNum startUnitNum;
    [Header("初始單位數量設定")]
    public MaxUnitNum maxUnitNum;
    [Header("角色預製物")]
    public GameObject[] characterPrebfabs;
    [Header("小羊產生回合")]
    public int lambRoundNum = 0;
    [Header("狼產生回合")]
    public int wolfRoundNum = 5;
    [Header("自動下一回合")]
    public Toggle AutoToggle;
    [Header("遊戲速度")]
    public SpeedState speedState = SpeedState.normal;
    public UnityEvent speeEvent = new UnityEvent();
    [Header("狼出現音效")]
    public AudioData wolfShowSound;

    public enum SpeedState
    {
        normal,
        fast,
    }

    public bool IsMoving = false; //移動結束
    public bool IsRoundEnd = true; //回合結束

    WaitUntil waitMoveEnd; //是否移動結束
    WaitForSeconds waitForNextUnitTime; //下一個開始移動間隔
    WaitForSeconds waitForNextRoundTime; //自動下一回合間隔

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
    /// 初始化地圖，回合數，生成第一回合的單位
    /// </summary>
    private void Init()
    {
        GridMeshCreate.Instance.gridEvent = GridEvent;
        GridMeshCreate.Instance.CreateMesh(); //產生地圖

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
    /// 單一網格初始化，用Action呼叫
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
    /// 執行回合動作
    /// </summary>
    public void NextRound()
    {
        if (!IsRoundEnd) return;
        StartCoroutine(nameof(NextRoundCoroutine));
    }
    IEnumerator NextRoundCoroutine()
    {
        Debug.Log("第"+ GameLog.Instance.roundNum+"回合啟動");

        IsRoundEnd = false;

        //----------移動----------//

        //綿羊移動
        for (int i = 0; i < TargetManager.Instance.sheepList.Count; i++)
        {
            Debug.Log("羊"+i+" 開始移動");
            IsMoving = true;
            TargetManager.Instance.sheepList[i].StartMove();
            yield return waitMoveEnd;
            Debug.Log("移動結束");
            yield return waitForNextUnitTime;
            GridMeshCreate.Instance.ResetMeshType();
        }
        //狼移動
        for (int i = 0; i < TargetManager.Instance.wolfList.Count; i++)
        {
            Debug.Log("狼" + i + " 開始移動");
            IsMoving = true;
            TargetManager.Instance.wolfList[i].StartMove();
            yield return waitMoveEnd;
            Debug.Log("移動結束");
            yield return waitForNextUnitTime;
            GridMeshCreate.Instance.ResetMeshType();
        }
        //犬移動
        for (int i = 0; i < TargetManager.Instance.dogList.Count; i++)
        {
            Debug.Log("犬" + i + " 開始移動");
            IsMoving = true;
            TargetManager.Instance.dogList[i].StartMove();
            yield return waitMoveEnd;
            Debug.Log("移動結束");
            yield return waitForNextUnitTime;
            GridMeshCreate.Instance.ResetMeshType();
        }

        //----------產生新單位----------//

        //當場內羊及小羊總數< 10隻時每回合隨機生5個草
        if (TargetManager.Instance.sheepList.Count < 10)
        {
            GameLog.Instance.UpdateEventLog("生成5個草");
            for (int Num = 0; Num < 5; Num++)
            {
                SpawnUnit(1);
            }
        }
        //當場內羊及小羊總數>=10隻時每回合隨機產生3個草
        else if (TargetManager.Instance.sheepList.Count >= 10)
        {
            GameLog.Instance.UpdateEventLog("生成3個草");
            for (int Num = 0; Num < 3; Num++)
            {
                SpawnUnit(1);
            }
        }

        //隔5回合才可生成一次
        lambRoundNum--;
        if (lambRoundNum <= 0)
        {
            //當場內羊及小羊總數<=10隻時每5回合會在地圖上隨機多2隻小羊
            if (TargetManager.Instance.sheepList.Count <= 10)
            {
                GameLog.Instance.UpdateEventLog("生成2隻小羊");
                for (int Num = 0; Num < 2; Num++)
                {
                    SpawnUnit(0).GetComponent<Sheet>().SetStatus(1);
                }
            }
            //當場內羊及小羊總數> 10隻時每5回合會在地圖上隨機多1隻小羊
            else if (TargetManager.Instance.sheepList.Count > 10)
            {
                GameLog.Instance.UpdateEventLog("生成1隻小羊");
                for (int Num = 0; Num < 1; Num++)
                {
                    SpawnUnit(0).GetComponent<Sheet>().SetStatus(1);
                }
            }
            lambRoundNum = 5;
        }

        //狼不在場上才可倒數
        if(TargetManager.Instance.wolfList.Count == 0)
        {
            wolfRoundNum--;
        }
        if (wolfRoundNum <= 0)
        {
            AudioManager.Instance.PlaySFX(wolfShowSound);
            GameLog.Instance.UpdateEventLog("狼出現了");
            SpawnUnit(2);
            wolfRoundNum = UnityEngine.Random.Range(6,10);
            Debug.Log("下次出現狼，幾回合後:"+wolfRoundNum);
        }

        IsRoundEnd = true;

        GameLog.Instance.AddNum(0, 1);

        //自動下一回合
        if (AutoToggle.isOn)
        {
            yield return waitForNextRoundTime;
            NextRound();
        }
    }

    /// <summary>
    /// 監測不重複的位置後，生成該單位  0羊  1草  2狼  3狗
    /// </summary>
    /// <param name="PrebfabIndex">需產生的單位編號</param>
    public GameObject SpawnUnit(int PrebfabIndex)
    {
        //檢測位置是否有單位了
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
        //角色高度固定在1
        Vector3 NewPos = new Vector3(NewGrid.transform.position.x,1, NewGrid.transform.position.z);
        //設定角色狀態
        GameObject NewChar = PoolManager.Release(characterPrebfabs[PrebfabIndex], NewPos);
        NewChar.GetComponent<Character>().GridPosition = NewGrid;
        return NewChar;
    }

    /// <summary>
    /// 更新目前無法走的網格，只有目標跟本身不算在內
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
    /// 切換速度，需要更改速度的地方會自動取得狀態判斷
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
        //觸發所有需要改速度的事件
        speeEvent.Invoke();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
