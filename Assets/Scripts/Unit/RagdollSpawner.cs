using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform unitRagdollPrefab;
    [SerializeField] private Transform originRoot;
    private Transform ragdoll;
    private Transform ragdollRoot;
    private Rigidbody ragdollRigidbody;
    //private float explosionForce = 300f;
    private Transform explosionCenter;

    public void GenerateRagdoll()
    {
        ragdoll = Instantiate(unitRagdollPrefab, transform.position, Quaternion.identity);
        ragdollRoot = ragdoll.Find("Character_Dummy_Male_01/Root");
        ragdollRigidbody = ragdoll.GetComponent<Rigidbody>();
        explosionCenter = ragdoll.Find("ExplosionCenter");
        // Debug.Log("ragdollRoot: " + ragdollRoot);
        MatchAllChildren(originRoot, ragdollRoot);
        //AddExplosionForce();
    }

    // 递归修改Ragdoll的变换，使与原Unit姿态保持一致
    private void MatchAllChildren(Transform originRoot, Transform ragdollRoot)
    {
        foreach (Transform child in originRoot)
        {
            Transform toMatchTransform = ragdollRoot.Find(child.name);// Find函数只会查找直接子对象
            if (toMatchTransform != null)
            {
                // Debug.Log("matching "+ child.name);
                toMatchTransform.position = child.position;
                toMatchTransform.rotation = child.rotation;
                MatchAllChildren(child, toMatchTransform);
            }
        }
    }

    // private void AddExplosionForce()
    // {
    //     // // 获取受力方向
    //     // Unit damageFrom = ActionSystem.Instance.GetSelectedUnit();
    //     // Vector3 explosionDir = (damageFrom.transform.position - explosionCenter.position).normalized;
    //     // explosionDir.y = explosionCenter.transform.position.y;
    //     // explosionCenter.position += explosionDir;
    //     //foreach()
    //     ragdollRigidbody.AddExplosionForce(explosionForce, transform.position, 10f);
    //     Debug.Log("Add explosion force to ragdoll");
    // }
}
