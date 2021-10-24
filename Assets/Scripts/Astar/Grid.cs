using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public int posX;
    public int posY;
    public bool isHinder; //這是障礙
    public bool isShowColor = true; //是否變色
    public Action OnClick;

    //計算計劃路徑三個值
    public int G = 0;
    public int H = 0;
    public int All = 0;

    //記錄在尋路過程中該格子的父格子
    public Grid parentGrid;

    public Text ShowText;
    public void ChangeColor(Color color)
    {
        if (!isShowColor) return;
        gameObject.GetComponent<MeshRenderer>().material.color = color;
    }

    //委託綁定模板點擊事件
    private void OnMouseDown()
    {
        OnClick?.Invoke();
    }
}
