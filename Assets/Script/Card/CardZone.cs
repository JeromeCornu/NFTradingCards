using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardZone : MonoBehaviour
{
    [SerializeField]
    private Layout _layout;

    internal void AddCard(SelectableCard selectableCard)
    {
        selectableCard.transform.parent = _layout.transform;
        selectableCard.tag = TurnManager.Untagged;
        // Debug try : FindObjectOfType<TurnManager>().SwitchTurn();
    }
}
