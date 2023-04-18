using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField, Header("Data"), OnValueChanged(nameof(UpdateCardData))]
    CardData _cardData;
    [Header("References")]
    [SerializeField]
    CardUI _cardUI;
    [SerializeField]
    SelectableCard _selectable;
    [SerializeField]
    CardAnimator _animator;
    [SerializeField]
    PlayerID _playerID;
    private void Start()
    {
        _cardUI.SetVisuals(_cardData);
        _selectable.Animator = _animator;
        _selectable.cardData = _cardData;
    }
    void UpdateCardData()
    {
        _cardUI.Card = _cardData;
    }
    public CardUI CardUI { get => _cardUI; }
    public SelectableCard Selectable { get => _selectable; }
    public PlayerID PlayerID { get => _playerID; }
    public CardData CardData
    {
        get => _cardData; set
        {
            _cardData = value;
            UpdateCardData();
        }
    }
    public override string ToString()
    {
        return _cardData.ToString();
    }
}
