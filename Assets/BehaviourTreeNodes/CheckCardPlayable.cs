using System.Collections;
using System.Collections.Generic;
using UniBT;
using UnityEngine;
using System.Linq;

public class CheckCardPlayable : Action
{
    private GameSystem _game;
    private GameSystem.Player _player;
    public override void Awake()
    {
        base.Awake();
    }
    protected override Status OnUpdate()
    {
        if (_game == null)
        {
            _game = BT_Blackboard.GameObjects?["Game"].GetComponent<GameSystem>();
            _player = _game[1];
            return Status.Failure;
        }
        if (_game.getDeckBehaviour(1).MHand.Any((c) => _player.CanAffordCard(c)))
        {
            return Status.Success;
        }
        return Status.Failure;
    }

}
