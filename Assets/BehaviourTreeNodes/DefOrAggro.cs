using System.Collections.Generic;
using UniBT;

namespace BehaviourTreeNodes
{
    public class DefOrAggro : Action
    {
        
        private static GameSystem gameSystem;
        private int _playerCardNumber;
        private int _aiCardNumber;
        //Order in list : Money, Temp, Satisfacion;
        private List<int> _playerStats = new ();
        private List<int> _aiStats = new ();
        public const string StatKey = "StatList";
        


        protected override Status OnUpdate()
        {
            
            gameSystem ??= BT_Blackboard.GameObjects?["Game"].GetComponent<GameSystem>();
            if (gameSystem==null)return Status.Failure;
            
           
            _playerCardNumber = gameSystem.getDeckBehaviour(0).MHand.Count;
            _aiCardNumber = gameSystem.getDeckBehaviour(1).MHand.Count;
            
            _playerStats.Add(gameSystem[0].Money);
            _playerStats.Add(gameSystem[0].Temperature);
            _playerStats.Add(gameSystem[0].PeopleSatistfaction);
            _aiStats.Add(gameSystem[1].Money);
            _aiStats.Add(gameSystem[1].Temperature);
            _aiStats.Add(gameSystem[1].PeopleSatistfaction);

            float averagePlayerStat;
            float averageAiStat;

            float avrgMoney = (float)(_playerStats[0] - gameSystem.MStartMoney)/gameSystem.MStartMoney;
            float avrgTemp = (float)(_playerStats[1] - gameSystem.MStartTemp)/gameSystem.MStartTemp;
            float avrgPplSat = (float)(_playerStats[2] - gameSystem.MStartSatisfaction)/gameSystem.MStartSatisfaction;
            
            //(va-vi)/vi
            
            averagePlayerStat = (avrgMoney + avrgTemp + avrgPplSat)/_playerStats.Count;
            
            avrgMoney = (float)(_aiStats[0] - gameSystem.MStartMoney)/gameSystem.MStartMoney;
            avrgTemp = (float)(_aiStats[1] - gameSystem.MStartTemp)/gameSystem.MStartTemp;
            avrgPplSat = (float)(_aiStats[2] - gameSystem.MStartSatisfaction)/gameSystem.MStartSatisfaction;
           
            averageAiStat = (avrgMoney + avrgTemp + avrgPplSat)/_aiStats.Count;
            
            
            if (_aiCardNumber > _playerCardNumber && averageAiStat > averagePlayerStat)
            {
                BT_Blackboard.Bools["bAggro"] = true;
            }

            return Status.Success;
        }
    }
}