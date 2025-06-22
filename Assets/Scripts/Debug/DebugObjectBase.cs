using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugObjectBase : MonoBehaviour
{
    public virtual void SetText(string text)
    {
        throw new System.Exception("Base class method called.");
    }
    public virtual void SetGridObject(object obj)
    {
        throw new System.Exception("Base class method called.");
    }
}
