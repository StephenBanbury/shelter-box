using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace Com.MachineApps.PrepareAndDeploy
{
    // Reference: -
    // https://www.youtube.com/watch?time_continue=1&v=iAbaqGYdnyI&feature=emb_logo

    public class HighscoreTable : MonoBehaviour
    {
        private Transform entryContainer;
        private Transform entryTemplate;
        private List<Transform> highscoreEntryTransformList;

        public void seed()
        {
            AddHighscoreEntry(100, "ABC");
        }

        private void Awake()
        {
            entryContainer = transform.Find("highscoreEntryContainer");
            entryTemplate = entryContainer.Find("highscoreEntryTemplate");

            entryTemplate.gameObject.SetActive(false);

            seed();

            string jsonString = PlayerPrefs.GetString("HighScoreTable");
            Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

            // Sort list by score
            highscores.highscoreEntryList.Sort((x, y) => y.score.CompareTo(x.score));

            // Take top 10
            highscores.highscoreEntryList = highscores.highscoreEntryList.Take(10).ToList();

            highscoreEntryTransformList = new List<Transform>();
            foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
            {
                CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
            }
        }

        private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
        {
            float templateHeight = 0.1f;
            Transform entryTransform = Instantiate(entryTemplate, container);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
            entryTransform.gameObject.SetActive(true);

            int rank = transformList.Count + 1;
            string rankString;
            switch (rank)
            {
                default:
                    rankString = rank + "TH"; break;

                case 1: rankString = "1ST"; break;
                case 2: rankString = "2ND"; break;
                case 3: rankString = "3RD"; break;
            }

            entryTransform.Find("posText").GetComponent<Text>().text = rankString;

            int score = highscoreEntry.score;
            entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

            string name = highscoreEntry.name;
            entryTransform.Find("nameText").GetComponent<Text>().text = name;

            // Set background visible if odds and evens
            entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);

            // Highlight 1ST
            if (rank == 1)
            {
                entryTransform.Find("posText").GetComponent<Text>().color = Color.green;
                entryTransform.Find("scoreText").GetComponent<Text>().color = Color.green;
                entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;
            }

            // Set trophy
            //switch (rank)
            //{
            //    default:
            //        entryTransform.Find("trophy").gameObject.SetActive(false);
            //        break;
            //    case 1:
            //        entryTransform.Find("trophy").GetComponent<Image>().color = new Color(255,210,0,255);
            //        break;
            //    case 2:
            //        entryTransform.Find("trophy").GetComponent<Image>().color = new Color(198,198,198,255);
            //        break;
            //    case 3:
            //        entryTransform.Find("trophy").GetComponent<Image>().color = new Color(183,111,86,255);
            //        break;

            //}

            transformList.Add(entryTransform);
        }

        private void AddHighscoreEntry(int score, string name)
        {
            Debug.Log($"AddHighscoreEntry: {score}, {name}");

            // Create HighscoreEntry
            HighscoreEntry highscoreEntry = new HighscoreEntry {score = score, name = name};

            // Load saved Highscores
            string jsonString = PlayerPrefs.GetString("HighScoreTable");
            Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

            string json;

            if (highscores != null)
            {
                // Add new entry to Highscores
                highscores.highscoreEntryList.Add(highscoreEntry);
                highscores.highscoreEntryList = highscores.highscoreEntryList.Take(10).ToList();
                json = JsonUtility.ToJson(highscores);
            }
            else
            {
                json = JsonUtility.ToJson(highscoreEntry);
            }

            // Save updated Highscores
            //string json = JsonUtility.ToJson(highscores); 
            PlayerPrefs.SetString("HighScoreTable", json);
            PlayerPrefs.Save();
        }

        /*
         * Represents a single High Score entry
         */
        private class Highscores
        {
            public List<HighscoreEntry> highscoreEntryList;
        }

        [System.Serializable]
        private class HighscoreEntry
        {
            public int score;
            public string name;
        }
    }
}