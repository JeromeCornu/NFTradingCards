using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NaughtyAttributes;

public class CardUI : MonoBehaviour
{
    [Header("Data")]
    [SerializeField, OnValueChanged(nameof(SetVisuals)), AllowNesting()]
    private CardData _card;
    [Header("Common texts used")]
    [SerializeField]
    private string _quote = "'";
    [SerializeField]
    private string _descriptionPrefix = "Description: ";
    [SerializeField, Header("References")]
    private TextMeshProUGUI theName;
    [SerializeField]
    private GameObject color;
    [SerializeField]
    private GameObject image;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private TextMeshProUGUI ecologieAmount;
    [SerializeField]
    private TextMeshProUGUI economieAmount;
    [SerializeField]
    private TextMeshProUGUI socialAmount;
    [SerializeField]
    private TextMeshProUGUI quote;
    private void Start()
    {
        SetVisuals();
        GetComponent<Canvas>().worldCamera=Camera.main;
    }
    private void OnValueChanged()
    {
        SetVisuals();
    }
    public void SetVisuals()
    {
        setCardName(_card.Name);
        setCardDescription(_card.Descrition);
        setCardQuote(_card.Quote);
        setCardImage(_card.Sprite);
        setCardEconomieStat(_card[CardData.Pillar.Economic].Val);
        setCardEcologieStat(_card[CardData.Pillar.Ecologic].Val);
        setCardSocialStat(_card[CardData.Pillar.Social].Val);
        setCardColor(_card.Color);
    }
    private void setCardName(string newName)
    {
        theName.text = newName.ToUpper();
    }

    private void setCardDescription(string newDescription)
    {
        description.text = _descriptionPrefix + newDescription;
    }

    private void setCardQuote(string newQuote)
    {
        quote.text = _quote + newQuote + _quote;
    }

    private void setCardImage(Sprite newImage)
    {
        image.GetComponent<Image>().sprite = newImage;
    }

    private void setCardEcologieStat(int newEcologie)
    {
        ecologieAmount.text = newEcologie.ToString();
    }

    private void setCardEconomieStat(int newEconomie)
    {
        economieAmount.text = newEconomie.ToString();
    }

    private void setCardSocialStat(int newSocial)
    {
        socialAmount.text = newSocial.ToString();
    }

    private void setCardColor(Color newColor)
    {
        color.GetComponent<Image>().color = newColor;
    }
}
