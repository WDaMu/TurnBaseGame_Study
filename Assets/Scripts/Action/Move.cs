using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : BaseAction
{
    private Vector3 targetPosition;
    private List<GridPosition> path;
    private int currentIndex = 0;
    
    void Update()
    {
        if (isActive)
        {
            TakeAction();
        }
    }
    private void TakeAction()
    {
        float moveSpeed = 5f;
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        // 调整Unit朝向移动方向
        if (transform.forward != moveDir)
        {
            float rotateSpeed = 20f;
            transform.forward = Vector3.Lerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
        }

        Vector3 currentTargetPosition = GridManager.Instance.GetWorldPosition(path[currentIndex]);

        transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, moveSpeed * Time.deltaTime);
        if (transform.position == currentTargetPosition)
        {
            currentIndex++;
            if (currentIndex >= path.Count)
            {
                EndAction();
                currentIndex = 0;
            }
        }

    }

    public override void StartAction(GridPosition gridPosition)
    {
        base.StartAction(gridPosition);

        targetPosition = GridManager.Instance.GetWorldPosition(targetGridPosition);

        //计算Path
        path = Pathfinding.Instance.FindPath(GridManager.Instance.GetGridPosition(transform.position), targetGridPosition) ;
    }
    public override void EndAction()
    {
        base.EndAction();
    }
}
