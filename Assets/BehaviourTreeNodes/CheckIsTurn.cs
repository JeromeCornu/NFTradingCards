using System.Collections;
using System.Collections.Generic;
using UniBT;
using UnityEngine;

public class CheckIsTurn : Action
{
    private GameSystem _game;
    public override void Awake()
    {
        base.Awake();
    }
    protected override Status OnUpdate()
    {
        _game ??= BT_Blackboard.GameObjects?["Game"].GetComponent<GameSystem>();
        if (_game == null)
        {
            return Status.Running;
        }

        if (_game.TurnManager.IsPlayerTurn)
        {
            return Status.Failure;
        }
        return Status.Success;
    }

}
