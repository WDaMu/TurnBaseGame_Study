/*
    游戏控制系统
    1. 选择单位
    2. 计算行动点
    
*/

using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    [SerializeField] private bool isDebugMode = true;
    [SerializeField] private int ACTIONPOINT = 5;
    private bool canClick = true;
    [SerializeField] private LayerMask unitLayerMask;
    [SerializeField] private LayerMask planeLayerMask;

    public event EventHandler<Unit> onSelectedUnitChanged;
    public event EventHandler onActionPointChanged;
    public event EventHandler<BaseAction> OnSelectedActionChanged;

    private Unit selectedUnit;
    private BaseAction selectedAction;
    private int actionPoint;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
            Debug.Log("Multiple ControllerSystem found. Destroying this one.");
        }
        Instance = this;

        selectedUnit = null;
        actionPoint = ACTIONPOINT;
    }
    void Start()
    {
        TurnSystem.Instance.onTurnChanged += TurnSystem_onTurnChanged;
        BaseAction.onActionEnd += BaseAction_onActionEnd;
    }
    void Update()
    {
        // 有动作在执行时不能单击
        if (!canClick)
        {
            return;
        }
        if (TurnSystem.Instance.IsPlayerTurn() == false)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            // 如果鼠标在UI上，则不进行任何操作
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            if (TryHandleUnitSelect())
            {
                return;
            }
            if (selectedAction == null)
            {
                return;
            }
            // 执行Action
            if (MouseManager.TryGetMouseClickObject(planeLayerMask, out RaycastHit hitInfo))
            {
                GridPosition targetGridPosition = GridManager.Instance.GetGridPosition(hitInfo.point);
                List<GridPosition> validGrid = GridManager.Instance.GetAvailableGrid(selectedAction, selectedUnit);
                if (!validGrid.Contains(targetGridPosition))
                {
                    Debug.Log("Invalid position");
                    return;
                }
                if (!TryCostPointToTakeAction(selectedAction.GetActionCost()))
                {
                    return;
                }

                selectedAction.StartAction(targetGridPosition);
                canClick = false;
                // 如果是MoveAction，显示寻路信息
                if (isDebugMode && selectedAction is Move)
                {
                    List<GridPosition> pathList = Pathfinding.Instance.FindPath(GridManager.Instance.GetUnitGridPosition(selectedUnit), targetGridPosition);
                    for(int i = 0; i < pathList.Count - 1; i++)
                    {
                        Debug.DrawLine(GridManager.Instance.GetWorldPosition(pathList[i]), GridManager.Instance.GetWorldPosition(pathList[i + 1]), Color.red, 10f);
                    }
                }
            }
        }
    }

    private bool TryHandleUnitSelect()
    {
        if (!MouseManager.TryGetMouseClickObject(unitLayerMask, out RaycastHit hitInfo))
        {
            return false;
        }

        Unit unit = hitInfo.transform.GetComponent<Unit>();
        if (selectedUnit == unit)
        {
            return true;
        }
        if (unit.IsEnemy() == true)
        {
            return false;
        }

        selectedUnit = unit;

        onSelectedUnitChanged?.Invoke(this, selectedUnit);
        selectedAction = null; // selectedUnit改变时应将selectedAction置空

        return true;
    }
    private bool TryCostPointToTakeAction(int actionCost)
    {
        if (actionPoint - actionCost >= 0)
        {
            actionPoint -= actionCost;
            onActionPointChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
        return false;
    }


    public int GetActionPoint()
    {
        return actionPoint;
    }
    public void SetSelectedAction(BaseAction action)
    {
        selectedAction = action;
        OnSelectedActionChanged?.Invoke(this, action);
    }
    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
    // Unit相关函数
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
    public void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        onSelectedUnitChanged?.Invoke(this,selectedUnit);
    }
    // 事件函数
    private void TurnSystem_onTurnChanged(object sender, EventArgs e)
    {
        actionPoint = ACTIONPOINT;
        onActionPointChanged?.Invoke(this, EventArgs.Empty);
        // 切换回合时重置选中状态
        SetSelectedAction(null);
        SetSelectedUnit(null);
    }
    private void BaseAction_onActionEnd(object sender, EventArgs e)
    {
        canClick = true;
    }

}
