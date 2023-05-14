using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PositionTweener : MonoBehaviour
{
    public TweenParameter[] _availablePredefinedTweens;
    private void Start()
    {
        /*var t = DOTween.Sequence().Ap;
        DOTween.DoSp*/
    }

    public Tween StartTween(Transform target, Vector3 worldDest, TweenParameter parameter)
    {
        return StartTween(target, worldDest, parameter, () => { return; });
    }
    public Tween StartTween(Transform target, Vector3 worldDest, TweenParameter parameter, TweenCallback onComplete)
    {
        return target.DOMove(worldDest, parameter._duration).SetEase(parameter._ease).OnComplete(onComplete);
        //target.DoSpiral()
    }
}
[System.Serializable]
public struct TweenParameter
{
    [SerializeField]
    public readonly float _duration;
    [SerializeField]
    public readonly Ease _ease;

}
