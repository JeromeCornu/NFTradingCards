using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcologicPrio : PillardCalculation
{

    protected override string outputKey => DecidePillarWeight.ecoLKey;

    protected override float DeterminOutputPriority()
    {
        return Mathf.InverseLerp(_game.MStartTemp, _game.MMaxTemperature,player.Temperature);
    }
}
