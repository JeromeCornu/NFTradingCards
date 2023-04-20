using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NaughtyAttributes;

public class CardUI : MonoBehaviour
{

    [SerializeField] private GameObject cardGameObject;

    [Header("Common texts used")]

    [SerializeField]
    private string _quote = "'";
    [SerializeField]
    private string _descriptionPrefix = "Description: ";
    [SerializeField, Header("References")]
    private TextMeshProUGUI theName;
    [SerializeField]
    private TextMeshProUGUI price;
    [SerializeField]
    private Color color;
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


    public CardData Card
    {
        set
        {
            SetVisuals(value);
        }
    }
    public void SetVisuals(CardData _card)
    {
        if (_card == null)
            return;
        SetCardName(_card.Name);
        SetCardCost(_card.Cost);
        SetCardDescription(_card.Descrition);
        SetCardQuote(_card.Quote);
        SetCardImage(_card.Sprite);
        SetCardEconomieStat(_card[CardData.Pillar.Economic].Val);
        SetCardEcologieStat(_card[CardData.Pillar.Ecologic].Val);
        SetCardSocialStat(_card[CardData.Pillar.Social].Val);
        SetColor(_card.Color);
        Debug.Log(_card.Color.ToString());
    }

    private void SetCardName(string prmName)
    {
        theName.text = prmName.ToUpper();
    }

    private void SetCardCost(int prmCost)
    {
        price.text = prmCost.ToString() + "M";
    }

    private void SetCardDescription(string prmDescription)
    {
        description.text = _descriptionPrefix + prmDescription;
    }

    private void SetCardQuote(string prmQuote)
    {
        quote.text = _quote + prmQuote + _quote;
    }

    private void SetCardImage(Sprite prmImage)
    {
        image.GetComponent<Image>().sprite = prmImage;
    }

    private void SetCardEcologieStat(int prmEcologie)
    {
        ecologieAmount.text = prmEcologie.ToString();
    }

    private void SetCardEconomieStat(int prmEconomie)
    {
        economieAmount.text = prmEconomie.ToString();
    }

    private void SetCardSocialStat(int prmSocial)
    {
        socialAmount.text = prmSocial.ToString();
    }

    private void SetColor(Color prmColor)
    {
        color = prmColor;
        SetCardColor(color);
    }

    private void SetCardColor(Color prmColor)
    {
        cardGameObject.GetComponent<SpriteRenderer>().material.SetColor("_Color", prmColor);
    }

    private void ActivateHolo()
    {
        cardGameObject.GetComponent<SpriteRenderer>().material.SetInteger("_ActivateHolo", 1);
    }

    private void DisableHolo()
    {
        cardGameObject.GetComponent<SpriteRenderer>().material.SetInteger("_ActivateHolo", 0);
    }
}
