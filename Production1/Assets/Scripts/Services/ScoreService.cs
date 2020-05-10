using System.Collections.Generic;
using System.Linq;
using Com.MachineApps.PrepareAndDeploy.Enums;
using Com.MachineApps.PrepareAndDeploy.Models;
using UnityEngine;


namespace Com.MachineApps.PrepareAndDeploy.Services
{
    public class ScoreService
    {
        private readonly int numberToFetch;

        public ScoreService(int numberInTable = -1)
        {
            numberToFetch = numberInTable;
        }

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

            if (highscores != null)
            {
                // Sort list by score
                highscores = SortHighscores(highscores);

                // Take top x
                if (numberToFetch != -1)
                {
                    highscores.highscoreEntryList = highscores.highscoreEntryList.Take(numberToFetch).ToList();
                }
            }
            return highscores;
        }

        private Highscores SortHighscores(Highscores highscores)
        {
            highscores.highscoreEntryList.Sort((x, y) => y.score.CompareTo(x.score));
            return highscores;
        }

        public HighscoreEntry GetTopHighScore()
        {
            HighscoreEntry highscore = new HighscoreEntry();
            var highscores = GetHighscores();
            if (highscores != null)
            {
                highscores = SortHighscores(highscores);
                highscore = highscores?.highscoreEntryList.FirstOrDefault();
            }
            return highscore;
        }

        public void AddHighscoreEntry(int score, string name)
        {
            Debug.Log($"AddHighscoreEntry: {score}, {name}");

            // Create HighscoreEntry
            HighscoreEntry highscoreEntry = new HighscoreEntry {score = score, name = name};

            // Load saved Highscores
            Highscores highscores = GetHighscores();

            //string json = "Not initialised";

            // In case we don't have a list yet
            if (highscores == null)
            {
                highscores = new Highscores {highscoreEntryList = new List<HighscoreEntry>()};
            }

            // Add new entry to Highscores
            highscores.highscoreEntryList.Add(highscoreEntry);

            highscores = SortHighscores(highscores);

            // TODO To consider: do we want to limit the saved highscores? 
            highscores.highscoreEntryList = highscores.highscoreEntryList.Take(numberToFetch).ToList();

            var json = JsonUtility.ToJson(highscores);

            Debug.Log(json);

            PlayerPrefs.SetString("HighScoreTable", json);
            PlayerPrefs.Save();
        }

        public void ResetLeaderBoard()
        {
            PlayerPrefs.DeleteKey("HighScoreTable");
            GetHighscores();
        }
    }
}