using UnityEngine;
using System;
public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Unit unit;

    void Start()
    {
        UnitActionSystem.Instance.onSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
    }
    void Awake()
    {
        Hide();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, Unit selectedUnit)
    {
        if (selectedUnit == unit)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        spriteRenderer.enabled = true;
    }

    public void Hide()
    {
        spriteRenderer.enabled = false;
    }
}
