using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : DebugObjectBase
{
    [SerializeField] private TextMeshPro gridDebugText;

    private object gridObject;

    private void Update()
    {
        gridDebugText.text = gridObject.ToString();
    }

    public override void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }

    public override void SetText(string text)
    {
        gridDebugText.text = text;
    }

}
