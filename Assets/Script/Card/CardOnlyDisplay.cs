using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardOnlyDisplay : MonoBehaviour
{
    [SerializeField, Header("Data"), OnValueChanged(nameof(UpdateCardData))]
    CardData _cardData;
    [Header("References")]
    [SerializeField]
    CardUI _cardUI;
    internal object canvasGroup;

    private void Start()
    {
        _cardUI.SetVisuals(_cardData);
    }
    void UpdateCardData()
    {
        _cardUI.Card = _cardData;
    }
    public CardUI CardUI { get => _cardUI; }
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
