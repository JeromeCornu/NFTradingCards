using Lean.Common;
using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    [Header("Deph of the card")]
    [SerializeField]
    public float normalDepth;
    [SerializeField]
    public float selectedDepth;

    [Header("Sound")]
    [SerializeField]
    private SoundManager soundManager;
    public AudioClip placeCardSound;
    public AudioClip SelectCardSound;


    public CardAnimator Animator { get => _animator; set => _animator = value; }

    public bool IsSelectable
    {
        get => _isSelectable; set
        {
            _isSelectable = value;
            _drag.enabled = _isSelectable;
        }
    }

    private void Start()
    {
        soundManager = Camera.main.GetComponent<SoundManager>();
    }

    protected override void OnSelected(LeanSelect select)
    {
        base.OnSelected(select);
        _origin = transform.parent;
        _animator.AdjustDepth(selectedDepth);
        transform.parent = null;
    }
    protected override void OnDeselected(LeanSelect select)
    {
        base.OnDeselected(select);
        Release();
    }


    private void Release()
    {
        if (CheckAreaToLockIn(out CardZone zone))
        {
            if (zone.AddCard(this))
            {
                soundManager.PlaySound(placeCardSound);
                zone.AddCard(this);
                _animator.AdjustDepth(normalDepth);
                return;
            }
            else
                _animator.CostTooHigh(cardData.Cost);
        }
        _animator.Reparent(_origin);
    }
    private bool CheckAreaToLockIn(out CardZone zone)
    {
        zone = null;
        return CastBoxUpAndDown(out RaycastHit info) && info.transform.gameObject.CompareTag(gameObject.tag)
               && info.transform.parent.TryGetComponent<CardZone>(out zone);
    }

    private bool CastBoxUpAndDown(out RaycastHit info)
    {
        return Physics.BoxCast(transform.position, .33f * Vector3.one, -transform.forward, out info, Quaternion.identity, 10f, _layerMask.value)
               || Physics.BoxCast(transform.position, .33f * Vector3.one, transform.forward, out info, Quaternion.identity, 10f, _layerMask.value);
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
