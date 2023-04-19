using UniBT;

namespace BehaviourTreeNodes
{
    public class EndTurn : Action
    {

        private TurnManager _turnManager;

        protected override Status OnUpdate()
        {
            _turnManager ??= BT_Blackboard.GameObjects?["Game"].GetComponent<TurnManager>();
            if (_turnManager == null)
            {
                return Status.Failure;
            }

            _turnManager.SwitchTurn();
            return Status.Success;
        }

    }
}