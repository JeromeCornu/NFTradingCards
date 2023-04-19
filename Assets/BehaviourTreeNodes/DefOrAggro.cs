using System.Collections.Generic;
using UniBT;

namespace BehaviourTreeNodes
{
    public class DefOrAggro : Action
    {
        
        private static GameSystem _gameSystem;
        private int _playerCardNumber;
        private int _aiCardNumber;
        //Order in list : Money, Temp, Satisfacion;
        private List<int> _playerStats = new ();
        private List<int> _aiStats = new ();
        public const string StatKey = "StatList";
        


        protected override Status OnUpdate()
        {
            
            _gameSystem ??= BT_Blackboard.GameObjects?["Game"].GetComponent<GameSystem>();
            if (_gameSystem==null)return Status.Failure;
            
           
            _playerCardNumber = _gameSystem.getDeckBehaviour(0).MHand.Count;
            _aiCardNumber = _gameSystem.getDeckBehaviour(1).MHand.Count;
            
            _playerStats.Add(_gameSystem[0].Money);
            _playerStats.Add(_gameSystem[0].Temperature);
            _playerStats.Add(_gameSystem[0].PeopleSatistfaction);
            _aiStats.Add(_gameSystem[1].Money);
            _aiStats.Add(_gameSystem[1].Temperature);
            _aiStats.Add(_gameSystem[1].PeopleSatistfaction);

            float averagePlayerStat;
            float averageAiStat;

            float avrgMoney = (float)(_playerStats[0] - _gameSystem.MStartMoney)/_gameSystem.MStartMoney;
            float avrgTemp = (float)(_playerStats[1] - _gameSystem.MStartTemp)/_gameSystem.MStartTemp;
            float avrgPplSat = (float)(_playerStats[2] - _gameSystem.MStartSatisfaction)/_gameSystem.MStartSatisfaction;
            
            //(va-vi)/vi
            
            averagePlayerStat = (avrgMoney + avrgTemp + avrgPplSat)/_playerStats.Count;
            
            avrgMoney = (float)(_aiStats[0] - _gameSystem.MStartMoney)/_gameSystem.MStartMoney;
            avrgTemp = (float)(_aiStats[1] - _gameSystem.MStartTemp)/_gameSystem.MStartTemp;
            avrgPplSat = (float)(_aiStats[2] - _gameSystem.MStartSatisfaction)/_gameSystem.MStartSatisfaction;
           
            averageAiStat = (avrgMoney + avrgTemp + avrgPplSat)/_aiStats.Count;
            
            
            if (_aiCardNumber > _playerCardNumber && averageAiStat > averagePlayerStat)
            {
                BT_Blackboard.Bools["bAggro"] = true;
            }

            return Status.Success;
        }
    }
}