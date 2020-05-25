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
    public class ApiService
    {
        public void HighscoresPut(Highscores highscores)
        {
            // Using Put rather than Post. Rest specification : -
            // 1. If the Request-URI refers to an already existing resource – an update operation will happen, otherwise create operation should happen if Request-URI is a valid resource URI
            // 2. PUT method is idempotent. So if you send retry a request multiple times, that should be equivalent to single request modification. In this case this is good because the first, i.e. highest score will be taken if multiple exist

            try
            {
                var uri = $"https://shelterbox-cbg1.firebaseio.com/";

                if (highscores?.highscoreEntryList != null)
                {
                    var ix = 1;

                    foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
                    {
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

        public Highscores HighscoresGet()
        {
            try
            {
                var uri = "https://shelterbox-cbg1.firebaseio.com/";

                var highscores = new Highscores();
                var highscoresEntryList = new List<HighscoreEntry>();

                RestClient.GetArray<HighscoreEntry>(uri + ".json").Then(response =>
                {
                    foreach (var highscoreEntry in response.Where(r => r.name != null))
                    {
                        //EditorUtility.DisplayDialog("Response", $"{highscoreEntry.name}: {highscoreEntry.score}", "Ok");
                        //Debug.Log($"HighscoresGet: {highscoreEntry.name} / {highscoreEntry.score}");
                        highscoresEntryList.Add(highscoreEntry);
                    }

                    highscores.highscoreEntryList = highscoresEntryList;
                    return highscores;
                });

                return highscores;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}