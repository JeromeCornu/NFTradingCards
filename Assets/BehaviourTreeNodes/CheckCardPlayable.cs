using System.Collections;
using System.Collections.Generic;
using UniBT;
using UnityEngine;
using System.Linq;
using Debug = System.Diagnostics.Debug;

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
        _game = BT_Blackboard.GameObjects?["Game"].GetComponent<GameSystem>();
        _player = _game?[1];
        
        if (_game == null || _player == null)
        {

            return Status.Running;
        }
        
        if (_game.getDeckBehaviour(1).MHand.Any((c) => _player.CanAffordCard(c)))
        {
            return Status.Success;
        }
        return Status.Failure;
    }

}
