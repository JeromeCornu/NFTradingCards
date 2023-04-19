using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class FillDeck : MonoBehaviour
{
    public CardData[] cardsData;
    public GameObject[] cards3D;
    public GameObject[] decks;

    // Start is called before the first frame update
    void Start()
    {
        // Get all CardData
        var loaded = LoadCardData();
        Debug.Log(loaded);

        // Get all Cards3D
        List<GameObject> allChildObjects = new List<GameObject>();

        foreach (GameObject parentObject in decks)
        {
            GameObject[] childObjects = GetAllCards(parentObject);
            allChildObjects.AddRange(childObjects);
        }
        cards3D = allChildObjects.ToArray();

        FillCardsData();
    }

    private bool LoadCardData()
    {
        cardsData = Resources.LoadAll<CardData>("Data");

        if (cardsData.Length != 0) { return true; }
        else { return false; };
    }

    private GameObject[] GetAllCards(GameObject parentObject)
    {
        GameObject[] childObjects = new GameObject[parentObject.transform.childCount];

        for (int i = 0; i < parentObject.transform.childCount; i++)
        {
            childObjects[i] = parentObject.transform.GetChild(i).gameObject;
        }

        return childObjects;
    }

    private void FillCardsData()
    {
        // Shuffle Deck and Cards
        ShuffleGameObject(cards3D); // Shuffle cards ref if we need to re-put info in the last cards, si the last cards will not be always the same ones
        ShuffleCardData(cardsData); // Shuffle info to put in card

        int j = 0;
        for (int i = 0; i < cards3D.Length; i++)
        {
            j =+ 1;
            cards3D[i].GetComponent<Card>().CardData = cardsData[j];

            // if we used all the diffrent CardsData, use it again
            if (cardsData.Length >= j - 1)
            {
                j = 0;
                ShuffleCardData(cardsData);
            }
        }
    }

    private CardData[] ShuffleCardData(CardData[] arrayToShuffle)
    {
        int n = arrayToShuffle.Length;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            CardData temp = arrayToShuffle[k];
            arrayToShuffle[k] = arrayToShuffle[n];
            arrayToShuffle[n] = temp;
        }

        return arrayToShuffle;
    }

    private GameObject[] ShuffleGameObject(GameObject[] arrayToShuffle)
    {
        int n = arrayToShuffle.Length;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            GameObject temp = arrayToShuffle[k];
            arrayToShuffle[k] = arrayToShuffle[n];
            arrayToShuffle[n] = temp;
        }

        return arrayToShuffle;
    }

}
