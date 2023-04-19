using UniBT;
using UnityEngine;

namespace BehaviourTreeNodes
{
    public class PlaceCard : Action
    {
        private SelectableCard _card;
        
        protected override Status OnUpdate()
        {

            _card ??= BT_Blackboard.GameObjects?["CardToPlay"].GetComponent<SelectableCard>();
            if (_card == null)
            {
                return Status.Running;
            }

            if (BT_Blackboard.Bools["bAggro"])
            {
                BT_Blackboard.GameObjects?["PlayerZone"].GetComponent<CardZone>().AddCard(_card);
            }
            else
            {
                BT_Blackboard.GameObjects?["AiZone"].GetComponent<CardZone>().AddCard(_card);
            }

            Debug.Log("CardPlaced");
            Debug.Log(BT_Blackboard.GameObjects?["Game"].GetComponent<GameSystem>().getDeckBehaviour(1).MHand.Count);

            return Status.Success;
        }
    }
}