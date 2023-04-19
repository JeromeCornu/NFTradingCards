using System.Collections;
using System.Collections.Generic;
using UniBT;
using UnityEngine;
using System.Linq;
using static CardData;

public class DecidePillarWeight : Action
{
    private bool _aggro;
    private string key => "bAggro";
    private string outputKey = "Weights";
    Dictionary<Pillar, float> weights;
    public override void Awake()
    {
        base.Awake();
        weights = new Dictionary<Pillar, float>();
        Fill(0, 0, 0);
    }
    protected override Status OnUpdate()
    {
        _aggro = BT_Blackboard.Bools[key];
        GameSystem.Player player = BT_Blackboard.GameObjects?["Game"].GetComponent<GameSystem>()[_aggro ? 0 : 1];
        BT_Blackboard.Objects[outputKey] = weights;
        return Status.Success;
    }
    private void Fill(float ecolo, float econo, float social)
    {
        weights[Pillar.Economic] = econo;
        weights[Pillar.Ecologic] = ecolo;
        weights[Pillar.Social] = social;
    }
}
