using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

public class Wait : Action
{
    [SerializeField] private float timeDelay;
    private float delay = 0;
    protected override Status OnUpdate()
    {
        delay += Time.deltaTime;

        if (delay > timeDelay) { 
            delay = 0;
            return Status.Success;
        }

        return Status.Running;


        
    }
}
