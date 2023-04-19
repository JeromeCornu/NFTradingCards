using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using static GameSystem;

public class GameSystem : MonoBehaviour
{
    public class Player
    {
        public List<Card> CardOnBoard;
        public int Temperature;
        public int Money;
        public int PeopleSatistfaction;
        public bool HasStarted;
        public bool HasLost;

        public Player(int iInitTemp, int iInitMoney, int iInitSatisfaction)
        {
            CardOnBoard = new();
            Temperature = iInitTemp;
            Money = iInitMoney;
            PeopleSatistfaction = iInitSatisfaction;
            HasStarted = false;
            HasLost = false;
        }
        public override string ToString()
        {
            return "Temp : " + Temperature + ", �: " + Money + ", satist : " + PeopleSatistfaction +
                " lost ? " + HasLost + " Board : " + string.Join('\n', CardOnBoard);
        }
    }

    [SerializeField] int m_StartTemp = 60;
    [SerializeField] int m_StartMoney = 0;
    [SerializeField] int m_StartSatisfaction = 50;
    [SerializeField] int m_MaxTemperature = 100;
    [SerializeField] int m_MinSatisfaction = 10;

    private List<Player> m_Players = new List<Player>();
    public Player this[int i]=> m_Players[i];
    [SerializeField] int m_NbPlayer = 2;

    [SerializeField] TurnManager m_TurnManager;

    [SerializeField] DeckBehaviour[] m_PlayersDeck;

    public DeckBehaviour getDeckBehaviour(int prmIndex)
    {
        Assert.IsTrue(0 <= prmIndex && prmIndex < m_NbPlayer);
        return m_PlayersDeck[prmIndex];
    }
        
    public UnityEvent<int> OnPlayerLost;
    public UnityEvent<(int, Player)> OnPlayerValuesUpdates;

    public TurnManager TurnManager { get => m_TurnManager; set => m_TurnManager = value; }

    void Awake()
    {
        if (OnPlayerLost == null)
            OnPlayerLost = new();
        for (int playerIndex = 0; playerIndex < m_NbPlayer; playerIndex++){
            Player p = new Player(m_StartTemp, m_StartMoney, m_StartSatisfaction);
            m_Players.Add(p);
            OnPlayerValuesUpdates.Invoke((playerIndex, p));
        }
        if (OnPlayerValuesUpdates == null)
            OnPlayerValuesUpdates = new();
    }

    // Start is called before the first frame update
    void Start()
    {
        Assert.AreEqual(m_Players.Count, m_PlayersDeck.Length);
        m_TurnManager.TurnChanged.AddListener(OnEndTurn);
    }

    void OnEndTurn(bool iIsPlayer)
    {
        Produce(PlayerID.IsPlayerAsInt(iIsPlayer)); // proc for new player
        DrawCardForPlayer(PlayerID.IsPlayerAsInt(iIsPlayer));
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
        OnPlayerValuesUpdates.Invoke((iPlayerIndex, player));
        return hasDied;
    }

    public bool TryPlayCard(int iPlayerIndex, Card iCard) => AddCard(iPlayerIndex, iCard);

    public void DrawCardForPlayer(int iPlayerIndex)
    {
        Assert.IsTrue(0 <= iPlayerIndex && iPlayerIndex < m_NbPlayer);
        Player player = m_Players[iPlayerIndex];
        int nCardToDraw = player.PeopleSatistfaction / 33 + 1;
        if (!player.HasStarted)
            nCardToDraw = 4;
        m_PlayersDeck[iPlayerIndex].DrawCardsWithoutReturn(nCardToDraw);
        player.HasStarted = true;
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
        OnPlayerValuesUpdates.Invoke((iPlayerIndex, player));
        //m_TurnManager.SwitchTurn();
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
