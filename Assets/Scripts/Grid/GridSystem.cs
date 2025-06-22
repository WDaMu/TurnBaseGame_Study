using UnityEngine;
using System;
using TMPro;

/*
1. 管理网格上的所有对象
2. 网格的大小、数量、位置等信息
*/
public class GridSystem<TGridObject>
{
    private int width;
    private int height;
    private int gridSize;
    private Vector3 origin;
    private TGridObject[,] gridObjectArray; // 网格对象数组
    private DebugObjectBase[,] gridDebugObjectArray; // 网格调试信息预制体数组

    public GridSystem(int width, int height, int gridSize, Vector3 origin, Func<GridSystem<TGridObject>, GridPosition, TGridObject> InitalTGridObject)
    {
        this.width = width;
        this.height = height;
        this.gridSize = gridSize;
        this.origin = origin;


        // 初始化TGridObject
        gridObjectArray = new TGridObject[width, height];
        // 初始化网格调试信息预制体
        gridDebugObjectArray = new DebugObjectBase[width, height];


        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                gridObjectArray[x, z] = InitalTGridObject(this, new GridPosition(x, z));
            }
        }
    }

    public void CreateGridDebugObject(Transform gridDebugObjectPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // 创建调试信息
                Transform gridDebugObjectTransform = GameObject.Instantiate(gridDebugObjectPrefab, new Vector3(x, 0, z) * gridSize + origin, Quaternion.identity);
                DebugObjectBase gridDebugObject = gridDebugObjectTransform.GetComponent<DebugObjectBase>();
                gridDebugObject.SetGridObject(gridObjectArray[x, z]);
                gridDebugObjectArray[x, z] = gridDebugObject;
            }
        }
    }
    public void UpdateDebugInfo()
    {
        // 更新调试信息
        GridManager.Instance.GetGridSystemInfo(out int width, out int height, out int gridSize, out Vector3 origin);
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // 更新调试信息
                gridDebugObjectArray[x, z].SetText(GridManager.Instance.GetGridObject(new GridPosition(x, z)).ToString());
            }
        }
    }

    // 是否在GridSystem范围内
    public bool IsValidGridPosition(GridPosition position)
    {
        if (position.x < 0 || position.x >= width || position.z < 0 || position.z >= height)
        {
            return false;
        }
        else return true;
    }


    public GridPosition GetGridPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x - origin.x) / gridSize;
        int z = Mathf.RoundToInt(position.z - origin.z) / gridSize;
        return new GridPosition(x, z);
    }
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * gridSize + origin;
    }

    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public void GetGridSystemInfo(out int width, out int height, out int gridSize, out Vector3 origin)
    {
        width = this.width;
        height = this.height;
        gridSize = this.gridSize;
        origin = this.origin;
    }
    public int GetGridSystemWidth()
    {
        return width;
    }
    public int GetGridSystemHeight()
    {
        return height;
    }
    public int GetGridSystemGridSize()
    {
        return gridSize;
    }
    public Vector3 GetGridSystemOrigin()
    {
        return origin;
    }
}
