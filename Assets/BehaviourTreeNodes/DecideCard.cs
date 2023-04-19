using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniBT;
using UnityEngine;

public class DecideCard : UniBT.Action
{
    public const string aggroKey = "bAggro";
    public const string cardKey = "CardToPlay";
    private GameSystem _game;
    private GameSystem.Player _player;
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
        var dico = BT_Blackboard.Objects[DecidePillarWeight.weightsKey] as Dictionary<CardData.Pillar, float>;
        var seq = h.Select(c => c.CardData);
        Func<CardData, float> scoreSelector = c =>
        {
            var sum = 0f;
            sum += Ponderate(CardData.Pillar.Ecologic, c, dico);
            sum += Ponderate(CardData.Pillar.Economic, c, dico);
            sum += Ponderate(CardData.Pillar.Social, c, dico);
            return sum;
        };
        var topC = aggro ? seq.Min(scoreSelector) : seq.Max(scoreSelector);
        return Status.Success;
    }
    private float Ponderate(CardData.Pillar pillar, CardData card, Dictionary<CardData.Pillar, float> dico)
    {
        return card[pillar].Val * dico[pillar];
    }
}
