using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [Header("����")]
    public int Type; //1�O��  2�O��  3�O�T  4�O��

    [Header("��ʤO")]
    public int ActionNum;

    [Header("�ؼ�")]
    public int TargetIndex;
    public Character TargetCha;

    [Header("��m")]
    public Grid GridPosition;

    [Header("���ʸ��|")]
    public List<Grid> TargetRodes = new List<Grid>();

    [Header("���ʭ���")]
    public AudioData[] walkSounds;

    //�t�ק�s�e�U
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
        //���U�t�ק�s�e�U
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
    /// �]�w�ؼи���|��}�l����
    /// </summary>
    public void StartMove()
    {
        //�]�w�ؼ�
        TargetCha = TargetManager.Instance.GetCharacter(transform, TargetIndex);
        if (TargetCha == null)
        {
            GameManager.Instance.IsMoving = false;
            return;
        }
        //��s��ê�I
        GameManager.Instance.UpdateHinderGird(GridPosition, TargetCha.GridPosition);
        //���o���|
        TargetRodes = AStarLookRode.Instance.GetRodeFast(GridPosition, TargetCha.GridPosition);
        //�}�l����
        StartCoroutine(nameof(MoveCoroutine));
    }

    IEnumerator MoveCoroutine()
    {
        int NowActionNum = ActionNum;

        yield return waitForMoveTime;

        for (int i = 1; i < TargetRodes.Count; i++)
        {
            //��ʤO-1
            NowActionNum--;
            //�]�w�U�@�B��m
            TargetPos = new Vector3(TargetRodes[i].transform.position.x, transform.position.y, TargetRodes[i].transform.position.z);
            //��s�Ҧb����
            GridPosition = TargetRodes[i];
            //�����n��
            AudioManager.Instance.PlayRandomSFX(walkSounds);
            //���q�Ҧ��A���`����
            if (GameManager.Instance.speedState == GameManager.SpeedState.normal)
            {
                IsMove = true;
            }
            //�ֳt�Ҧ��A������Ӧ�m
            else if (GameManager.Instance.speedState == GameManager.SpeedState.fast)
            {
                transform.position = TargetPos;
            }
            yield return waitForMoveTime;
            IsMove = false;

            //��F�ؼа����k
            if (i == TargetRodes.Count - 1)
            {
                Arrive();
            }
            //��ʤO����
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
        Debug.Log("����s�t�� !");
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
    /// ��F�ؼ�
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
