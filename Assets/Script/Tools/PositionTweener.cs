using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PositionTweener : MonoBehaviour
{
    public TweenParameter[] _availablePredefinedTweens;
    public List<Tweener> _tweens = new List<Tweener>();
    //In case we don't override the duration when trying to call a tween
    private const float DEFAULTTWEENDURATION=1f;
    private void Start()
    {
        RegisterReplayableTween(DOTweenProShortcuts.DOSpiral((Transform)null, DEFAULTTWEENDURATION, new Vector3(0, 1, 1)));
        RegisterReplayableTween(ShortcutExtensions.DORotate(transform, new Vector3(0, 360, 0), DEFAULTTWEENDURATION,RotateMode.FastBeyond360));
    }
    private void RegisterReplayableTween(Tweener tween)
    {
        tween.Pause();
        tween.target = null;
        _tweens.Add(tween);
    }
    public Tween PlayTween(int registeredTween,Transform target, Vector3 worldDest, TweenCallback onComplete = null)
    {
        return StartTween(_tweens[registeredTween], target, worldDest, _availablePredefinedTweens[0], onComplete);
    }
    public Tween StartTween(Tweener tween, Transform target, Vector3 worldDest, TweenParameter parameter, TweenCallback onComplete = null)
    {
        tween.target = target;
        tween.ChangeEndValue(worldDest, parameter._duration);
        tween.SetEase(parameter._ease);
        if (onComplete != null)
            tween.OnComplete(onComplete);
        tween.OnComplete(() => tween.Rewind());
        tween.Restart();
        return tween;
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
