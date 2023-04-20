using System.Collections.Generic;
using UnityEngine;
using UniBT;
using System.Data.SqlTypes;

namespace BehaviourTreeNodes
{
    public class DefOrAggro : Action
    {
        [SerializeField] private int MoneyThresholdForDefenceMultiplier;
        private int MoneyThresholdForDefence;
        [SerializeField] private int TemperatureThresholdForDefence;
        [SerializeField] private int PplSatThresholdForDefence;


        private static GameSystem _gameSystem;
        public override void Awake()
        {
            base.Awake();
        }

        protected override Status OnUpdate()
        {

            _gameSystem = BT_Blackboard.GameObjects["Game"].GetComponent<GameSystem>();
            MoneyThresholdForDefence = (int)_gameSystem.getDeckBehaviour(1).CalculateAverageCost() * MoneyThresholdForDefenceMultiplier;
            if (_gameSystem == null) return Status.Failure;



            GameSystem.Player player = _gameSystem[1];
            Vector3 stats = _gameSystem.GetResourcesForNextRound(_gameSystem[1]);


            if (stats.x > MoneyThresholdForDefence && stats.y < TemperatureThresholdForDefence && stats.z > PplSatThresholdForDefence)
            {
                BT_Blackboard.Bools[DecideCard.aggroKey] = true;
            }
            else
            {
                BT_Blackboard.Bools[DecideCard.aggroKey] = false;
            }

            //Force ; BT_Blackboard.Bools["bAggro"] = true;
            return Status.Success;
        }
    }
}