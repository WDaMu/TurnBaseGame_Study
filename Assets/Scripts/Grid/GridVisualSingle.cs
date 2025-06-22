using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    public void Show(Color color)
    {
        meshRenderer.enabled = true;
        meshRenderer.material.color = color;
    }
    public void Hide()
    {
        meshRenderer.enabled = false;
    }

}
