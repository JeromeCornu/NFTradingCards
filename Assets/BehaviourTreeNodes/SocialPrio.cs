using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialPrio : PillardCalculation
{
    [SerializeField]
    private float _percentTreshold = .08f;
    protected override string outputKey => DecidePillarWeight.socKey;
    public override void Awake()
    {
        base.Awake();
    }
    protected override float DeterminOutputPriority()
    {
        float maxPrio = 4;
        int currPrio = 0;
        float normalized = player.PeopleSatistfaction / 100f;
        const float tresh1 = 1f / GameSystem.Player.NbOfCardDrawTreshold;
        const float tresh2 = 2f * tresh1;
        if (normalized < (_game.MMinSatisfaction / 100f) + _percentTreshold)
            currPrio = 4;
        else if (normalized < tresh1 - _percentTreshold)
            currPrio = 2;
        else if (Mathf.Abs(normalized - tresh1) < _percentTreshold)
            currPrio = 3;
        else if (normalized < tresh2 - _percentTreshold)
            currPrio = 1;
        else if (Mathf.Abs(normalized - tresh2) < _percentTreshold)
            currPrio = 2;
        else if (normalized > tresh2 + _percentTreshold)
            currPrio = 0;
        //Debug.Log(normalized + " gave test value : " + currPrio);
        return currPrio / maxPrio;
    }
}
