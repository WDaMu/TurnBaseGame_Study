using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grenade : BaseAction
{
    [SerializeField] private Transform grenadeTransform;
    [SerializeField] private Transform grenadeOriginTransform;
    [SerializeField] private LayerMask grenadeHitLayer;


    void Update()
    {
        if (isActive)
        {
            GenerateGrenade(OnGrenadeHit, grenadeOriginTransform.position.y);
            isActive = false; // 一次只生成一个炸弹
        }
    }


    public override void StartAction(GridPosition gridPosition)
    {
        base.StartAction(gridPosition);
        Debug.Log("Grenade Action Start");

    }

    public override void EndAction()
    {
        base.EndAction();
        Debug.Log("Grenade Action End");
    }
    private void OnGrenadeHit()
    {
        ReduceHealth();
        EndAction();
    }

    public override void ReduceHealth()
    {
        int damageRange = actionSO.actionRange;
        Collider[] colliders = Physics.OverlapSphere(targetWorldPosition, damageRange, grenadeHitLayer);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
            {
                healthSystem.TakeDamage(actionSO.actionDamage);
            }

        }
    }
    private void GenerateGrenade(Action onGrenadeHit, float height)
    {
        Transform grenade = Instantiate(grenadeTransform, grenadeOriginTransform.position, Quaternion.identity);
        grenade.GetComponent<GrenadeProjectile>().SetUp(targetWorldPosition, onGrenadeHit, height);
    }
}
