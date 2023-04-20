using System.Collections;
using System.Collections.Generic;
using UniBT;
using UnityEngine;
using System.Linq;
using static CardData;

public class DecidePillarWeight : Action
{
    public const string weightsKey = "Weights";
    public const string socKey = "SocialP";
    public const string econKey = "EconomicP";
    public const string ecoLKey = "EcologicP";
    Dictionary<Pillar, float> weights;
    public override void Awake()
    {
        base.Awake();
        weights = new Dictionary<Pillar, float>();
        Fill(0, 0, 0);
    }
    protected override Status OnUpdate()
    {
        Fill(BT_Blackboard.Floats[ecoLKey], BT_Blackboard.Floats[econKey], BT_Blackboard.Floats[socKey]);
        BT_Blackboard.Objects[weightsKey] = weights;
        return Status.Success;
    }
    private void Fill(float ecolo, float econo, float social)
    {
        var sum = ecolo + econo + social;
        weights[Pillar.Economic] = econo / sum;
        weights[Pillar.Ecologic] = ecolo / sum;
        weights[Pillar.Social] = social / sum;
    }
}
