using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrenadeProjectile : MonoBehaviour
{
    [SerializeField] private Transform hitEffect;
    // 使用AnimationCurve控制炸弹的轨迹，在y轴上使用动画曲线控制
    [SerializeField] private AnimationCurve curve;
    private float bulletSpeed = 10f;
    private Vector3 targetPosition;
    private float stopDistance = 0.1f;
    private Action onBulletHit;
    private float totalXZDistance;
    private Vector3 positionXZ;
    private float heightOffset;


    void Update()
    {
        if ((targetPosition - transform.position).magnitude > stopDistance)
        {
            Vector3 temp = Vector3.Lerp(transform.position, targetPosition, bulletSpeed * Time.deltaTime);
            positionXZ = temp;
            positionXZ.y = 0;
            float distance = Vector3.Distance(positionXZ, targetPosition);
            float normalizedDistance = 1- distance / totalXZDistance;
            temp.y = curve.Evaluate(normalizedDistance) * heightOffset;
            transform.position = temp;
        }
        else
        {
            Instantiate(hitEffect, targetPosition, Quaternion.identity);
            onBulletHit();
            Destroy(this.gameObject);
        }
    } 
    public void SetUp(Vector3 targetPosition, Action onBulletHit, float height)
    {
        this.targetPosition = targetPosition;
        this.onBulletHit = onBulletHit;

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalXZDistance = Vector3.Distance(positionXZ, targetPosition); // targetPosition的z为0(在地面上)

        this.heightOffset = height;
    }
}
