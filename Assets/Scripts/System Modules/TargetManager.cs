using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : Singleton<TargetManager>
{
    [Header("場上羊的清單")]
    public List<Character> sheepList;
    [Header("場上草的清單")]
    public List<Character> grassList;
    [Header("場上狼的清單")]
    public List<Character> wolfList;
    [Header("場上犬的清單")]
    public List<Character> dogList;
    [Header("場上全部的清單")]
    public List<Character> AllList;

    private List<float> DistanceList;
    private Dictionary<float, Character> DistanceDic;

    protected override void Awake()
    {
        base.Awake();
        sheepList = new List<Character>();
        grassList = new List<Character>();
        wolfList = new List<Character>();
        dogList = new List<Character>();
        AllList = new List<Character>();
        DistanceList = new List<float>();
        DistanceDic = new Dictionary<float, Character>();
    }

    /// <summary>
    /// 傳入本身位置，需要取得距離最近的類型 1是羊  2是草  3是狼
    /// </summary>
    public Character GetCharacter(Transform myTran,int TargetIndex)
    {
        List<Character> TargetList = TargetIndex == 1 ? sheepList : TargetIndex == 2 ? grassList : TargetIndex == 3 ? wolfList : sheepList;

        //沒有目標就不移動
        if(TargetList.Count == 0)
        {
            return null;
        }
        Character targetTemp = TargetList[0];
        float dis = Mathf.Abs(myTran.position.x - TargetList[0].transform.position.x) + Mathf.Abs(myTran.position.z - TargetList[0].transform.position.z);
        float disTemp;


        for (int i = 1; i < TargetList.Count; i++)
        {
            //disTemp = Vector3.Distance(transform.position, TargetList[i].transform.position); //計算斜線距離
            disTemp = Mathf.Abs(myTran.position.x - TargetList[i].transform.position.x) + Mathf.Abs(myTran.position.z - TargetList[i].transform.position.z); //計算直角距離
            if (disTemp < dis)
            {
                targetTemp = TargetList[i];
                dis = disTemp;
            }
        }

        return targetTemp;
    }
}
