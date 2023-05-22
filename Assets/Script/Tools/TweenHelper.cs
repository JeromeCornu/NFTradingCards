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

public class TweenHelper : MonoBehaviour
{
    [System.Serializable]
    private struct TweenParameter
    {
        [SerializeField]
        public float _duration;
        [SerializeField]
        public Ease _ease;
        [SerializeField]
        public int _loops;
        public static TweenParameter Default => new TweenParameter();
        public const float DEFAULTTWEENDURATION = 1f;
        public TweenParameter(float duration = DEFAULTTWEENDURATION, Ease ease = Ease.Linear, int loops = 1)
        {
            _duration = duration;
            _ease = ease;
            _loops = loops;
        }
        public TweenParameter(int loops) : this()
        {
            _loops = loops;
        }
        public TweenParameter(Ease ease) : this()
        {
            _ease = ease;
        }
        public TweenParameter(Ease ease, int loops) : this()
        {
            _loops = loops;
            _ease = ease;
        }
    }
    [SerializeField, Tooltip("Not currently used")]
    private TweenParameter[] _tweenParameters;
    private Dictionary<TweenPreset, Tweener> _registeredTweens;
    private TweenParameter CurrentParameter => TweenParameter.Default;
    //In case we don't override the duration when trying to call a tween
    private void Awake()
    {
        _registeredTweens = new();
        DOTween.defaultEaseType = CurrentParameter._ease;
        //DefaultTweenRotation & target will be rewritten when passing parameters
        RegisterTween(TweenPreset.MoveTo, ShortcutExtensions.DOMove((Transform)null, Vector3.zero, CurrentParameter._duration).SetEase(CurrentParameter._ease));
        RegisterTween(TweenPreset.Flip, RotateOnY(null, 180f));
        RegisterTween(TweenPreset.Spiral, DOTweenProShortcuts.DOSpiral((Transform)null, CurrentParameter._duration, new Vector3(0, 1, 1)));
        RegisterTween(TweenPreset.RotateNTimes, ShortcutExtensions.DORotate((Transform)null, new Vector3(0, 360, 0), CurrentParameter._duration, RotateMode.FastBeyond360));
    }
    private void RegisterTween(TweenPreset key, Tweener tween)
    {
        tween.SetAutoKill(false);
        tween.Pause();
        tween.target = null;
        _registeredTweens.Add(key, tween);
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
    public Tweener PlayTween<T>(TweenPreset registeredTween, Transform target, T worldDest)
    {
        return StartTween(_registeredTweens[registeredTween], target, worldDest, null);
    }
    private Tweener StartTween(Tweener tween, Transform target, object endValue, TweenParameter? parameter)
    {
        tween.target = target;
        tween.SetLoops(1);
        tween.ChangeEndValue(endValue, parameter.HasValue ? parameter.Value._duration : TweenParameter.DEFAULTTWEENDURATION);
        if (parameter.HasValue)
        {
            var val = parameter.Value;
            tween.SetEase(val._ease);
            if (val._loops != 1)
                tween.SetLoops(val._loops);
        }
        tween.OnComplete(() => tween.Rewind());
        tween.Restart();
        return tween;
    }

    internal Tween ScaleInFlipScaleOut(Transform transform, float rotationTarget, Vector3 finalValue)
    {
        var seq = DOTween.Sequence();
        seq.Append(transform.DOScale(.5f * Vector3.one, 0.2f)
            .SetEase(Ease.InQuad));
        //We cannot modify end value of tween inside of a sequence and the flip one actually rewrites it's duration (by it's end value) when it starts, this is a workaround
        seq.AppendCallback(() =>
        {
            PlayTween<Vector3>(TweenPreset.Flip, transform, new(0, rotationTarget, 0)).OnComplete(() =>
            {
                seq.Play();
            });
            seq.Pause();
        });
        seq.Append(transform.DOScale(Vector3.one * 1.2f, 0.2f)
                            .SetEase(Ease.InQuad));
        seq.Append(transform.DOScale(finalValue, 0.2f)
                                    .SetEase(Ease.OutBounce));
        return seq;
    }
    internal Tweener RotateOnY(Transform transform, float rotationTarget)
    {
        Vector3 target = new Vector3(0, rotationTarget, 0);
        var tween = ShortcutExtensions.DORotate(transform, target, TweenParameter.DEFAULTTWEENDURATION).SetEase(Ease.OutBack);
        //When the tween starts, if we already are at the target rotation, we set duration to 0, so it will be instant
        tween.OnStart(() =>
        {
            if (((tween.target as Transform).rotation.eulerAngles - target).magnitude < .0001f)
                tween.ChangeEndValue(tween.endValue, 0f);
        });
        return tween;
    }
}
