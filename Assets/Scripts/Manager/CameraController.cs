using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float MAX_FOLLOWOFFSET = 30f;
    private float MIN_FOLLOWOFFSET = 5f;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private float moveSpeed = 10f;
    Vector3 followOffset;
    Vector3 targetOffset;
    void Awake()
    {
        targetOffset = virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        RotateCamera();
        ZoomCamera();
    }

    private void MoveCamera()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
        }
    }
    private void RotateCamera()
    {
        float rotateSpeed = 60f;
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, -rotateSpeed * Time.deltaTime, 0);
        }
    }
    // 参考CodeMonkey教程
    private void ZoomCamera()
    {
        Vector3 zoomDir = virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.normalized;
        followOffset = virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        float zoomAmount = 5f;
        if (Input.mouseScrollDelta.y < 0)
        {
            targetOffset = followOffset + zoomDir  * zoomAmount;
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            targetOffset = followOffset - zoomDir * zoomAmount;
        }
        if (targetOffset.magnitude > MAX_FOLLOWOFFSET)
        {
            targetOffset = zoomDir * MAX_FOLLOWOFFSET;
        }
        if (targetOffset.magnitude < MIN_FOLLOWOFFSET)
        {
            targetOffset = zoomDir * MIN_FOLLOWOFFSET;
        }
        float zoomSpeed = 5f;
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(followOffset, targetOffset, zoomSpeed * Time.deltaTime);

    }
}
