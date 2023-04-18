using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Implement feedback and animation (eitheir with animator or tweening engine) instead of the draft implementation
/// </summary>
public class CardAnimator : MonoBehaviour
{
    [SerializeField, Label("Transform override")]
    private new Transform transform;
    public void Flip(bool upwards)
    {
        float dir = upwards ? 180f : 0f;
        transform.eulerAngles = new Vector3(0, dir, 0);
    }

    internal void AdjustDepth(float newPosZ)
    {
        var pos = transform.position;
        pos.z = newPosZ;
        //Debug.Log(pos.z);
        transform.position = pos;
    }

    internal void CostTooHigh(int cost)
    {
        Debug.Log("Cost " + cost + " too high to play this card");
    }

    internal void Reparent(Transform parent)
    {
        transform.parent = parent;
    }
}
