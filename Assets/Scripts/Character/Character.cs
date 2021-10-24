using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [Header("類型")]
    public int Type; //1是羊  2是草  3是狼  4是狗

    [Header("行動力")]
    public int ActionNum;

    [Header("目標")]
    public int TargetIndex;
    public Character TargetCha;

    [Header("位置")]
    public Grid GridPosition;

    [Header("移動路徑")]
    public List<Grid> TargetRodes = new List<Grid>();

    [Header("移動音效")]
    public AudioData[] walkSounds;

    //速度更新委託
    public UnityAction UpdateSpeedAction = delegate { };

    bool IsMove = false;
    Vector3 TargetPos;

    protected Text ShowText;

    WaitForSeconds waitForMoveTime;

    private void Awake()
    {
        ShowText = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        waitForMoveTime = new WaitForSeconds(0.5f);
    }

    private void Start()
    {
        //註冊速度更新委託
        UpdateSpeedAction = new UnityAction(ChangeSpeed);
        GameManager.Instance.speeEvent.AddListener(UpdateSpeedAction);
    }

    protected virtual void OnEnable()
    {
        ChangeSpeed();
        switch (Type)
        {
            case 1:
                TargetManager.Instance.sheepList.Add(this);
                break;
            case 2:
                TargetManager.Instance.grassList.Add(this);
                break;
            case 3:
                TargetManager.Instance.wolfList.Add(this);
                break;
            case 4:
                TargetManager.Instance.dogList.Add(this);
                break;
            default:
                break;
        }
        TargetManager.Instance.AllList.Add(this);
    }

    protected virtual void OnDisable()
    {
        switch (Type)
        {
            case 1:
                TargetManager.Instance.sheepList.Remove(this);
                break;
            case 2:
                TargetManager.Instance.grassList.Remove(this);
                break;
            case 3:
                TargetManager.Instance.wolfList.Remove(this);
                break;
            case 4:
                TargetManager.Instance.dogList.Remove(this);
                break;
            default:
                break;
        }
        TargetManager.Instance.AllList.Remove(this);

        TargetCha = null;
        GridPosition = null;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// 設定目標跟路徑後開始移動
    /// </summary>
    public void StartMove()
    {
        //設定目標
        TargetCha = TargetManager.Instance.GetCharacter(transform, TargetIndex);
        if (TargetCha == null)
        {
            GameManager.Instance.IsMoving = false;
            return;
        }
        //刷新障礙點
        GameManager.Instance.UpdateHinderGird(GridPosition, TargetCha.GridPosition);
        //取得路徑
        TargetRodes = AStarLookRode.Instance.GetRodeFast(GridPosition, TargetCha.GridPosition);
        //開始移動
        StartCoroutine(nameof(MoveCoroutine));
    }

    IEnumerator MoveCoroutine()
    {
        int NowActionNum = ActionNum;

        yield return waitForMoveTime;

        for (int i = 1; i < TargetRodes.Count; i++)
        {
            //行動力-1
            NowActionNum--;
            //設定下一步位置
            TargetPos = new Vector3(TargetRodes[i].transform.position.x, transform.position.y, TargetRodes[i].transform.position.z);
            //更新所在網格
            GridPosition = TargetRodes[i];
            //移動聲音
            AudioManager.Instance.PlayRandomSFX(walkSounds);
            //普通模式，正常移動
            if (GameManager.Instance.speedState == GameManager.SpeedState.normal)
            {
                IsMove = true;
            }
            //快速模式，直接到該位置
            else if (GameManager.Instance.speedState == GameManager.SpeedState.fast)
            {
                transform.position = TargetPos;
            }
            yield return waitForMoveTime;
            IsMove = false;

            //抵達目標執行方法
            if (i == TargetRodes.Count - 1)
            {
                Arrive();
            }
            //行動力結束
            if (NowActionNum == 0) break;
        }
        GameManager.Instance.IsMoving = false;
    }

    float t;
    private void Update()
    {
        if (IsMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPos, Time.deltaTime * 2.5f);
        }
    }

    void ChangeSpeed()
    {
        Debug.Log("單位更新速度 !");
        if(GameManager.Instance.speedState == GameManager.SpeedState.normal)
        {
            waitForMoveTime = new WaitForSeconds(0.5f);
        }
        else if(GameManager.Instance.speedState == GameManager.SpeedState.fast)
        {
            waitForMoveTime = new WaitForSeconds(0.05f);
        }
    }

    /// <summary>
    /// 到達目標
    /// </summary>
    protected virtual void Arrive()
    {
        TargetRodes.Clear();
    }

    public virtual void Die()
    {
        gameObject.SetActive(false);
    }
}
