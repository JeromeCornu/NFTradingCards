using Lean.Touch;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableGlobal : MonoBehaviour
{
    [SerializeField]
    private TurnManager _turn;
    [SerializeField]
    private LeanSelectByFinger _select;
    
    private void OnEnable()
    {
        _turn.TurnChanged.AddListener(Handle);
    }
    private void Handle(bool val)
    {
        _select.ScreenQuery.RequiredTag = _turn.GetTag();
    }
}
