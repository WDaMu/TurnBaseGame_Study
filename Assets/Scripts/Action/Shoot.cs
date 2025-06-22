using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shoot : BaseAction
{
    public event EventHandler OnShootAnimationStart;
    [SerializeField] private Transform bulletTransform;
    [SerializeField] private Transform bulletOriginTransform;
    float timer = 0f;
    private Vector3 rotateDir;
    public enum ShootState
    {
        aim,
        shoot,
        reloading
    }
    private ShootState shootState;
    void Awake()
    {
        shootState = ShootState.aim;
    }

    void Update()
    {
        if (isActive)
        {
            // 调整Unit朝向
            if (rotateDir != Vector3.zero && transform.forward != rotateDir)
            {
                float rotateSpeed = 3f;
                Quaternion targetRotation = Quaternion.LookRotation(rotateDir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed);
            }
            timer += Time.deltaTime;
            switch (shootState)
            {
                case ShootState.aim:
                    if (timer > 1f)
                    {
                        shootState = ShootState.shoot;
                        timer = 0f;
                        OnShootAnimationStart?.Invoke(this, EventArgs.Empty);
                        GenerateBullet(OnBulletHit);
                    }
                    break;
                case ShootState.shoot:
                    if (timer > 1f)
                    {
                        shootState = ShootState.reloading;
                        timer = 0f;
                    }
                    break;
                case ShootState.reloading:
                    if (timer > 0.5f)
                    {
                        shootState = ShootState.aim;
                        timer = 0f;
                        EndAction();
                    }
                    break;
            }
        }
    }

    public ShootState GetShootState()
    {
        return shootState;
    }
    public override void StartAction(GridPosition gridPosition)
    {
        base.StartAction(gridPosition);
        rotateDir = (GridManager.Instance.GetWorldPosition(targetGridPosition) - transform.position).normalized;
        rotateDir.y = 0; // 确保只在水平方向旋转
        Debug.Log("Shoot Action Start");

    }

    public override void EndAction()
    {
        base.EndAction();
        Debug.Log("Shoot Action End");
    }
    private void GenerateBullet(Action onBulletHit)
    {
        Transform bullet = Instantiate(bulletTransform, bulletOriginTransform.position, Quaternion.identity);
        GridPosition targetGridPosition = GetTargetGridPosition();
        Vector3 targetPosition = GridManager.Instance.GetWorldPosition(targetGridPosition);
        targetPosition.y = bullet.position.y;
        bullet.GetComponent<BulletProjectile>().SetTargetPosition(targetPosition, onBulletHit);
    }
    private void OnBulletHit()
    {
        ReduceHealth();
    }
}
