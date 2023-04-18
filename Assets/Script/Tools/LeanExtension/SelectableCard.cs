using Lean.Common;
using Lean.Touch;
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
        transform.parent = null;
    }
    protected override void OnDeselected(LeanSelect select)
    {
        base.OnDeselected(select);
        if ((Physics.BoxCast(transform.position, .33f * transform.lossyScale, -transform.forward, out RaycastHit info, Quaternion.identity, 2f, _layerMask.value)
                ||
                Physics.BoxCast(transform.position, .33f * transform.lossyScale, transform.forward, out info, Quaternion.identity, 2f, _layerMask.value)
            )
            && info.transform.gameObject.CompareTag(gameObject.tag)
            && info.transform.parent.TryGetComponent<CardZone>(out CardZone zone)
            )
        {
            zone.AddCard(this);
        }
        else
            transform.parent = _origin;
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
