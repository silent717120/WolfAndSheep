using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarLookRode : Singleton<AStarLookRode>
{
    [Header("---產生路徑腳本---")]

    GridMeshCreate meshMap;

    [Header("開始跟結束的格子")]
    public Grid startGrid;
    public Grid endGrid;

    [Header("列表與路徑")]
    public List<Grid> openGrids; //開放列表
    public List<Grid> closeGrids; //閉合列表
    public Stack<Grid> rodes; //路徑列
    public List<Grid> targetRodes = new List<Grid>(); //最終路徑

    [Header("路徑顏色")]
    public Color roadColor;

    private void Start()
    {
        this.meshMap = GridMeshCreate.Instance;
        openGrids = new List<Grid>();
        closeGrids = new List<Grid>();
        rodes = new Stack<Grid>();
    }

    public void Init(Grid startGrid, Grid endGrid)
    {
        this.startGrid = startGrid;
        this.endGrid = endGrid;
        openGrids = new List<Grid>();
        closeGrids = new List<Grid>();
        rodes = new Stack<Grid>();
    }

    /// <summary>
    /// 立即取得路徑
    /// </summary>
    public List<Grid> GetRodeFast(Grid startGrid, Grid endGrid)
    {
        this.startGrid = startGrid;
        this.endGrid = endGrid;
        OnStartFast();
        ResetType();
        return targetRodes;
    }

    /// <summary>
    /// 判斷周圍格子，添加至路徑 (四方向)
    /// </summary>
    public void TraverseItem(int i, int j)
    {
        int xMin = Mathf.Max(i - 1, 0);
        int xMax = Mathf.Min(i + 1, meshMap.meshRange.horizontal - 1);
        int yMin = Mathf.Max(j - 1, 0);
        int yMax = Mathf.Min(j + 1, meshMap.meshRange.vertical - 1);

        Grid item = meshMap.grids[i, j].GetComponent<Grid>();
        //算左右
        for (int x = xMin; x <= xMax; x++)
        {
            Grid grid = meshMap.grids[x, j].GetComponent<Grid>();
            //閉合不算
            if ((i == x) || closeGrids.Contains(grid))
            {
                continue;
            }
            //開放則計算期望值
            if (openGrids.Contains(grid))
            {
                if (item.All > GetLength(grid, item))
                {
                    item.parentGrid = grid;
                    SetNoteData(item);
                }
                continue;
            }
            //都不是則加入開放內
            if (!grid.isHinder)
            {
                openGrids.Add(grid);
                //grid.ChangeColor(Color.blue);
                grid.parentGrid = item;
            }
        }
        //算上下
        for (int y = yMin; y <= yMax; y++)
        {
            Grid grid = meshMap.grids[i, y].GetComponent<Grid>();
            //閉合不算
            if ((y == j) || closeGrids.Contains(grid))
            {
                continue;
            }
            //開放則計算期望值
            if (openGrids.Contains(grid))
            {
                if (item.All > GetLength(grid, item))
                {
                    item.parentGrid = grid;
                    SetNoteData(item);
                }
                continue;
            }
            //都不是則加入開放內
            if (!grid.isHinder)
            {
                openGrids.Add(grid);
                //grid.ChangeColor(Color.blue);
                grid.parentGrid = item;
            }
        }
    }

    /// <summary>
    /// 判斷周圍格子，添加至路徑 (八方向)
    /// </summary>
    public void TraverseItem2(int i, int j)
    {
        int xMin = Mathf.Max(i - 1, 0);
        int xMax = Mathf.Min(i + 1, meshMap.meshRange.horizontal - 1);
        int yMin = Mathf.Max(j - 1, 0);
        int yMax = Mathf.Min(j + 1, meshMap.meshRange.vertical - 1);

        Grid item = meshMap.grids[i, j].GetComponent<Grid>();
        for (int x = xMin; x <= xMax; x++)
        {
            for (int y = yMin; y <= yMax; y++)
            {
                Grid grid = meshMap.grids[x, y].GetComponent<Grid>();
                //閉合不算
                if ((y == j && i == x) || closeGrids.Contains(grid))
                {
                    continue;
                }
                //開放則計算期望值
                if (openGrids.Contains(grid))
                {
                    if (item.All > GetLength(grid, item))
                    {
                        item.parentGrid = grid;
                        SetNoteData(item);
                    }
                    continue;
                }
                //都不是則加入開放內
                if (!grid.isHinder)
                {
                    openGrids.Add(grid);
                    //grid.ChangeColor(Color.blue);
                    grid.parentGrid = item;
                }
            }
        }
    }

    /// <summary>
    /// 計算單一格子期望值
    /// </summary>
    public int SetNoteData(Grid grid)
    {
        Grid itemParent = rodes.Count == 0 ? startGrid : grid.parentGrid;
        int numG = Mathf.Abs(itemParent.posX - grid.posX) + Mathf.Abs(itemParent.posY - grid.posY);
        int n = numG == 1 ? 10 : 14;
        grid.G = itemParent.G + n;

        int numH = Mathf.Abs(endGrid.posX - grid.posX) + Mathf.Abs(endGrid.posY - grid.posY);
        grid.H = numH * 10;
        grid.All = grid.H + grid.G;
        //grid.ShowText.text = grid.All.ToString();
        return grid.All;
    }

    /// <summary>
    /// 求格子到下個格子的期望路徑長度
    /// </summary>
    public int GetLength(Grid bejinGrid, Grid grid)
    {
        int numG = Mathf.Abs(bejinGrid.posX - grid.posX) + Mathf.Abs(bejinGrid.posY - grid.posY);
        int n = numG == 1 ? 10 : 14;
        int G = bejinGrid.G + n;

        int numH = Mathf.Abs(endGrid.posX - grid.posX) + Mathf.Abs(endGrid.posY - grid.posY);
        int H = numH * 10;
        int All = grid.H + grid.G;
        return All;
    }

    /// <summary>
    /// 在開放列表中找到路徑最短的點加入路徑列，同時將路徑點加入到閉合列表中
    /// </summary>
    public void Traverse()
    {
        if (openGrids.Count == 0)
        {
            return;
        }
        Grid minLenthGrid = openGrids[0];
        int minLength = SetNoteData(minLenthGrid);
        for (int i = 0; i < openGrids.Count; i++)
        {
            if (minLength > SetNoteData(openGrids[i])) //找尋最短路徑的格子
            {
                minLenthGrid = openGrids[i];
                minLength = SetNoteData(openGrids[i]);
            }
        }
        //minLenthGrid.ChangeColor(Color.green);

        closeGrids.Add(minLenthGrid);
        openGrids.Remove(minLenthGrid);
        rodes.Push(minLenthGrid);
    }

    /// <summary>
    /// 抵達終點計算期望值最小的路徑
    /// </summary>
    void GetRode()
    {
        targetRodes.Clear();

        rodes.Peek().ChangeColor(roadColor);
        targetRodes.Insert(0, rodes.Pop());

        while (rodes.Count != 0)
        {
            if (targetRodes[0].parentGrid != rodes.Peek())
            {
                rodes.Pop();
            }
            else
            {
                rodes.Peek().ChangeColor(roadColor);
                targetRodes.Insert(0, rodes.Pop());
            }
        }
    }

    /// <summary>
    /// 開始移動
    /// </summary>
    public IEnumerator OnStart()
    {
        //Item itemRoot = Map.bolls[0].item;
        rodes.Push(startGrid);
        closeGrids.Add(startGrid);

        //判斷起始點周圍格子
        TraverseItem(startGrid.posX, startGrid.posY);
        yield return new WaitForSeconds(0.1f);
        Traverse();

        //為了避免無法完成尋路而無法跳出循環，使用For來指定尋路的最大步數
        for (int i = 0; i < 6000; i++)
        {
            //預期路徑到達終點為止
            if (rodes.Peek().posX == endGrid.posX && rodes.Peek().posY == endGrid.posY)
            {
                GetRode();
                break;
            }
            //取得周圍格子預期值
            TraverseItem(rodes.Peek().posX, rodes.Peek().posY);
            yield return new WaitForSeconds(0.03f);
            //獲得最佳路徑
            Traverse();
        }
    }

    /// <summary>
    /// 開始移動(直接完成)
    /// </summary>
    public void OnStartFast()
    {
        //Item itemRoot = Map.bolls[0].item;
        rodes.Push(startGrid);
        closeGrids.Add(startGrid);

        //判斷起始點周圍格子
        TraverseItem(startGrid.posX, startGrid.posY);
        Traverse();

        //為了避免無法完成尋路而無法跳出循環，使用For來指定尋路的最大步數
        for (int i = 0; i < 6000; i++)
        {
            //預期路徑到達終點為止
            if (rodes.Peek().posX == endGrid.posX && rodes.Peek().posY == endGrid.posY)
            {
                GetRode();
                break;
            }
            //取得周圍格子預期值
            TraverseItem(rodes.Peek().posX, rodes.Peek().posY);
            //獲得最佳路徑
            Traverse();
        }
    }

    /// <summary>
    /// 清除路徑記錄
    /// </summary>
    public void ResetType()
    {
        this.startGrid = null;
        this.endGrid = null;
        openGrids.Clear();
        closeGrids.Clear();
        rodes.Clear();
    }
}
