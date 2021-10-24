using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMeshCreate : Singleton<GridMeshCreate>
{
    [Serializable]
    public class MeshRange
    {
        public int horizontal;
        public int vertical;
    }
    [Header("---網格創建腳本---")]
    [Header("網格地圖範圍")]
    public MeshRange meshRange;
    [Header("網格地圖起始點")]
    private Vector3 startPos;
    [Header("創建地圖網格父節點")]
    public Transform parentTran;
    [Header("網格地圖模板預製物")]
    public GameObject gridPre;
    [Header("網格地圖模板大小")]
    public Vector2 scale;
    [Header("網格顏色")]
    public Color gridColor;


    private GameObject[,] m_grids;
    public GameObject[,] grids
    {
        get
        {
            return m_grids;
        }
    }

    //註冊模板事件
    public Action<GameObject, int, int> gridEvent;

    /// <summary>
    /// 基於掛載組件的初始數據創建網格
    /// </summary>
    public void CreateMesh()
    {
        if (meshRange.horizontal == 0 || meshRange.vertical == 0)
        {
            return;
        }
        ClearMesh();
        m_grids = new GameObject[meshRange.horizontal, meshRange.vertical];
        for (int i = 0; i < meshRange.horizontal; i++)
        {
            for (int j = 0; j < meshRange.vertical; j++)
            {
                CreateGrid(i, j);
            }
        }
    }

    /// <summary>
    /// 多載，基於傳入寬高數據來創建網格
    /// </summary>
    /// <param name="height"></param>
    /// <param name="widght"></param>
    public void CreateMesh(int height, int widght)
    {
        if (widght == 0 || height == 0)
        {
            return;
        }
        ClearMesh();
        m_grids = new GameObject[widght, height];
        for (int i = 0; i < widght; i++)
        {
            for (int j = 0; j < height; j++)
            {
                CreateGrid(i, j);

            }
        }
    }

    /// <summary>
    /// 根據位置創建一個基本的Grid物體
    /// </summary>
    /// <param name="row">x軸座標</param>
    /// <param name="column">y軸座標</param>
    public void CreateGrid(int row, int column)
    {
        GameObject go = GameObject.Instantiate(gridPre, parentTran);
        //T grid = go.GetComponent<T>();

        float posX = startPos.x + scale.x * row;
        float posZ = startPos.z + scale.y * column;
        go.transform.position = new Vector3(posX, startPos.y, posZ);
        go.SetActive(true);
        m_grids[row, column] = go;
        gridEvent?.Invoke(go, row, column);
    }

    /// <summary>
    /// 隨機獲得一個方格
    /// </summary>
    public Grid RandomGetGrid()
    {
        int PosX = UnityEngine.Random.Range(0, meshRange.horizontal);
        int PosY = UnityEngine.Random.Range(0, meshRange.vertical);
       return  m_grids[PosX, PosY].GetComponent<Grid>();
    }

    /// <summary>
    /// 清除網格記錄，重製回普通網格
    /// </summary>
    public void ResetMeshType()
    {
        foreach (GameObject go in m_grids)
        {
            Grid grid = go.GetComponent<Grid>();
            grid.ChangeColor(gridColor);
            grid.ShowText.text = "";
            grid.isHinder = false;
        }
    }

    /// <summary>
    /// 刪除網格地圖，並清除緩存數據
    /// </summary>
    public void ClearMesh()
    {
        if (m_grids == null || m_grids.Length == 0)
        {
            return;
        }
        foreach (GameObject go in m_grids)
        {
            if (go != null)
            {
                Destroy(go);
            }
        }
        Array.Clear(m_grids, 0, m_grids.Length);
    }
}
