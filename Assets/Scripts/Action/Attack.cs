using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : BaseAction
{
    float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            // TEMPORARY CODE
            timer += Time.deltaTime;
            if (timer > 2f)
            {
                timer = 0f;
                EndAction();
            }
        }
    }

    public override void StartAction(GridPosition gridPosition)
    {
        base.StartAction(gridPosition);
        Debug.Log("Attack Action Start");
    }

    public override void EndAction()
    {
        base.EndAction();
        Debug.Log("Attack Action End");
    }
}
