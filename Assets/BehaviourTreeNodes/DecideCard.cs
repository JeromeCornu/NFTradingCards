using BehaviourTreeNodes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniBT;
using UnityEngine;

public class DecideCard : UniBT.Action
{
    public const string aggroKey = "bAggro";
    private GameSystem _game;
    private GameSystem.Player _player;
    Dictionary<CardData.Pillar, float> dico;
    public override void Awake()
    {
        base.Awake();
    }
    protected override Status OnUpdate()
    {
        _game = BT_Blackboard.GameObjects?["Game"].GetComponent<GameSystem>();
        _player = _game?[1];

        if (_game == null || _player == null)
        {

            return Status.Running;
        }
        bool aggro = BT_Blackboard.Bools[aggroKey];
        var h = _game.getDeckBehaviour(1).MHand;
        dico = BT_Blackboard.Objects[DecidePillarWeight.weightsKey] as Dictionary<CardData.Pillar, float>;
        var seq = h.Where(c => _player.CanAffordCard(c)).Select(c => c.CardData);
        float topScore = aggro ? seq.Min((c) => PonderateSumCard(c)) : seq.Max((c) => PonderateSumCard(c));
        Card topCard = h.First(c => PonderateSumCard(c.CardData) == topScore);
        BT_Blackboard.GameObjects[PlaceCard.CtPkey] =
            topCard
            .gameObject;
        Debug.Log(topCard + " with pillars : " + dico + " <- top is, from : " + string.Join('\n', seq));
        return Status.Success;
    }
    private float PonderateSumCard(CardData c)
    {
        var sum = 0f;
        sum += PonderateVal(CardData.Pillar.Ecologic, c, dico);
        sum += PonderateVal(CardData.Pillar.Economic, c, dico);
        sum += PonderateVal(CardData.Pillar.Social, c, dico);
        return sum;
    }
    private float PonderateVal(CardData.Pillar pillar, CardData card, Dictionary<CardData.Pillar, float> dico)
    {
        return card[pillar].Val * dico[pillar];
    }
}
