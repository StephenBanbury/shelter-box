using Com.MachineApps.PrepareAndDeploy.Enums;
using Com.MachineApps.PrepareAndDeploy.Models;
using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace Com.MachineApps.PrepareAndDeploy.Services
{
    public class ScoreService
    {
        private readonly int numberToFetch;

        private Highscores highscoresFromPlayerPrefs;
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

        //public Highscores GetHighscoresSorted()
        //{
        //    // Load saved Highscores
        //    string jsonString = PlayerPrefs.GetString("HighScoreTable");
        //    Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //    if (highscores != null)
        //    {
        //        // Sort list by score
        //        highscores = SortHighscores(highscores);

        //        // Take top x
        //        if (numberToFetch != -1)
        //        {
        //            highscores.highscoreEntryList = highscores.highscoreEntryList.Take(numberToFetch).ToList();
        //        }
        //    }
        //    return highscores;
        //}

        public  Highscores GetHighscoresSorted(bool useApi)
        {
            // Highscores from Firebase will have already been loaded into highscoresFromApi

            // Load saved Highscores from PlayerPrefs
            var highscoresFromPlayerPrefs = GetHighscoresFromPlayerPrefs();

            //highscoresFromPlayerPrefs.highscoreEntryList.ForEach(h =>
            //{
            //    Debug.Log($"highscoresFromPlayerPrefs: {h.name} / {h.score}");
            //});
            //highscoresFromApi.highscoreEntryList.ForEach(h =>
            //{
            //    Debug.Log($"highscoresFromApi: {h.name} / {h.score}");
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
            var deduped = (
                    from c in combined
                    group c by c.name
                    into grp
                    select grp.OrderByDescending(c => c.score).FirstOrDefault())
                .OrderByDescending(h => h.score)
                .ThenBy(h => h.name);

            //Debug.Log($"Combined and de-duped: {deduped.ToList().Count}");

            deduped.ToList().ForEach(h =>
            {
                Debug.Log($"highscoresSorted: {h.name} / {h.score}");
            });

            highscoresSorted = new Highscores { highscoreEntryList = deduped.Take(numberToFetch).ToList() };

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

        //#region API operations

        //public void HighscoresPut()
        //{
        //    // Using Put rather than Post. Rest specification : -
        //    // 1. If the Request-URI refers to an already existing resource – an update operation will happen, otherwise create operation should happen if Request-URI is a valid resource URI
        //    // 2. PUT method is idempotent. So if you send retry a request multiple times, that should be equivalent to single request modification. In this case this is good because the first, i.e. highest score will be taken if multiple exist

        //    try
        //    {
        //        var highscores = GetHighscoresSorted();
        //        var uri = $"https://shelterbox-cbg1.firebaseio.com/";

        //        if (highscores?.highscoreEntryList != null)
        //        {
        //            var ix = 1;

        //            foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        //            {
        //                RestClient.Put(uri + $"/{ix}.json", JsonUtility.ToJson(highscoreEntry)).Then(response =>
        //                {
        //                    Debug.Log($"HighscoresPut: {highscoreEntry.name} / {highscoreEntry.score}");
        //                });

        //                ix++;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void HighscoresGet()
        //{
        //    try
        //    {
        //        var uri = "https://shelterbox-cbg1.firebaseio.com/";

        //        RestClient.GetArray<HighscoreEntry>(uri + ".json").Then(response =>
        //        {
        //            foreach (var highscoreEntry in response.Where(r => r.name != null))
        //            {
        //                EditorUtility.DisplayDialog("Response", $"{highscoreEntry.name}: {highscoreEntry.score}", "Ok");
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //#endregion

    }
}