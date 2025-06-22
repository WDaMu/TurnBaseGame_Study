using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        Vector3 lookDirection = (mainCamera.position - transform.position).normalized;
        transform.LookAt(transform.position + lookDirection * -1);   
    }
}
