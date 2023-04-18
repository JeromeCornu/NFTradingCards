using Lean.Common;
using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableCard : LeanSelectableBehaviour
{
    [SerializeField]
    private bool _isSelectable = true;
    [SerializeField]
    private LeanDragTranslate _drag;
    [SerializeField]
    private LayerMask _layerMask;
    Transform _origin;
    private CardAnimator _animator;
    [HideInInspector]
    public CardData cardData;

    public CardAnimator Animator { get => _animator; set => _animator = value; }

    public bool IsSelectable
    {
        get => _isSelectable; set
        {
            _isSelectable = value;
            _drag.enabled = _isSelectable;
        }
    }

    protected override void OnSelected(LeanSelect select)
    {
        base.OnSelected(select);
        _origin = transform.parent;
        _animator.AdjustDepth(true);
        transform.parent = null;
    }
    protected override void OnDeselected(LeanSelect select)
    {
        base.OnDeselected(select);
        _animator.AdjustDepth(false);
        Release();
    }

    private void Release()
    {
        if (ValidateCardCost(cardData.Cost))
        {
            if (CheckAreaToLockIn(out CardZone zone))
            {
                zone.AddCard(this);
                return;
            }
        }
        else
            _animator.CostTooHigh(cardData.Cost);
        _animator.Reparent(_origin);
    }

    private bool ValidateCardCost(int cost)
    {
        return UnityEngine.Random.Range(0f, 1f) > .25f;
    }

    private bool CheckAreaToLockIn(out CardZone zone)
    {
        zone = null;
        return CastBoxUpAndDown(out RaycastHit info)
               && info.transform.gameObject.CompareTag(gameObject.tag)
               && info.transform.parent.TryGetComponent<CardZone>(out zone);
    }

    private bool CastBoxUpAndDown(out RaycastHit info)
    {
        return Physics.BoxCast(transform.position, .33f * transform.lossyScale, -transform.forward, out info, Quaternion.identity, 2f, _layerMask.value)
               || Physics.BoxCast(transform.position, .33f * transform.lossyScale, transform.forward, out info, Quaternion.identity, 2f, _layerMask.value);
    }
    /*[SerializeField]
LeanSelectable[] _selectablesToForward;
public void ForwardSelect(LeanSelect l)
{
Forward(l, true);
}
public void ForwardDeselect(LeanSelect l)
{
Forward(l, false);
}

private void Forward(LeanSelect origin, bool state)
{
if (state)
foreach (var s in _selectablesToForward)
 origin.Select(s);
else
foreach (var s in _selectablesToForward)
 origin.Deselect(s);
}*/
}
