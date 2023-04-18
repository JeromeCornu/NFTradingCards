using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField]
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

    private void setCardName(string newName)
    {
        theName.text = newName.ToUpper();
    }

    private void setCardDescription(string newDescription)
    {
        description.text = "Description: " + newDescription;
    }

    private void setCardQuote(string newQuote)
    {
        quote.text = "'" + newQuote + "'";
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
