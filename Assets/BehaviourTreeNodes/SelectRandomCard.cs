using UniBT;
using Random = UnityEngine.Random;
using System.Linq;

namespace BehaviourTreeNodes
{
    public class SelectRandomCard : Action
    {
        private GameSystem _gameSystem;

        protected override Status OnUpdate()
        {
            _gameSystem ??= BT_Blackboard.GameObjects?["Game"].GetComponent<GameSystem>();
            if (_gameSystem == null) return Status.Running;

            var playables = _gameSystem.getDeckBehaviour(1).MHand.Where(c => _gameSystem[1].CanAffordCard(c)).ToList();
            Card card = playables[Random.Range(0, playables.Count)];

            BT_Blackboard.GameObjects[PlaceCard.CtPkey] = card.gameObject;

            return Status.Success;
        }
    }
}