using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTest : MonoBehaviour
{
    //是否創造地圖了
    bool isCreateMap = false;
    int clickNum = 0;
    Grid startGrid;
    Grid endGrid;

    private void Start()
    {
        //產生地圖
        Run();
        isCreateMap = false;
        clickNum = 0;
    }

    private void Update()
    {
        //開始尋路
        if (Input.GetKeyDown(KeyCode.Q) && isCreateMap)
        {
            AStarLookRode.Instance.Init(startGrid, endGrid);
            StartCoroutine(AStarLookRode.Instance.OnStart());
        }
        //清除計錄
        if (Input.GetKeyDown(KeyCode.E) && isCreateMap)
        {
            GridMeshCreate.Instance.ResetMeshType();
            AStarLookRode.Instance.ResetType();
            clickNum = 0;
            isCreateMap = false;
            startGrid = null;
            endGrid = null;
        }
    }
    private void Run()
    {
        GridMeshCreate.Instance.gridEvent = GridEvent;
        GridMeshCreate.Instance.CreateMesh();
    }

    /// <summary>
    /// 創建執行的方法，通過委託時創建 (需要點擊的話)
    /// </summary>
    /// <param name="grid"></param>
    private void GridEvent(GameObject go, int row, int column)
    {
        Grid grid = go.GetComponent<Grid>();
        grid.ChangeColor(Color.white);
        grid.isHinder = false;
        grid.posX = row;
        grid.posY = column;

        //模板點擊事件
        grid.OnClick = () => {

            if(clickNum < 3)
            {
                clickNum++;
            }

            //起始點不能設定成障礙
            if (grid == startGrid || grid == startGrid) return;
            //1是起始點   2終點  3障礙  4正常
            switch (clickNum)
            {
                case 1:
                    startGrid = grid;
                    grid.ChangeColor(Color.yellow);
                    Debug.Log("設定起始點");
                    break;
                case 2:
                    endGrid = grid;
                    grid.ChangeColor(Color.yellow);
                    isCreateMap = true;
                    Debug.Log("設定終點:"+ endGrid.posX+ " , " +endGrid.posY);
                    break;
                case 3:
                    if (grid.isHinder)
                    {
                        grid.ChangeColor(Color.white);
                    }
                    else
                    {
                        grid.ChangeColor(Color.red);
                    }
                    grid.isHinder = !grid.isHinder;
                    Debug.Log("設定障礙");
                    break;
                default:
                    break;
            }

        };

    }
}
