/*
    ActionButtonManager功能
    1. 显示选中单元的可用动作
    2. 初始化动作按钮时为按钮添加点击事件（设置SelectedAction）
    3. 动作执行时禁用按钮
    3. TODO:点数不足时禁用对应按钮
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ActionButtonManager : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonParent;
    private List<ActionButtonUI> actionButtonUIArray;

    void Awake()
    {
        actionButtonUIArray = new List<ActionButtonUI>();
    }

    void Start()
    {
        BaseAction.onActionStart += BaseAction_onActionStart;
        BaseAction.onActionEnd += BaseAction_onActionEnd;
        UnitActionSystem.Instance.onSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
    }
    // 显示选中单元的可用动作的按钮
    private void ShowSelectedUnitActionButton()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction[] actions = selectedUnit.GetActions();
        foreach (BaseAction action in actions)
        {
            Transform button = Instantiate(actionButtonPrefab, actionButtonParent);
            button.GetComponent<ActionButtonUI>().SetAction(action);
            actionButtonUIArray.Add(button.GetComponent<ActionButtonUI>());
            string actionName = action.GetActionName();
            button.GetComponentInChildren<TextMeshProUGUI>().text = actionName;
            button.GetComponentInChildren<Button>().onClick.AddListener(() => UnitActionSystem.Instance.SetSelectedAction(action));
        }
    }

    private void DeleteAllActionButton()
    {
        foreach (Transform child in actionButtonParent)
        {
            Destroy(child.gameObject);
        }
        actionButtonUIArray.Clear(); // 删除按钮时清空数组
    }

    // 有动作在执行时禁止选择所有按钮
    private void DisableAllActionButton()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIArray)
        {
            actionButtonUI.ButtonUnavailable();
        }
    }
    private void EnableAllActionButton()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIArray)
        {
            actionButtonUI.ButtonAvailable();
        }
    }
    // 事件函数
    private void BaseAction_onActionStart(object sender, BaseAction executedAction)
    {
        DisableAllActionButton();
    }
    private void BaseAction_onActionEnd(object sender, EventArgs e)
    {
        EnableAllActionButton();
    }
    private void UnitActionSystem_OnSelectedUnitChanged(object sender, Unit unit)
    {
        if (unit == null)
        {
            DeleteAllActionButton();
        }
        else
        {
            DeleteAllActionButton();
            ShowSelectedUnitActionButton();
        }
    }

}
