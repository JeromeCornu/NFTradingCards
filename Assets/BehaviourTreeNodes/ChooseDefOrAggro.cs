using System.Collections.Generic;
using UnityEngine;
using UniBT;
using System.Data.SqlTypes;
using Random = UnityEngine.Random;

namespace BehaviourTreeNodes
{
    public class ChooseDefOrAggro : Action
    {
        [SerializeField] private float MoneyThresholdForDefenceMultiplier;
        private float MoneyThresholdForDefence;
        [SerializeField] private int TemperatureThresholdForDefence;
        [SerializeField] private int PplSatThresholdForDefence;

        private int DefCardScore = 0;
        private int OffCardScore = 0;


        private static GameSystem _gameSystem;

        protected override Status OnUpdate()
        {
            DefCardScore = 0;
            OffCardScore = 0;

            Card _defCard = BT_Blackboard.GameObjects[PlaceCard.CtPDefkey]?.GetComponent<Card>();
            Card _offCard = BT_Blackboard.GameObjects[PlaceCard.CtPOffkey]?.GetComponent<Card>();

            _gameSystem = BT_Blackboard.GameObjects["Game"].GetComponent<GameSystem>();
            if (_gameSystem == null) return Status.Failure;

            MoneyThresholdForDefence = _gameSystem.getDeckBehaviour(1).CalculateAverageCost() *
                                       MoneyThresholdForDefenceMultiplier;
            

            Card cardToPlay = MesureImpact(_defCard, _offCard);


            GameSystem.Player player = _gameSystem[1];
            Vector3 stats = _gameSystem.GetResourcesForNextRound(player);


            if (stats.x > MoneyThresholdForDefence && stats.y < TemperatureThresholdForDefence &&
                stats.z > PplSatThresholdForDefence)
            {
                cardToPlay = _defCard;
                BT_Blackboard.Bools[PillardCalculation.AggroKey] = false;
            }


            BT_Blackboard.GameObjects[PlaceCard.CtPkey] = cardToPlay.gameObject;
            
            Debug.Log(_defCard?.CardData.name +  " | " + _offCard?.CardData.name);

            //Force ; BT_Blackboard.Bools["bAggro"] = true;
            return Status.Success;
        }

        private Card MesureImpact(Card defCardCardData, Card offCardCardData)
        {
            bool def = false;

            if (defCardCardData == null && offCardCardData == null)
            {
                return null;
            }
            
            if (defCardCardData == null)
            {
                BT_Blackboard.Bools[PillardCalculation.AggroKey] = true;
                return offCardCardData;
            }

            if (offCardCardData == null)
            {
                BT_Blackboard.Bools[PillardCalculation.AggroKey] = false;
                return defCardCardData;
            }
            
            if (defCardCardData.CardData.Cost > offCardCardData.CardData.Cost)
            {
                BT_Blackboard.Bools[PillardCalculation.AggroKey] = false;
                return defCardCardData;
                
            }
            if (defCardCardData.CardData.Cost < offCardCardData.CardData.Cost)
            {
                BT_Blackboard.Bools[PillardCalculation.AggroKey] = true;
                return offCardCardData;
            }
            
            def = Random.value >= BT_Blackboard.Floats["Aggro"];

            if (def)
            {
                BT_Blackboard.Bools[PillardCalculation.AggroKey] = false;
                return defCardCardData;
            }

            BT_Blackboard.Bools[PillardCalculation.AggroKey] = true;
            return offCardCardData;

        }
    }
}