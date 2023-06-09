using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;
using Random = UnityEngine.Random;

public class Intelect : Action
{
    private float intelectThreshold;

    protected override Status OnUpdate()
    {
        intelectThreshold = BT_Blackboard.Floats[BT_Blackboard.DifficultyKey];
        return Random.Range(0f, 1f) > intelectThreshold ? Status.Success : Status.Failure;
    }
}