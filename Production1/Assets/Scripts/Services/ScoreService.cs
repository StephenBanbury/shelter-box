using Com.MachineApps.PrepareAndDeploy.Enums;
using Com.MachineApps.PrepareAndDeploy.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Com.MachineApps.PrepareAndDeploy.Services
{
    public class ScoreService
    {
        private readonly int numberToFetch;

        private Highscores highscoresFromApi;
        private Highscores highscoresSorted;


        public ScoreService(int numberInHighscoresTable = -1)
        {
            numberToFetch = numberInHighscoresTable;
        }

        public Highscores HighscoresSorted => highscoresSorted;

        #region Public Methods

        public int GetScoreValue(ScoreType scoreType)
        {
            var scoreValues = ScoreValue();
            var scoreValue = scoreValues[scoreType];
            return scoreValue;
        }

        public  Highscores GetHighscoresSorted(bool useApi)
        {
            // Highscores from Firebase will have already been loaded into highscoresFromApi

            // Load saved Highscores from PlayerPrefs
            var highscoresFromPlayerPrefs = GetHighscoresFromPlayerPrefs();
            //});

            IEnumerable<HighscoreEntry> combined = new List<HighscoreEntry>();
            if (useApi)
            {
                //Combine into single list
                combined =
                    highscoresFromPlayerPrefs.highscoreEntryList
                        .Union(highscoresFromApi.highscoreEntryList);
            }
            else
            {
                combined = highscoresFromPlayerPrefs.highscoreEntryList;
            }

            // De-dup - Where there are duplicate players, take highest score for each duplicate
            // If there is a tie this should still take a single entry
            var deDuped = (
                    from c in combined
                    group c by c.name
                    into grp
                    select grp.OrderByDescending(c => c.score).FirstOrDefault())
                .OrderByDescending(h => h.score)
                .ThenBy(h => h.name);
            
            highscoresSorted = new Highscores { highscoreEntryList = deDuped.Take(numberToFetch).ToList() };

            return highscoresSorted;
        }

        public HighscoreEntry GetTopHighScore()
        {
            HighscoreEntry highscore = new HighscoreEntry();

            var highscores = highscoresSorted;
            if (highscores != null)
            {
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
            //Highscores highscores = GetHighscoresSorted();
            var highscores = highscoresSorted;
            Debug.Log($"highscoresSorted: {highscores}");

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

            // Save to PlayerPrefs
            var json = JsonUtility.ToJson(highscores);
            PlayerPrefs.SetString("HighScoreTable", json);
            PlayerPrefs.Save();

            // Save to database
            var apiService = new ApiService();
            apiService.HighscoresPut(highscores);
        }

        public void ResetLeaderBoard()
        {
            PlayerPrefs.DeleteKey("HighScoreTable");
            GetHighscoresSorted(false);
        }



        public IEnumerator GetHighscoresFromApi()
        {
            Debug.Log("GetHighscoresFromApi");

            var apiService = new ApiService();
            highscoresFromApi = apiService.HighscoresGet();
           
            yield return new WaitUntil(() => highscoresFromApi != null && highscoresFromApi.highscoreEntryList != null);


            Debug.Log($"GetHighscoresFromApi - done: {highscoresFromApi.highscoreEntryList.Count}");
        }

        #endregion


        /*-----------------------------------------------------------------------------------------*/


        #region Private methods

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

        private Highscores GetHighscoresFromPlayerPrefs()
        {
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

        #endregion
    }
}