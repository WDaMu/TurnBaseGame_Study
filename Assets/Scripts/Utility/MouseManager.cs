// 静态工具类
using UnityEngine;

public static class MouseManager 
{
    public static bool TryGetMouseClickObject(LayerMask layerMask, out RaycastHit hitInfo)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        return Physics.Raycast(ray, out hitInfo, float.MaxValue, layerMask);

    }
        public static bool TryGetMouseClickObject(out RaycastHit hitInfo)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        return Physics.Raycast(ray, out hitInfo, float.MaxValue);

    }
}
