/*
    1. 管理Unit的动画
    2. 包括移动，射击，攻击等动画
*/

using System;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    private BaseAction executingAction;
    void Start()
    {
        BaseAction.onActionStart += BaseAction_OnActionStart;
        BaseAction.onActionEnd += BaseAction_OnActionEnd;
    }
    [SerializeField] private Animator UnitAnimator;
    [SerializeField] private Unit unit;

    private void AnimationStart()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() != unit)
        {
            return;
        }
        switch (executingAction)
        {
            case Move:
                UnitAnimator.SetBool("isRun", true);
                break;
            case Attack:
                break;
            case Shoot:
                UnitAnimator.SetTrigger("shoot");
                break;
        }
    }
    private void AnimationEnd()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() != unit)
        {
            return;
        }
        switch (executingAction)
        {
            case Move:
                UnitAnimator.SetBool("isRun", false);
                break;
            case Attack:
                break;
            case Shoot:
                break;
        }
    }

    private void BaseAction_OnActionStart(object sender, BaseAction selectedAction)
    {
        executingAction = selectedAction;
        AnimationStart();

    }
    private void BaseAction_OnActionEnd(object sender, EventArgs e)
    {
        AnimationEnd();
    }

}
