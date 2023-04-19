using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EconomicPrio : PillardCalculation
{
    [SerializeField, Label("MaxNbOfPlayableCardPerTurnToConsiderTooMuchMoney")]
    private int _nbOfCard = 6;

    protected override string outputKey => DecidePillarWeight.econKey;

    protected override float DeterminOutputPriority()
    {
        //We base ourselve on the AI dekc always, not considering which we are targetting, considering we know our deck but not the opponent ones, we still consider it will have same average
        var avgCost = _game.getDeckBehaviour(1).CalculateAverageCost();
        var currentMoney = player.Money;
        int playableCard = (int)(currentMoney / avgCost);
        return playableCard / (float)_nbOfCard;
    }
}
