using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardAnimator : MonoBehaviour
{
    [SerializeField, Label("Transform override")]
    private new Transform transform;
    [SerializeField]
    PositionTweener _tweener;
    private Vector3 scaleInit;
    private void Start()
    {
        scaleInit = transform.localScale;
    }

    public void Flip(bool upwards)
    {
        _tweener.ScaleInOut(transform, upwards ? 180f : 0f,scaleInit);       
    }



    internal void AdjustDepth(float newPosZ)
    {
        var pos = transform.position;
        pos.z = newPosZ;

        transform.position = pos;
    }

    internal void CostTooHigh(int cost)
    {
        //Debug.Log("Cost " + cost + " too high to play this card");

        // Add shake animation
        transform.DOShakePosition(1f, 0.4f, 20, 90f, false);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="siblingIndex">Don"t use if you want normal reparentin behaviour i.e last child</param>
    internal void Reparent(Transform parent, int siblingIndex = -1,int tweenIndex=0)
    {
        transform.DOSpiral(2f, new Vector3(0, 1, 1)).OnComplete(() =>
        {
            transform.parent = parent;
            if (siblingIndex >= 0 && siblingIndex < parent.childCount)
                transform.SetSiblingIndex(siblingIndex);
        });
        transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360).SetLoops(5);
        /* _tweener.PlayTween(0, transform, transform.parent.position).OnComplete(() =>
         {
             transform.parent = parent;
             if (siblingIndex >= 0 && siblingIndex < parent.childCount)
                 transform.SetSiblingIndex(siblingIndex);
         });
         _tweener.PlayTween(1, transform, transform.parent.position);*/
    }
}
