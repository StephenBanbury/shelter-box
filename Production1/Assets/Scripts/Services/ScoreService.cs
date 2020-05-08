using System.Collections.Generic;
using System.Linq;
using Com.MachineApps.PrepareAndDeploy.Enums;
using Com.MachineApps.PrepareAndDeploy.Models;
using UnityEngine;


namespace Com.MachineApps.PrepareAndDeploy.Services
{
    public class ScoreService
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
            returnScoreDictionary.Add(ScoreType.OperationSuccessful, 1000);
            returnScoreDictionary.Add(ScoreType.ItemDropped, -50);
            returnScoreDictionary.Add(ScoreType.ItemNotRequired, -50);
            returnScoreDictionary.Add(ScoreType.ItemAlreadyAssigned, -50);
            returnScoreDictionary.Add(ScoreType.BalanceIntoRed, -100);
            returnScoreDictionary.Add(ScoreType.GameSuccessfullyCompleted, 2000);
            returnScoreDictionary.Add(ScoreType.OperationFailed, -500);

            return returnScoreDictionary;
        }

        public Highscores GetHighscores()
        {
            // Load saved Highscores
            string jsonString = PlayerPrefs.GetString("HighScoreTable");
            Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

            return highscores;
        }

        public HighscoreEntry GetHighScore()
        {
            var leaderBoard = GetHighscores();
            var highScore = leaderBoard?.highscoreEntryList.OrderByDescending(l => l.score).FirstOrDefault();
            return highScore;
        }

        public void AddHighscoreEntry(int score, string name)
        {
            Debug.Log($"AddHighscoreEntry: {score}, {name}");

            // Create HighscoreEntry
            HighscoreEntry highscoreEntry = new HighscoreEntry {score = score, name = name};

            // Load saved Highscores
            Highscores highscores = GetHighscores();

            string json = "Not initialised";

            if (highscores == null)
            {
                highscores = new Highscores();
                highscores.highscoreEntryList = new List<HighscoreEntry>();
            }

            // Add new entry to Highscores
            highscores.highscoreEntryList.Add(highscoreEntry);

            var highscoresSorted = highscores.highscoreEntryList.OrderByDescending(l => l.score);


            highscores.highscoreEntryList = highscoresSorted.Take(10).ToList();
            json = JsonUtility.ToJson(highscores);

            Debug.Log(json);

            PlayerPrefs.SetString("HighScoreTable", json);
            PlayerPrefs.Save();
        }

        public void ResetLeaderBoard()
        {
            PlayerPrefs.DeleteKey("HighScoreTable");
        }
    }
}