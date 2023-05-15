using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static SelectableCard;

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
        _tweener.ScaleInOut(transform, upwards ? 180f : 0f, scaleInit);
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
    //Generated wrapper for origin
    public struct ParentingOption
    {
        public Layout parent;
        public int indexInParent;
        public Vector3 WorldFinalPosition => parent.transform.TransformPoint(LocalFinalPosition);
        public Vector3 LocalFinalPosition => parent.PredictPos(indexInParent);

        public Transform transform => parent.transform;

        public ParentingOption(Transform transform) : this(transform.GetComponentInParent<Layout>(), transform.GetSiblingIndex()) { }
        public ParentingOption(Layout parent, int item2)
        {
            this.parent = parent;
            indexInParent = item2;
        }

        public override bool Equals(object obj)
        {
            return obj is ParentingOption other &&
                   EqualityComparer<Layout>.Default.Equals(parent, other.parent) &&
                   indexInParent == other.indexInParent;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(parent, indexInParent);
        }

        public void Deconstruct(out Layout item1, out int item2)
        {
            item1 = parent;
            item2 = indexInParent;
        }

        public static implicit operator (Transform, int)(ParentingOption value)
        {
            return (value.parent.transform, value.indexInParent);
        }

        public static implicit operator ParentingOption((Layout, int) value)
        {
            return new ParentingOption(value.Item1, value.Item2);
        }
        public static implicit operator Transform(ParentingOption value)
        {
            return value.transform;
        }
        public static implicit operator Vector3(ParentingOption value)
        {
            return value.WorldFinalPosition;
        }
    }
    internal void Reparent(Layout parent,int targetSiblingIndex,int tweenIndex=0)=> Reparent(new ParentingOption(parent,targetSiblingIndex),tweenIndex);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="siblingIndex">Don"t use if you want normal reparentin behaviour i.e last child</param>
    internal void Reparent(ParentingOption option, int tweenIndex = 0)
    {
        TweenCallback OnComplete = () =>
        {
            transform.parent = option;
            if (option.indexInParent >= 0 && option.indexInParent < option.transform.childCount)
                transform.SetSiblingIndex(option.indexInParent);
        };
        if (tweenIndex == 0)
        {
            transform.DOMove(option, 1f).SetEase(Ease.InOutSine).OnComplete(OnComplete);
        }
        else
        {
            transform.DOSpiral(2f, new Vector3(0, 1, 1)).OnComplete(OnComplete);
            transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360).SetLoops(5);
        }

        /* _tweener.PlayTween(0, transform, transform.parent.position).OnComplete(() =>
         {
             transform.parent = parent;
             if (siblingIndex >= 0 && siblingIndex < parent.childCount)
                 transform.SetSiblingIndex(siblingIndex);
         });
         _tweener.PlayTween(1, transform, transform.parent.position);*/
    }
}
