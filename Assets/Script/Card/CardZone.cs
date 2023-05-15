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
        var card = selectableCard.GetComponentInParent<Card>();
        if (!_game.AddCard(_id.AsInt, card))
            return false;
        var ind = _layout.GetPredictIndex(childDesiredPosition);
        //We use intense transition tween
        selectableCard.Animator.Reparent(_layout, ind,1);
        selectableCard.tag = TurnManager.Untagged;
        //selectableCard.Animator.Flip(true);
        selectableCard.transform.SetSiblingIndex(ind);
        return true;
    }
    internal bool AddCard(SelectableCard selectableCard)
    {
        return AddCard(selectableCard, _layout.transform.position);
    }
}
