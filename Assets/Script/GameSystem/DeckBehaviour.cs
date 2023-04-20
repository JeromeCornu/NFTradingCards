using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckBehaviour : MonoBehaviour
{
    [SerializeField] GameObject m_HandObject;

    private Stack<Card> m_CardsStack = new();
    private List<Card> m_Hand = new();
    public float CalculateAverageCost()
    {
        return m_CardsStack.Sum((c) => c.CardData.Cost) / (float)m_CardsStack.Count;
    }
    
    public List<Card> MHand => m_Hand;

    private void Start()
    {
        Card[] childrenCards = GetComponentsInChildren<Card>();
        List<Card> cards = new();
        foreach (Card card in childrenCards)
            cards.Add(card);

        InitCardStack(cards);
    }

    public void InitCardStack(List<Card> iCards)
    {
        List<Card> cards = iCards;
        var randomList = cards.OrderBy(card => Random.Range(0.0f, 1.0f));
        m_CardsStack.Clear();
        foreach (Card card in randomList)
        {
            m_CardsStack.Push(card);
            card.gameObject.transform.parent = transform;
            card.gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            card.gameObject.GetComponent<SelectableCard>().IsSelectable = false;
        }
    }

    public void DrawCardsWithoutReturn(int iNbCardsToDraw)
    {
        DrawCards(iNbCardsToDraw);
    }

    // return the number of drawn cards
    public List<Card> DrawCards(int iNbCardsToDraw)
    {
        if (iNbCardsToDraw > m_CardsStack.Count)
        {
            return _DrawCards(m_CardsStack.Count);
        }

        return _DrawCards(iNbCardsToDraw);
    }

    private List<Card> _DrawCards(int iNbCardsToDraw)
    {
        List<Card> drawnCards = new();
        for (int i = 0; i < iNbCardsToDraw; i++)
        {
            Card card = m_CardsStack.Pop();
            card.transform.parent = m_HandObject.transform;
            card.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            card.gameObject.GetComponent<SelectableCard>().IsSelectable = true;
            card.GetComponent<CardAnimator>().Flip(true);
            drawnCards.Add(card);
        }
        m_HandObject.GetComponent<Layout>().UpdateLayout();
        m_Hand.AddRange(drawnCards);

        return drawnCards;
    }

    public bool RemoveCardFromHand(Card iCard)
    {
        // animation and addition to card zone is managed by drag n drop
        return m_Hand.Remove(iCard);
    }
}
