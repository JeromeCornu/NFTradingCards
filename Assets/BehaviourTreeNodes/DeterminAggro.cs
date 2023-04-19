using UniBT;
using Action = UniBT.Action;
using Random = UnityEngine.Random;

namespace BehaviourTreeNodes
{
    public class DeterminAggro : Action
    {
        protected override Status OnUpdate()
        {
            if (BT_Blackboard.Floats == null) return Status.Failure;
            
            if (BT_Blackboard.Floats["Aggro"] == -1)
            {
                BT_Blackboard.Floats["Aggro"] = Random.Range(0.2f, 0.8f);
            }
            return Status.Success;
        }
    }
}
