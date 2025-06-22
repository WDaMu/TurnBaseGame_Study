/*
    GridManager.cs
    Function:
        1. 管理整个网格系统，包括网格上的Object，Unit
        2. 单例模式提供全局访问入口。
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{   
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int gridSize; 
    [SerializeField] private Vector3 origin;

    [SerializeField] private Transform gridDebugInfoPrefab; 
    [SerializeField] private bool isDebugMode = true;
    public static GridManager Instance { get; private set; }
    private GridSystem<UnitObject> gridSystem;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gridSystem = new GridSystem<UnitObject>(width, height, gridSize, origin, (GridSystem<UnitObject> gridSystem, GridPosition gridPosition)=> new UnitObject(gridSystem, gridPosition));

        if (isDebugMode)
        {
            gridSystem.CreateGridDebugObject(gridDebugInfoPrefab);
        }
    }
    void Start()
    {
        Pathfinding.Instance.SetUp(width, height, gridSize, origin);

        BaseAction.onActionEnd += BaseAction_OnActionEnd;
        if (isDebugMode)
        {
            gridSystem.UpdateDebugInfo();
        }
    }

    public void SetUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        UnitObject gridObject = GetGridObject(gridPosition);
        gridObject.SetUnit(unit);
    }
    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        UnitObject gridObject = GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }
    public void ClearUnitAtGridPosition(GridPosition gridPosition)
    {
        UnitObject gridObject = GetGridObject(gridPosition);
        gridObject.SetUnit(null);
    }
    public void MoveUnitToGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        ClearUnitAtGridPosition(fromGridPosition);
        SetUnitAtGridPosition(toGridPosition, unit);
    }
    public List<GridPosition> GetAvailableGrid(BaseAction selectedAction, Unit selectedUnit)
    {
        /*
            功能实现步骤：
            1. 获取动作的范围
            2. 根据动作的类型在范围内获取合法的位置
        */
        List<GridPosition> validGrid = new List<GridPosition>();
        GridPosition unitGridPosition = GetGridPosition(selectedUnit.transform.position);
        int actionRange = selectedAction.GetActionRange();

        // 获取动作范围内的所有网格
        for (int x = - actionRange; x <= actionRange; x++)
        {
            for (int z = - actionRange; z <= actionRange; z++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(z) > actionRange)
                {
                    continue;
                }
                GridPosition itemGridPosition = new GridPosition(x, z) + unitGridPosition;
                if (!IsValidPosition(itemGridPosition))
                {
                    continue;
                }
                validGrid.Add(itemGridPosition);
            }
        }

        // 根据动作类型，过滤范围内合法的位置
        List<GridPosition> validGrid_ = new List<GridPosition>(validGrid); // 复制一份，避免修改原列表（无法在遍历时直接修改列表）
        switch (selectedAction)
        {
            case Move:
                foreach (GridPosition gridPosition in validGrid_)
                {
                    if (GetUnitAtGridPosition(gridPosition) != null)
                    {
                        validGrid.Remove(gridPosition);
                    }
                }
                break;
            case Shoot:
                foreach (GridPosition gridPosition in validGrid_)
                {
                    if (GetUnitAtGridPosition(gridPosition)?.IsEnemy() == false || GetUnitAtGridPosition(gridPosition) == null)
                    {
                        validGrid.Remove(gridPosition);
                    }
                }
                break;
            case Attack:
                foreach (GridPosition gridPosition in validGrid_)
                {
                    if (GetUnitAtGridPosition(gridPosition)?.IsEnemy() == false || GetUnitAtGridPosition(gridPosition) == null)
                    {
                        validGrid.Remove(gridPosition);
                    }
                }
                break;
        }

        return validGrid;
    }
    public bool IsValidPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);
    public GridPosition GetGridPosition(Vector3 position) => gridSystem.GetGridPosition(position);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);
    public void GetGridSystemInfo(out int width, out int height, out int gridSize, out Vector3 origin) => gridSystem.GetGridSystemInfo(out width, out height, out gridSize, out origin);
    public int GetGridSytemWidth() => gridSystem.GetGridSystemWidth();
    public int GetGridSytemHeight() => gridSystem.GetGridSystemHeight();
    public int GetGridSytemGridSize() => gridSystem.GetGridSystemGridSize();
    public Vector3 GetGridSytemOrigin() => gridSystem.GetGridSystemOrigin();
    public UnitObject GetGridObject(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition);
    public GridPosition GetUnitGridPosition(Unit unit)
    {
        return GetGridPosition(unit.transform.position);
    }
    private void BaseAction_OnActionEnd(object sender, EventArgs e)
    {
        if (isDebugMode)
        {
            gridSystem.UpdateDebugInfo();
        }
    }
}
