using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PathfindingGridDebugObject : DebugObjectBase
{
    [SerializeField] private TextMeshPro gCostText;
    [SerializeField] private TextMeshPro hCostText;
    [SerializeField] private TextMeshPro fCostText;
    private PathNode pathNode;
    
    private void Update()
    {
        gCostText.text = pathNode.GetGCost().ToString();
        hCostText.text = pathNode.GetHCost().ToString();
        fCostText.text = pathNode.GetFCost().ToString();
    }


    public override void SetGridObject(object gridObject)
    {
        pathNode = (PathNode)gridObject;
    }

}
