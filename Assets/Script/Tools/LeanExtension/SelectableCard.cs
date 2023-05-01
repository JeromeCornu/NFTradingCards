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
    private LayerMask _cardZoneLayerMask;
    [SerializeField, Range(.01f, 2f)]
    private float BoxCastSize = .75f;
    Transform _originalParent;
    private CardAnimator _animator;
    [HideInInspector]
    public CardData cardData;

    [Header("Deph of the card")]
    [SerializeField]
    private float normalDepth;
    [SerializeField]
    private float selectedDepth;

    [Header("Sound")]
    private SoundManager soundManager;
    public AudioClip placeCardSound;
    public AudioClip SelectCardSound;

    private Camera _camera;

    public CardAnimator Animator { get => _animator; set => _animator = value; }
    public bool IsInAZone => (transform.parent.gameObject.layer << 0b1 & _cardZoneLayerMask.value) != 0b0;
    //Checks if the original parent, i.e the hand, belongs to the player
    public bool IsInPlayerHand => _originalParent.GetComponent<PlayerID>().IsPlayer;

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
        _camera = Camera.main;
        _drag.Camera = _camera;
        soundManager = _camera.GetComponent<SoundManager>();
    }

    protected override void OnSelected(LeanSelect select)
    {
        if (!_isSelectable)
            return;

        base.OnSelected(select);
        _originalParent = transform.parent;
        AdjustDepth(false);
        transform.parent = null;
    }
    protected override void OnDeselected(LeanSelect select)
    {
        if (!_isSelectable)
            return;

        base.OnDeselected(select);
        Release();
    }

    public void AdjustDepth(bool normal)
    {
        _animator.AdjustDepth(normal ? normalDepth : selectedDepth);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 halfExtents = BoxCastSize * transform.lossyScale;
        halfExtents.z = .1f;
        Gizmos.DrawCube(transform.position, halfExtents);
    }
    private void Release()
    {
        /* for (float i = 0f; i < 3f; i += .1f)
         {
             Debug.Log(i + " => scaled : " + CastBoxUpAndDown(out RaycastHit _, i));
         }*/
        if (CheckAreaToLockIn(out CardZone zone))
        {
            if (zone.AddCard(this))
            {
                soundManager.PlaySound(placeCardSound);
                AdjustDepth(true);
                return;
            }
            else
                _animator.CostTooHigh(cardData.Cost);
        }
        _animator.Reparent(_originalParent);
    }
    private bool CheckAreaToLockIn(out CardZone zone)
    {
        zone = null;
        return CastBoxUpAndDown(out RaycastHit info) && info.transform.parent.TryGetComponent<CardZone>(out zone);
    }

    private bool CastBoxUpAndDown(out RaycastHit info, float size)
    {
        var forward = transform.position - _camera.transform.position;
        Vector3 halfExtents = BoxCastSize * transform.lossyScale;
        halfExtents.z = .1f;
        return Physics.BoxCast(transform.position, halfExtents, -forward, out info, Quaternion.identity, 10f, _cardZoneLayerMask.value)
               || Physics.BoxCast(transform.position, halfExtents, forward, out info, Quaternion.identity, 10f, _cardZoneLayerMask.value);
    }
    private bool CastBoxUpAndDown(out RaycastHit info)
    {
        return CastBoxUpAndDown(out info, BoxCastSize);
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
