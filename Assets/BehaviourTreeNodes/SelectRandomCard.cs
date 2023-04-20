using UniBT;
using Random = UnityEngine.Random;

namespace BehaviourTreeNodes
{
    public class SelectRandomCard : Action
    {
        private GameSystem _gameSystem;
        
        protected override Status OnUpdate()
        {
            _gameSystem ??= BT_Blackboard.GameObjects?["Game"].GetComponent<GameSystem>();
            if (_gameSystem==null)return Status.Running;

            Card card = _gameSystem.getDeckBehaviour(1)
                .MHand[Random.Range(0,_gameSystem.getDeckBehaviour(1).MHand.Count)];

            BT_Blackboard.GameObjects[PlaceCard.CtPkey] = card.gameObject;

            return Status.Success;
        }
    }
}