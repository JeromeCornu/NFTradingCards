using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardZone : MonoBehaviour
{
    [SerializeField]
    private Layout _layout;
    [SerializeField]
    private GameSystem _game;
    [SerializeField]
    private PlayerID _id;
    private void OnValidate()
    {
        if (_game == null)
            _game = FindObjectOfType<GameSystem>();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="selectableCard"></param>
    /// <returns>True if the player could effectively afford</returns>
    internal bool AddCard(SelectableCard selectableCard)
    {
        var card = selectableCard.GetComponentInParent<Card>();
        if (!_game.AddCard(card.PlayerID.AsInt, card))
            return false;
        selectableCard.transform.parent = _layout.transform;
        selectableCard.tag = TurnManager.Untagged;
        selectableCard.Animator.Flip(true);
        return true;
        // Debug try : FindObjectOfType<TurnManager>().SwitchTurn();
    }
}
