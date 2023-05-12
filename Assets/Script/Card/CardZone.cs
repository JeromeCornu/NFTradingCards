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
    public IEnumerable<Transform> ChildsInZone => _layout;
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
    internal bool AddCard(SelectableCard selectableCard, Vector3 childDesiredPosition)
    {
        if (AddCard(selectableCard))
        {
            selectableCard.transform.SetSiblingIndex(_layout.GetCorrectIndex(childDesiredPosition));
            return true;
        }
        else
            return false;
    }
    internal bool AddCard(SelectableCard selectableCard)
    {
        var card = selectableCard.GetComponentInParent<Card>();
        if (!_game.AddCard(_id.AsInt, card))
            return false;
        selectableCard.transform.parent = _layout.transform;
        selectableCard.tag = TurnManager.Untagged;
        selectableCard.Animator.Flip(true);
        selectableCard.AdjustDepth(true);
        return true;
    }
}
