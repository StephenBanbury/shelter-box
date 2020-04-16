using System.Collections.Generic;
using Com.MachineApps.PrepareAndDeploy.Enums;
using UnityEngine;


namespace Com.MachineApps.PrepareAndDeploy.Services
{
    public class ScoreService : MonoBehaviour
    {
        public int GetScoreValue(ScoreType scoreType)
        {
            var scoreValues = ScoreValue();
            var scoreValue = scoreValues[scoreType];
            return scoreValue;
        }

        private Dictionary<ScoreType, int> ScoreValue()
        {
            var returnScoreDictionary = new Dictionary<ScoreType, int>();

            returnScoreDictionary.Add(ScoreType.ItemAssigned, 200);
            returnScoreDictionary.Add(ScoreType.DeploymentCompleted, 1000);
            returnScoreDictionary.Add(ScoreType.ItemDropped, -50);
            returnScoreDictionary.Add(ScoreType.ItemNotRequired, -50);
            returnScoreDictionary.Add(ScoreType.ItemAlreadyAssigned, -50);
            returnScoreDictionary.Add(ScoreType.BalanceIntoRed, -100);

            return returnScoreDictionary;
        }
    }
}