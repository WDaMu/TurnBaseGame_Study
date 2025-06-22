/*
    Unit.cs
    Function:
        1. 定义Unit类，用于管理Unit的动作和状态
        2. 初始化时将自己添加到所在网格的GridObject中
*/

using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]private bool isEnemy;
    private Move moveAction;
    private BaseAction[] actions;
    private GridPosition gridPosition; // GridPosition改变时，更新网格显示


    void Awake()
    {
        gridPosition = GridManager.Instance.GetGridPosition(transform.position);
        GridManager.Instance.SetUnitAtGridPosition(gridPosition, this);


        // 初始化所有可用的动作
        moveAction = GetComponent<Move>();
        actions = GetComponents<BaseAction>();
    }
    void Start()
    {
    }

    void Update()
    {
        // Unit移动时更新GridObject
        GridPosition newGridPosition = GridManager.Instance.GetGridPosition(transform.position);
        if (gridPosition != newGridPosition)
        {
            GridManager.Instance.MoveUnitToGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }

    }
    public BaseAction[] GetActions()
    {
        return actions;
    }
    public Move GetMoveAction()
    {
        return moveAction;
    }
    public bool IsEnemy()
    {
        return isEnemy;
    }
}
