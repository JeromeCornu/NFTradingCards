using UniBT;
using UnityEngine;

namespace BehaviourTreeNodes
{
    public class PlaceCard : Action
    {
        private SelectableCard _card;
        public const string CtPkey = "CardToPlay";

        protected override Status OnUpdate()
        {

            _card = BT_Blackboard.GameObjects[CtPkey]?.GetComponent<SelectableCard>();
            //Card is null means we skip (and hopefully we didn't jus loose ref hihi)
            if (_card == null)
            {
                return Status.Failure;
            }

            if (BT_Blackboard.Bools["bAggro"])
            {
                if (!BT_Blackboard.GameObjects["PlayerZone"].GetComponent<CardZone>().AddCard(_card))
                    return Status.Running;

            }
            else
            {
                if (!BT_Blackboard.GameObjects["AiZone"].GetComponent<CardZone>().AddCard(_card))
                    return Status.Running;
            }

            Debug.Log("CardPlaced");
            //Debug.Log(BT_Blackboard.GameObjects?["Game"].GetComponent<GameSystem>().getDeckBehaviour(1).MHand.Count);

            return Status.Success;
        }
    }
}