using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }
    [SerializeField] private Transform pfGridDebugObject;
    [SerializeField] private bool isDebugMode = true;
    [SerializeField] private LayerMask obstacleLayerMask;
    private int MOVE_STRAIGHT_COST = 10;
    private int MOVE_DIAGONAL_COST = 14;
    private int width;
    private int height;
    private int gridSize;

    private GridSystem<PathNode> gridSystem;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void SetUp(int width, int height, int gridSize, Vector3 origin)
    {
        this.width = width;
        this.height = height;
        this.gridSize = gridSize;

        gridSystem = new GridSystem<PathNode>(width, height, gridSize, origin,
            (GridSystem<PathNode> gridSystem, GridPosition gridPosition) => new PathNode(gridPosition));
        if (isDebugMode)
        {
            gridSystem.CreateGridDebugObject(pfGridDebugObject);
        }
        // 设置障碍物
        int detectOffset = 5;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 position = gridSystem.GetWorldPosition(new GridPosition(x, z));
                if (Physics.Raycast(position + Vector3.down * detectOffset, Vector3.up, 2 * detectOffset, obstacleLayerMask))
                {
                    GetPathNode(new GridPosition(x, z)).SetIsWalkable(false);
                }
            }
        }
    }
    // 寻路算法
    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        // 初始化
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);

        // 重置所有PathNode
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();

            }
        }

        // 开始寻路
        // 初始化起点
        startNode.SetGCost(0);
        startNode.SetHCost(GetDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        // 开始遍历进行搜索
        while (openList.Count > 0)
        {
            // 从F值最小的Node开始搜索(从距离起点最近的Node开始搜索)
            PathNode currentNode = GetLowestFCostNode(openList);
            // 验证是否是终点
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // 搜索相邻节点
            foreach (PathNode neighbourNode in GetNeighbours(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }
                if (neighbourNode.IsWalkable() == false)
                {
                    closedList.Add(neighbourNode); // 遇到不可行走的节点，同样需要加入closedList
                    continue;
                }

                int tentativeGCost = currentNode.GetGCost() + GetDistance(neighbourNode.GetGridPosition(), currentNode.GetGridPosition());
                //  从CurrentNode到NeighbourNode的路径更短
                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(GetDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();
                }

                if (!openList.Contains(neighbourNode))
                {
                    openList.Add(neighbourNode);
                }
            }
        }

        // 没有找到路径
        return null;

    }
    public List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);

        PathNode currentNode = endNode;
        while (currentNode.GetCameFromNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromNode());
            currentNode = currentNode.GetCameFromNode();
        }
        
        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    }

    // 计算H值，相邻节点可以视为G值
    public int GetDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition GridPositionDistance = gridPositionA - gridPositionB;
        int xDistance = Math.Abs(GridPositionDistance.x);
        int zDistance = Math.Abs(GridPositionDistance.z);
        int diagonalDistance = Math.Min(xDistance, zDistance) * MOVE_DIAGONAL_COST;

        int totalDistance = Math.Abs(xDistance - zDistance) * MOVE_STRAIGHT_COST + diagonalDistance;

        return totalDistance;
    }
    // 获取F值最小的Node
    public PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostNode.GetFCost())
            {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }
    public List<PathNode> GetNeighbours(PathNode pathNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition gridPosition = pathNode.GetGridPosition();
        for (int x = gridPosition.x - 1; x <= gridPosition.x + 1; x++)
        {
            for (int z = gridPosition.z - 1; z <= gridPosition.z + 1; z++)
            {
                if (gridSystem.IsValidGridPosition(new GridPosition(x, z)))
                {
                    PathNode neighbourNode = gridSystem.GetGridObject(new GridPosition(x, z));
                    neighbourList.Add(neighbourNode);
                }
            }
        }

        return neighbourList;
    }

    public PathNode GetPathNode(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition);
    }
}
