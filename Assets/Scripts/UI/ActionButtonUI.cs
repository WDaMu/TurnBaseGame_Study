using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    
    [SerializeField] private Image selectedVisual;
    [SerializeField] private GameObject buttonBusyVisual;
    [SerializeField] private Button button;
    private BaseAction action; // 按钮对应的Action

    void Start()
    {
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
    }
    void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedActionChanged -= UnitActionSystem_OnSelectedActionChanged;
    }
    void Awake()
    {
        HideSelectedVisual();
        ButtonAvailable();
    }
    // 选中显示
    public void ShowSelectedVisual()
    {
        selectedVisual.enabled = true;
    }

    public void HideSelectedVisual()
    {
        selectedVisual.enabled = false;
    }
    // 有动作在执行时，显示不可用并禁用按钮组件
    public void ButtonAvailable()
    {
        buttonBusyVisual.SetActive(false);
        button.enabled = true;
    }
    public void ButtonUnavailable()
    {
        buttonBusyVisual.SetActive(true);
        button.enabled = false;
    }
    
    public void SetAction(BaseAction action)
    {
        this.action = action;
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, BaseAction action)
    {
        if (action == null)
        {
            return;
        }
        if (this.action.GetActionName() == action.GetActionName())
        {
            ShowSelectedVisual();
        } else
        {
            HideSelectedVisual();
        }
    }
}
