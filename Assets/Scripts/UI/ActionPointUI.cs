using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ActionPointUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointText;

    void Start()
    {
        UnitActionSystem.Instance.onActionPointChanged += ActionSystem_onActionPointChanged;
        UpdateActionPoint();
    }

    private void UpdateActionPoint()
    {
        actionPointText.text = "Action Point: " + UnitActionSystem.Instance.GetActionPoint().ToString();
    }

    private void ActionSystem_onActionPointChanged(object sender, EventArgs e)
    {
        UpdateActionPoint();
    }
}

