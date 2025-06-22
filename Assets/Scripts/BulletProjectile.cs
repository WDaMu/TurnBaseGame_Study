using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private Transform hitEffect;
    private float bulletSpeed = 50f;
    private Vector3 targetPosition;
    private float stopDistance = 0.1f;
    private Action onBulletHit;
    void Update()
    {
        if ((targetPosition - transform.position).magnitude > stopDistance)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, bulletSpeed * Time.deltaTime);
        }
        else
        {
            Instantiate(hitEffect, targetPosition, Quaternion.identity);
            onBulletHit();
            Destroy(this.gameObject);
        }
    } 
    public void SetTargetPosition(Vector3 targetPosition, Action onBulletHit)
    {
        this.targetPosition = targetPosition;
        this.onBulletHit = onBulletHit;
    }
}
