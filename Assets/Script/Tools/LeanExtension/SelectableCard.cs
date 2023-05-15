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
    Origin _originalParent;
    private CardAnimator _animator;
    [HideInInspector]
    public CardData cardData;

    //[SerializeField] private float normalDepth=-1;
    [SerializeField]
    [Header("Depht of the card when selected")]
    private float selectedDepth;

    private SoundManager soundManager;
    [Header("Sound")]
    public AudioClip placeCardSound;
    public AudioClip SelectCardSound;

    private Camera _camera;

    public CardAnimator Animator { get => _animator; set => _animator = value; }
    public bool IsInAZone => ((0b1 << transform.parent.gameObject.layer) & _cardZoneLayerMask.value) != 0b0;
    //Checks if the original parent, i.e the hand, belongs to the player
    public bool ComesFromPlayerHand => _originalParent.parent.GetComponent<PlayerID>().IsPlayer;
    public bool BelongsToPlayer => gameObject.CompareTag(TurnManager.PlayerTag);
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
        _originalParent = new Origin(transform);
        PullOnForeground();
        transform.parent = null;
    }
    protected override void OnDeselected(LeanSelect select)
    {
        if (!_isSelectable)
            return;

        base.OnDeselected(select);
        Release(Get_originalParent());
    }

    public void PullOnForeground()
    {
        _animator.AdjustDepth(selectedDepth);
        //_animator.AdjustDepth(normal ? normalDepth : selectedDepth);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 halfExtents = BoxCastSize * transform.lossyScale;
        halfExtents.z = .1f;
        Gizmos.DrawCube(transform.position, halfExtents);
    }

    private Origin Get_originalParent()
    {
        return _originalParent;
    }

    private void Release(Origin _originalParent)
    {
        /* for (float i = 0f; i < 3f; i += .1f)
         {
             Debug.Log(i + " => scaled : " + CastBoxUpAndDown(out RaycastHit _, i));
         }*/
        if (CheckAreaToLockIn(out CardZone zone, out Vector3 point))
        {
            if (zone.AddCard(this, point))
            {
                soundManager.PlaySound(placeCardSound);
                return;
            }
            else
                _animator.CostTooHigh(cardData.Cost);
        }
        //We use default "sliding" tween
        _animator.Reparent(_originalParent.parent, _originalParent.indexInParent,0);
    }
    private bool CheckAreaToLockIn(out CardZone zone, out Vector3 hitPoint)
    {
        zone = null;
        hitPoint = Vector3.zero;
        if (CastBoxUpAndDown(out RaycastHit info))
        {
            hitPoint = info.point;
            return info.transform.parent.TryGetComponent<CardZone>(out zone);
        }
        return false;
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
    //Generated wrapper for origin
    public struct Origin
    {
        public Transform parent;
        public int indexInParent;
        public Origin(Transform transform) : this(transform.parent, transform.GetSiblingIndex()) { }
        public Origin(Transform parent, int item2)
        {
            this.parent = parent;
            indexInParent = item2;
        }

        public override bool Equals(object obj)
        {
            return obj is Origin other &&
                   EqualityComparer<Transform>.Default.Equals(parent, other.parent) &&
                   indexInParent == other.indexInParent;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(parent, indexInParent);
        }

        public void Deconstruct(out Transform item1, out int item2)
        {
            item1 = parent;
            item2 = indexInParent;
        }

        public static implicit operator (Transform, int)(Origin value)
        {
            return (value.parent, value.indexInParent);
        }

        public static implicit operator Origin((Transform, int) value)
        {
            return new Origin(value.Item1, value.Item2);
        }
        public static implicit operator Origin(Transform value)
        {
            return new Origin(value);
        }
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