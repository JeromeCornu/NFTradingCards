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
    TweenHelper _tweener;
    private Vector3 scaleInit;
    private void Start()
    {
        scaleInit = transform.localScale;
    }

    public void Flip(bool upwards)
    {
        _tweener.ScaleInFlipScaleOut(transform, GetRotation(upwards), scaleInit);
    }

    private static float GetRotation(bool upwards)
    {
        return upwards ? 180f : 0f;
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
    internal void Reparent(Layout parent, int targetSiblingIndex, int tweenIndex = 0) => Reparent(new ParentingOption(parent, targetSiblingIndex), tweenIndex);
    internal void Reparent(ParentingOption option, int tweenIndex = 0)
    {
        TweenCallback OnComplete = () =>
        {
            transform.parent = option;
            if (option.indexInParent >= 0 && option.indexInParent < option.transform.childCount)
                transform.SetSiblingIndex(option.indexInParent);
        };
        Tween mainTween;
        if (tweenIndex == 0)
        {
            mainTween = _tweener.PlayTween<Vector3>(TweenPreset.MoveTo, transform, option);
        }
        else
        {
            float duration = 2f;
            //transform.DOSpiral(duration, new Vector3(0, 1, 1),SpiralMode.Expand);
            int nbOfLoops = 1;
            var rotateSequence = DOTween.Sequence();
            rotateSequence.Append(_tweener.RotateOnY(transform, GetRotation(true)));
            //We store the tween but do not trigger it (we pause it and not auto kill)
            mainTween = transform.DOMove(option, duration).SetEase(Ease.InOutSine).SetAutoKill(false).Pause();
            //When the rotate is finished we start the position and spinning twin in parralel
            rotateSequence.AppendCallback(() =>
            {
                mainTween.Play();
                var initRot = transform.eulerAngles;
                //We want total tween to take place in half the duration of the main tween
                transform.DORotate(initRot + new Vector3(0, 360, 0), (duration/2f) / nbOfLoops, RotateMode.FastBeyond360)
                    //We snap back to modulused initiarotation before looping again so we can get exact same behaviour
                    .OnComplete(() => transform.eulerAngles = initRot)
                    .SetEase(Ease.Linear)
                    .SetLoops(nbOfLoops);
            });
        }
        mainTween.OnComplete(OnComplete);
        /* _tweener.PlayTween(0, transform, transform.parent.position).OnComplete(() =>
         {
             transform.parent = parent;
             if (siblingIndex >= 0 && siblingIndex < parent.childCount)
                 transform.SetSiblingIndex(siblingIndex);
         });
         _tweener.PlayTween(1, transform, transform.parent.position);*/
    }
}
