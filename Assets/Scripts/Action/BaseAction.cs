using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    [SerializeField] protected ActionScriptableObject actionSO; 
    public static event EventHandler<BaseAction> onActionStart;
    public static event EventHandler onActionEnd;
    protected bool isActive;

    protected GridPosition targetGridPosition; // action的目标GridPosition
    protected Vector3 targetWorldPosition; // action的目标WorldPosition

    public virtual void StartAction(GridPosition targetGridPosition)
    {
        onActionStart?.Invoke(this, this);
        isActive = true;
        this.targetGridPosition = targetGridPosition;
        this.targetWorldPosition = GridManager.Instance.GetWorldPosition(targetGridPosition);
    }
    public virtual void EndAction()
    {
        onActionEnd?.Invoke(this, EventArgs.Empty);
        isActive = false;

    }
    public int GetActionCost()
    {
        return actionSO.actionCost;
    }
    public string GetActionName()
    {
        return actionSO.actionName;
    }
    public int GetActionRange()
    {
        return actionSO.actionRange;
    }
    public Color GetActionColor()
    {
        return actionSO.actionColor;
    }
    public GridPosition GetTargetGridPosition()
    {
        return targetGridPosition;
    }

    public virtual void ReduceHealth()
    {   
        Unit unit = GridManager.Instance.GetUnitAtGridPosition(targetGridPosition);
        unit.GetComponent<HealthSystem>().TakeDamage(actionSO.actionDamage);
    }

    public override string ToString()
    {
        return actionSO.actionName;
    }

}
