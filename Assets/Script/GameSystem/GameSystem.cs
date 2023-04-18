using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
public class GameSystem : MonoBehaviour
{
    public class Player
    {
        public List<Card> CardOnBoard;
        public int Temperature;
        public int Money;
        public int PeopleSatistfaction;
        public bool HasLost;

        public Player(int iInitTemp, int iInitMoney, int iInitSatisfaction)
        {
            CardOnBoard = new();
            Temperature = iInitTemp;
            Money = iInitMoney;
            PeopleSatistfaction = iInitSatisfaction;
            HasLost = false;
        }
        public override string ToString()
        {
            return "Temp : " + Temperature + ", £: " + Money + ", satist : " + PeopleSatistfaction +
                " lost ? " + HasLost + " Board : " + string.Join('\n', CardOnBoard);
        }
    }

    [SerializeField] int m_StartTemp = 60;
    [SerializeField] int m_StartMoney = 0;
    [SerializeField] int m_StartSatisfaction = 50;
    [SerializeField] int m_MaxTemperature = 100;
    [SerializeField] int m_MinSatisfaction = 10;

    private List<Player> m_Players = new List<Player>();
    [SerializeField] int m_NbPlayer = 2;

    [SerializeField] TurnManager m_TurnManager;

    public UnityEvent<int> OnPlayerLost;

    void Awake()
    {
        if (OnPlayerLost == null)
            OnPlayerLost = new();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int playerIndex = 0; playerIndex < m_NbPlayer; playerIndex++)
            m_Players.Add(new Player(m_StartTemp, m_StartMoney, m_StartSatisfaction));

        m_TurnManager.TurnChanged.AddListener(OnEndTurn);
    }

    void OnEndTurn(bool iWasPlayer)
    {
        if (iWasPlayer)
            Produce(0);
        else
            Produce(1);
    }

    private void _UpdatePlayerRessources(Player iPlayer)
    {
        foreach (Card card in iPlayer.CardOnBoard)
        {
            iPlayer.Temperature -= card.CardData[CardData.Pillar.Ecologic].Val;
            iPlayer.Money += card.CardData[CardData.Pillar.Economic].Val;
            iPlayer.PeopleSatistfaction += card.CardData[CardData.Pillar.Social].Val;
            iPlayer.PeopleSatistfaction = Mathf.Clamp(iPlayer.PeopleSatistfaction, 0, 100);
        }
    }

    private bool _HasPlayerJustDied(Player iPlayer)
    {
        if (iPlayer.HasLost)
            return false;

        if (iPlayer.PeopleSatistfaction < m_MinSatisfaction)
        {
            iPlayer.HasLost = true;
            return true;
        }

        if (iPlayer.Temperature > m_MaxTemperature)
        {
            iPlayer.HasLost = true;
            return true;
        }

        return false;
    }
    // oDeadPlayerIndices return only indices of players that died in this turn
    public bool Produce(int iPlayerIndex)
    {
        Assert.IsTrue(0 <= iPlayerIndex && iPlayerIndex < m_NbPlayer);
        Player player = m_Players[iPlayerIndex];
        _UpdatePlayerRessources(player);
        bool hasDied = _HasPlayerJustDied(player);
        if (hasDied)
            OnPlayerLost.Invoke(iPlayerIndex);
        Debug.Log(iPlayerIndex + " <index after producing : " + player.ToString());
        return hasDied;
    }

    // return if player can afford the the card cost
    public bool AddCard(int iPlayerIndex, Card iCard)
    {
        Assert.IsTrue(0 <= iPlayerIndex && iPlayerIndex < m_NbPlayer);
        Player player = m_Players[iPlayerIndex];
        if (iCard.CardData.Cost > player.Money)
            return false;

        player.CardOnBoard.Add(iCard);
        player.Money -= iCard.CardData.Cost;
        m_TurnManager.SwitchTurn();
        return true;
    }

    // return if deletion happened
    public bool RemoveCard(int iPlayerIndex, Card iCard)
    {
        Assert.IsTrue(0 <= iPlayerIndex && iPlayerIndex < m_NbPlayer);
        return m_Players[iPlayerIndex].CardOnBoard.Remove(iCard);
    }

    public int GetPlayerMoney(int iPlayerIndex)
    {
        Assert.IsTrue(0 <= iPlayerIndex && iPlayerIndex < m_NbPlayer);
        return m_Players[iPlayerIndex].Money;
    }

    public int GetPlayerTemperature(int iPlayerIndex)
    {
        Assert.IsTrue(0 <= iPlayerIndex && iPlayerIndex < m_NbPlayer);
        return m_Players[iPlayerIndex].Temperature;
    }

    public int GetPlayerPeopleSatisfaction(int iPlayerIndex)
    {
        Assert.IsTrue(0 <= iPlayerIndex && iPlayerIndex < m_NbPlayer);
        return m_Players[iPlayerIndex].PeopleSatistfaction;
    }
}
