using System.Collections.Generic;
using UniBT;
using UnityEngine;

namespace BehaviourTreeNodes
{
    public class PlaceCard : Action
    {
        private SelectableCard _card;
        public const string CtPDefkey = "CardToPlayDef";
        public const string CtPOffkey = "CardToPlayOff";
        public const string CtPkey = "CardToPlay";
        private const Status ErrorStatus = Status.Failure; //for go next turn but not ruin exe Status.Running;//For debug, and 

        protected override Status OnUpdate()
        {

            _card = BT_Blackboard.GameObjects[CtPkey]?.GetComponent<SelectableCard>();
            //Card is null means we skip (and hopefully we didn't jus loose ref hihi)
            if (_card == null)
            {
                return Status.Failure;
            }

            if (BT_Blackboard.Bools[PillardCalculation.AggroKey])
            {
                if (!BT_Blackboard.GameObjects["PlayerZone"].GetComponent<CardZone>().AddCard(_card))
                {
                    var score = DecideCard.PonderateSumCard(_card.GetComponent<Card>().CardData, BT_Blackboard.Objects[DecidePillarWeight.weightsKey] as Dictionary<CardData.Pillar, float>);
                    Debug.Log(_card.GetComponent<Card>() + "Error card with score ; " + score);
                    return ErrorStatus;
                }

            }
            else
            {
                if (!BT_Blackboard.GameObjects["AiZone"].GetComponent<CardZone>().AddCard(_card))
                {
                    var score = DecideCard.PonderateSumCard(_card.GetComponent<Card>().CardData, BT_Blackboard.Objects[DecidePillarWeight.weightsKey] as Dictionary<CardData.Pillar, float>);
                    Debug.Log(_card.GetComponent<Card>() + "Error card with score ; " + score);
                    return ErrorStatus;
                }
            }

            //Debug.Log("CardPlaced");
            //Debug.Log(BT_Blackboard.GameObjects?["Game"].GetComponent<GameSystem>().getDeckBehaviour(1).MHand.Count);

            return Status.Success;
        }
    }
}