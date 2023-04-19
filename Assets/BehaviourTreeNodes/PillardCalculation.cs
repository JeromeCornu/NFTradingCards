using System.Collections;
using System.Collections.Generic;
using UniBT;
using UnityEngine;
using System.Linq;
using static CardData;

public abstract class PillardCalculation : Action
{
    private bool _aggro;
    protected const string AggroKey = "bAggro";
    protected GameSystem _game { get; private set; }
    protected GameSystem.Player player { get; private set; }
    protected abstract string outputKey { get; }
    public override void Awake()
    {
        base.Awake();
    }
    protected override Status OnUpdate()
    {
        _aggro = BT_Blackboard.Bools[AggroKey];
        _game = BT_Blackboard.GameObjects?["Game"].GetComponent<GameSystem>();
        player = _game[PlayerIndex()];
        BT_Blackboard.Floats[outputKey] = DeterminOutputPriority();
        return Status.Success;
    }

    protected int PlayerIndex()
    {
        return _aggro ? 0 : 1;
    }

    protected abstract float DeterminOutputPriority();
}
