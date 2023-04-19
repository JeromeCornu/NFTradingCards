using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckBehaviour : MonoBehaviour
{
    private Stack<Card> m_CardsStack = new ();
    private List<Card> m_Hand = new ();

    public void InitCardStack(List<Card> iCards)
    {
        List<Card> cards = iCards;
        var randomList = cards.OrderBy(card => Random.Range(0.0f, 1.0f));
        m_CardsStack.Clear();
        foreach(Card card in randomList)
            m_CardsStack.Push(card);
    }

    // return the number of drawn cards
    public List<Card> DrawCards(int iNbCardsToDraw)
    {
        if(iNbCardsToDraw > m_CardsStack.Count)
        {
            return _DrawCards(m_CardsStack.Count);
        }

        return _DrawCards(iNbCardsToDraw);
    }

    private List<Card> _DrawCards(int iNbCardsToDraw)
    {
        List<Card> drawnCards = new ();
        for(int i = 0; i < iNbCardsToDraw; i++)
            drawnCards.Add(m_CardsStack.Pop());

        m_Hand.AddRange(drawnCards);
        return drawnCards;
    }

    public bool RemoveCardFromHand(Card iCard)
    {
        return m_Hand.Remove(iCard);
    }
}
