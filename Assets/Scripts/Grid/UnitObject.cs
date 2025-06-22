using System;
using UnityEngine;

public class UnitObject
{
    public GridSystem<UnitObject> gridSystem;
    public GridPosition gridPosition;
    public Unit unit;

    public UnitObject(GridSystem<UnitObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
    }

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
    }
    public Unit GetUnit()
    {
        return unit;
    }

    public override string ToString()
    {
        return gridPosition.ToString() + "\n" +unit;
    }


}
