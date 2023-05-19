using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
public enum TweenPreset
{
    MoveTo = 0, Spiral = 1, Flip = 2, RotateNTimes = 3
    , CustomTween1 = 101, CustomTween2 = 102, CustomTween3 = 103, CustomTween4 = 104, CustomTween5 = 105
}
[System.Serializable]
public struct TweenParameter
{
    [SerializeField]
    public float _duration;
    [SerializeField]
    public Ease _ease;
    public static TweenParameter Default => new TweenParameter() { _duration = DEFAULTTWEENDURATION, _ease = Ease.Linear };
    public const float DEFAULTTWEENDURATION = 1f;
}
public class PositionTweener : MonoBehaviour
{
    [SerializeField]
    private TweenParameter[] _availablePredefinedTweens;
    public List<Tweener> _tweens = new List<Tweener>();
    //In case we don't override the duration when trying to call a tween
    private const float DEFAULTTWEENDURATION = 1f;
    private void Start()
    {
        RegisterReplayableTween(DOTweenProShortcuts.DOSpiral((Transform)null, DEFAULTTWEENDURATION, new Vector3(0, 1, 1)));
        RegisterReplayableTween(ShortcutExtensions.DORotate(transform, new Vector3(0, 360, 0), DEFAULTTWEENDURATION, RotateMode.FastBeyond360));
    }
    private void RegisterReplayableTween(Tweener tween)
    {
        tween.SetAutoKill(false);
        tween.Pause();
        tween.target = null;
        _tweens.Add(tween);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Mostly Vector3 ,but might be anything you know a tweens uses as end value</typeparam>
    /// <param name="registeredTween"></param>
    /// <param name="target"></param>
    /// <param name="worldDest"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public Tweener PlayTween<T>(TweenPreset registeredTween, Transform target, T worldDest, TweenParameter? parameter = null)
    {
        return StartTween(_registeredTweens[registeredTween], target, worldDest, parameter);
    }
    private Tweener StartTween(Tweener tween, Transform target, object endValue, TweenParameter? parameter)
    {
        tween.target = target;
        tween.ChangeEndValue(endValue, parameter.HasValue ? parameter.Value._duration : TweenParameter.DEFAULTTWEENDURATION);
        if (parameter.HasValue)
            tween.SetEase(parameter.Value._ease);
        tween.OnComplete(() => tween.Rewind());
        tween.Restart();
        return tween;
    }

    internal Tween ScaleInFlipScaleOut(Transform transform, float rotationTarget, Vector3 finalValue)
    {
        var seq = DOTween.Sequence();
        seq.Append(transform.DOScale(.5f * Vector3.one, 0.2f)
            .SetEase(Ease.InQuad));
        seq.Append(RotateOnY(transform, rotationTarget));
        seq.Append(transform.DOScale(Vector3.one * 1.2f, 0.2f)
                            .SetEase(Ease.InQuad));
        seq.Append(transform.DOScale(finalValue, 0.2f)
                                    .SetEase(Ease.OutBounce));
        return seq;
    }
    internal Tween RotateOnY(Transform transform, float rotationTarget)
    {
        Vector3 target = new Vector3(0, rotationTarget, 0);
        return transform.DORotate(target, (transform.rotation.eulerAngles - target).magnitude > 001f ? 0.5f : 0f).SetEase(Ease.OutBack);
    }
}
