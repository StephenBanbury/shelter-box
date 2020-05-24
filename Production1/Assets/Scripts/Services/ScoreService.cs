using Com.MachineApps.PrepareAndDeploy.Enums;
using Com.MachineApps.PrepareAndDeploy.Models;
using Proyecto26;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
            var returnScoreDictionary = new Dictionary<ScoreType, int>
            {
                {ScoreType.ItemAssigned, 200},
                {ScoreType.OperationSuccessful, 1000},
                {ScoreType.ItemDropped, -50},
                {ScoreType.ItemNotRequired, -50},
                {ScoreType.ItemAlreadyAssigned, -50},
                {ScoreType.GameSuccessfullyCompleted, 2000},
                {ScoreType.OperationFailed, -500}
            };

            return returnScoreDictionary;
        }

        public Highscores GetHighscoresSorted()
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
            var highscores = GetHighscoresSorted();
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
            HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

            // Load saved Highscores
            Highscores highscores = GetHighscoresSorted();

            // In case we don't have a list yet
            if (highscores == null)
            {
                highscores = new Highscores { highscoreEntryList = new List<HighscoreEntry>() };
            }

            // Add new entry to Highscores
            highscores.highscoreEntryList.Add(highscoreEntry);

            highscores = SortHighscores(highscores);

            // TODO To consider: do we want to limit the saved highscores? 
            highscores.highscoreEntryList = highscores.highscoreEntryList.Take(numberToFetch).ToList();

            var json = JsonUtility.ToJson(highscores);

            //Debug.Log(json);

            // Save to PlayerPrefs
            PlayerPrefs.SetString("HighScoreTable", json);
            PlayerPrefs.Save();

            // Save to database
            HighscoresPut();
        }

        public void ResetLeaderBoard()
        {
            PlayerPrefs.DeleteKey("HighScoreTable");
            GetHighscoresSorted();
        }

        #region API operations

        public void HighscoresPut()
        {
            // Using Put rather than Post. Rest specification : -
            // 1. If the Request-URI refers to an already existing resource – an update operation will happen, otherwise create operation should happen if Request-URI is a valid resource URI
            // 2. PUT method is idempotent. So if you send retry a request multiple times, that should be equivalent to single request modification. In this case this is good because the first, i.e. highest score will be taken if multiple exist

            try
            {
                var highscores = GetHighscoresSorted();
                var uri = $"https://shelterbox-cbg1.firebaseio.com/";

                if (highscores?.highscoreEntryList != null)
                {
                    var ix = 1;

                    foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
                    {
                        if (highscoreEntry.name == "jck") highscoreEntry.name = "jack";
                        if (highscoreEntry.name == "ill") highscoreEntry.name = "bill";

                        RestClient.Put(uri + $"/{ix}.json", JsonUtility.ToJson(highscoreEntry)).Then(response =>
                        {
                            Debug.Log($"HighscoresPut: {highscoreEntry.name} / {highscoreEntry.score}");
                        });

                        ix++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void HighscoresGet()
        {
            try
            {
                var uri = "https://zzshelterbox-cbg1.firebaseio.com/";

                RestClient.GetArray<HighscoreEntry>(uri + ".json").Then(response =>
                {
                    foreach (var highscoreEntry in response.Where(r => r.name != null))
                    {
                        EditorUtility.DisplayDialog("Response", $"{highscoreEntry.name}: {highscoreEntry.score}", "Ok");
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}