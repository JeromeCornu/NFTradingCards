using System.Collections.Generic;
using UnityEngine;
using UniBT;
using System.Data.SqlTypes;

namespace BehaviourTreeNodes
{
    public class DefOrAggro : Action
    {
        [SerializeField] private int MoneyThresholdForDefence;
        [SerializeField] private int TemperatureThresholdForDefence;
        [SerializeField] private int PplSatThresholdForDefence;


        private static GameSystem _gameSystem;
      

        protected override Status OnUpdate()
        {

            _gameSystem = BT_Blackboard.GameObjects["Game"].GetComponent<GameSystem>();
            if (_gameSystem == null) return Status.Failure;

            int Money = 0;
            int Temp = 0;
            int PplSat = 0;

            GameSystem.Player player = _gameSystem[1];

            foreach (Card card in player.CardOnBoard)
            {
                Money += card.CardData[CardData.Pillar.Economic].Val;
                Temp -= card.CardData[CardData.Pillar.Ecologic].Val;
                PplSat += card.CardData[CardData.Pillar.Social].Val;
            }

            if(Money > MoneyThresholdForDefence && Temp < TemperatureThresholdForDefence && PplSat > PplSatThresholdForDefence) 
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