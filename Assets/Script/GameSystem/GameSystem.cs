using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using static GameSystem;

public class GameSystem : MonoBehaviour
{
    public class Player
    {
        public const int NbOfCardDrawTreshold = 3;
        private const int treshold = 100 / NbOfCardDrawTreshold;
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
        public bool CanAffordCard(Card iCard)
        {
            return iCard.CardData.Cost <= this.Money;
        }
        public override string ToString()
        {
            return "Temp : " + Temperature + ", �: " + Money + ", satist : " + PeopleSatistfaction +
                " lost ? " + HasLost + " Board : " + string.Join('\n', CardOnBoard);
        }

        internal int NbOfCardToDraw() => PeopleSatistfaction / treshold /*+ 1*/;


    }

    [SerializeField] int m_StartTemp = 60;
    [SerializeField] int m_StartMoney = 0;
    [SerializeField] int m_StartSatisfaction = 50;
    [SerializeField] int m_MaxTemperature = 100;
    [SerializeField] int m_MinSatisfaction = 10;

    public int MStartTemp => m_StartTemp;

    public int MStartMoney => m_StartMoney;

    public int MStartSatisfaction => m_StartSatisfaction;
    public int MMaxTemperature { get => m_MaxTemperature; }
    public int MMinSatisfaction { get => m_MinSatisfaction; }

    private List<Player> m_Players = new List<Player>();
    public Player this[int i] => m_Players[i];
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
        
        if (OnPlayerValuesUpdates == null)
            OnPlayerValuesUpdates = new();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int playerIndex = 0; playerIndex < m_NbPlayer; playerIndex++)
        {
            Player p = new Player(m_StartTemp, m_StartMoney, m_StartSatisfaction);
            m_Players.Add(p);
            OnPlayerValuesUpdates.Invoke((playerIndex, p));
        }
        
        Assert.AreEqual(m_Players.Count, m_PlayersDeck.Length);
        m_TurnManager.TurnChanged.AddListener(OnEndTurn);
    }

    void OnEndTurn(bool iIsPlayer)
    {
        Player playerEndedTurn = m_Players[PlayerID.IsPlayerAsInt(!iIsPlayer)];
        playerEndedTurn.HasStarted = true;
        Produce(PlayerID.IsPlayerAsInt(iIsPlayer)); // proc for new player
        DrawCardForPlayer(PlayerID.IsPlayerAsInt(iIsPlayer));
    }

    private void _UpdatePlayerRessources(Player iPlayer)
    {
        foreach (Card card in iPlayer.CardOnBoard)
        {
            iPlayer.Temperature -= card.CardData[CardData.Pillar.Ecologic].Val;
            iPlayer.Money += card.CardData[CardData.Pillar.Economic].Val;
            Mathf.Clamp(iPlayer.Money, 1, iPlayer.Money);
            iPlayer.PeopleSatistfaction += card.CardData[CardData.Pillar.Social].Val;
            Mathf.Clamp(iPlayer.PeopleSatistfaction, 0, 100);
        }
    }

    public Vector3 GetResourcesForNextRound(GameSystem.Player prmPlayer) 
    {
        int Money = 0;
        int Temp = 0;
        int PplSat = 0;

        foreach (Card card in prmPlayer.CardOnBoard)
        {
            Money += card.CardData[CardData.Pillar.Economic].Val;
            Temp -= card.CardData[CardData.Pillar.Ecologic].Val;
            PplSat += card.CardData[CardData.Pillar.Social].Val;
        }

        return new Vector3(Money, Temp, PplSat);

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

    private void _CheckNbLeftCards(int iPlayerIndex)
    {
        if (m_PlayersDeck[iPlayerIndex].MHand.Count > 0)
            return;
        if (m_PlayersDeck[iPlayerIndex].MStack.Count > 0)
            return;

        // no card left, end of the game
        // computing who lost and send events
        int minTemperature = m_MaxTemperature + 1;
        List<int> playerIdsWithMinTemp = new ();

        Action<int, Player> newMinScore = (int playerId, Player player) =>
        {
            foreach (int playerLostId in playerIdsWithMinTemp)
            {
                m_Players[playerLostId].HasLost = true;
                OnPlayerLost.Invoke(playerLostId);
            }
            playerIdsWithMinTemp.Clear();
            playerIdsWithMinTemp.Add(playerId);
            minTemperature = player.Temperature;
        };

        int playerId = -1;
        foreach (Player player in m_Players)
        {
            playerId++;

            if (player.HasLost)
                continue;

            if (player.Temperature < minTemperature)
                newMinScore(playerId, player);
            else if (player.Temperature == minTemperature)
            {
                if (player.PeopleSatistfaction > GetPlayerPeopleSatisfaction(playerIdsWithMinTemp[0]))
                    newMinScore(playerId, player);
                else if (player.PeopleSatistfaction == GetPlayerPeopleSatisfaction(playerIdsWithMinTemp[0]))
                    playerIdsWithMinTemp.Add(playerId);
                else
                {
                    player.HasLost = true;
                    OnPlayerLost.Invoke(playerId);
                }
            }
            else
            {
                player.HasLost = true;
                OnPlayerLost.Invoke(playerId);
            }
        }
    }

    // return if player has died in this turn
    // if game ends because there is no card anymore and the player lost,
    // true is still returned
    public bool Produce(int iPlayerIndex)
    {
        Assert.IsTrue(0 <= iPlayerIndex && iPlayerIndex < m_NbPlayer);
        Player player = m_Players[iPlayerIndex];
        if (player.HasLost)
            return false;

        _UpdatePlayerRessources(player);
        bool hasDied = _HasPlayerJustDied(player);
        if (hasDied)
            OnPlayerLost.Invoke(iPlayerIndex);
        else
            _CheckNbLeftCards(iPlayerIndex);
        Debug.Log(iPlayerIndex + " <index after producing : " + player.ToString());
        OnPlayerValuesUpdates.Invoke((iPlayerIndex, player));
        return hasDied;
    }

    public void DrawCardForPlayer(int iPlayerIndex)
    {
        Assert.IsTrue(0 <= iPlayerIndex && iPlayerIndex < m_NbPlayer);
        Player player = m_Players[iPlayerIndex];
        int nCardToDraw = player.NbOfCardToDraw();
        if (!player.HasStarted)
            nCardToDraw = 0;
        m_PlayersDeck[iPlayerIndex].DrawCardsWithoutReturn(nCardToDraw);
        player.HasStarted = true;
    }

    // return if player can afford the the card cost
    public bool AddCard(int boardID, Card iCard)
    {
        int cardID = iCard.PlayerID.AsInt;
        Assert.IsTrue(0 <= cardID && cardID < m_NbPlayer);
        Player playerOwningCard = m_Players[cardID];
        if (!playerOwningCard.CanAffordCard(iCard))
            return false;

        m_Players[boardID].CardOnBoard.Add(iCard);
        m_PlayersDeck[cardID].RemoveCardFromHand(iCard);
        playerOwningCard.Money -= iCard.CardData.Cost;
        OnPlayerValuesUpdates.Invoke((cardID, playerOwningCard));
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
