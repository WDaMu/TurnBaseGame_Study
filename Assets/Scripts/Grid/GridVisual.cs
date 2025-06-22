/*
    可视化玩家行动网格
    Function:
        根据玩家选择动作显示不同的网格
*/
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class GridVisual : MonoBehaviour
{   
    [SerializeField] private Transform gridVisualSinglePrefab; // 可视化网格
    [SerializeField] private LayerMask obstacleLayerMask; // 障碍物层
    private List<GridPosition> obstacleList;
    private Transform[,] gridVisualSinglePrefabArray;

    private void Awake()
    {
        obstacleList = new List<GridPosition>();
        CaculateObstacleList();

        GridManager.Instance.GetGridSystemInfo(out int width, out int height, out int gridSize, out Vector3 origin);
        gridVisualSinglePrefabArray = new Transform[width, height];
        // 初始化所有网格和调试信息
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // 创建网格显示
                Transform gridVisualSinglePrefabTransform = Instantiate(gridVisualSinglePrefab, new Vector3(x, 0, z) * gridSize + origin, Quaternion.identity);
                gridVisualSinglePrefabArray[x, z] = gridVisualSinglePrefabTransform;
                // 隐藏网格显示
                gridVisualSinglePrefabTransform.GetComponent<GridVisualSingle>().Hide();
            }
        }
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_onSelectedActionChanged;
        BaseAction.onActionEnd += BaseAction_onActionEnd;
    }

    private void UpdateGridVisual()
    {
        HideAllGrid();
        UpdateValidGrid();
    }

    public void CaculateObstacleList()
    {
        int detectOffset = 5;
        for (int x = 0; x < GridManager.Instance.GetGridSytemWidth(); x++)
        {
            for (int z = 0; z < GridManager.Instance.GetGridSytemHeight(); z++)
            {
                Vector3 position = GridManager.Instance.GetWorldPosition(new GridPosition(x, z));
                if (Physics.Raycast(position + Vector3.down * detectOffset, Vector3.up, 2 * detectOffset, obstacleLayerMask))
                {
                    obstacleList.Add(new GridPosition(x, z));
                }
            }
        }

    }

    private void UpdateValidGrid()
    {
        // 根据选中的Unit以及选中的Action显示网格
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        Color actionColor = selectedAction.GetActionColor();
        Color actionColorLight = new Color(actionColor.r, actionColor.g, actionColor.b, 0.2f);
        
        if (selectedUnit == null || selectedAction == null)
        {
            return;
        }

        // 显示技能范围
        int actionRange = selectedAction.GetActionRange();
        GridPosition unitGridPosition = GridManager.Instance.GetGridPosition(selectedUnit.transform.position);

        if (selectedAction.ToString() == "Shoot" || selectedAction.ToString() == "Attack")
        {
            for (int x = - actionRange; x <= actionRange; x++)
            {
                for (int z = - actionRange; z <= actionRange; z++)
                {
                    if (Mathf.Abs(x) + Mathf.Abs(z) > actionRange)
                    {
                        continue;
                    }
                    GridPosition itemGridPosition = new GridPosition(x, z) + unitGridPosition;
                    if (!GridManager.Instance.IsValidPosition(itemGridPosition))
                    {
                        continue;
                    }
                    gridVisualSinglePrefabArray[itemGridPosition.x, itemGridPosition.z].GetComponent<GridVisualSingle>().Show(actionColorLight);

                }
            }  
        }

        // 显示可行动作网格
        List<GridPosition> validGrid = GridManager.Instance.GetAvailableGrid(selectedAction, selectedUnit);
        foreach (GridPosition gridPosition in validGrid)
        {
            // 排除有障碍物的网格
            if (obstacleList.Contains(gridPosition) && selectedAction.ToString() == "Move") // 只有MoveAction才需要排除障碍物
            {
                gridVisualSinglePrefabArray[gridPosition.x, gridPosition.z].GetComponent<GridVisualSingle>().Hide();
                continue;
            }

            gridVisualSinglePrefabArray[gridPosition.x, gridPosition.z].GetComponent<GridVisualSingle>().Show(actionColor);
        }

    }
    private void HideAllGrid()
    {
        foreach (Transform gridVisualSingle in gridVisualSinglePrefabArray)
        {
            gridVisualSingle.GetComponent<GridVisualSingle>().Hide();
        }
    }

    private void UnitActionSystem_onSelectedActionChanged(object sender, BaseAction selected)
    {
        UpdateGridVisual();
    }
    private void BaseAction_onActionEnd(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

}
