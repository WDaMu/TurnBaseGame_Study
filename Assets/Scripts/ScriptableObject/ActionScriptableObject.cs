using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionSO", menuName = "ActionScriptableObject/ActionSO")]
public class ActionScriptableObject : ScriptableObject
{
    public string actionName;
    public int actionRange;
    public int actionCost;
    public int actionDamage = 20;
    public int damageRange = 1; // 伤害范围
    public Color actionColor; // 显示动作可行范围的颜色
}
