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
    private Vector3 transformInit;

    private void Start()
    {
        transformInit = transform.localScale;
    }

    public void Flip(bool upwards)
    {
        float dir = upwards ? 180f : 0f;
        transform.DOScale(.5f * Vector3.one, 0.2f)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                transform.DORotate(new Vector3(0, dir, 0), 0.5f)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() =>
                    {
                        transform.DOScale(Vector3.one * 1.2f, 0.2f)
                            .SetEase(Ease.InQuad)
                            .OnComplete(() =>
                            {
                                transform.DOScale(transformInit, 0.2f)
                                    .SetEase(Ease.OutBounce);
                            });
                    });
            });

    }



    internal void AdjustDepth(float newPosZ)
    {
        var pos = transform.position;
        pos.z = newPosZ;

        transform.position = pos;
    }

    internal void CostTooHigh(int cost)
    {
        Debug.Log("Cost " + cost + " too high to play this card");

        // Add shake animation
        transform.DOShakePosition(1f, 0.4f, 20, 90f, false);
    }

    internal void Reparent(Transform parent, int siblingIndex = -1)
    {
        transform.parent = parent;
        if (siblingIndex >= 0 && siblingIndex < parent.childCount)
            transform.SetSiblingIndex(siblingIndex);
    }
}
