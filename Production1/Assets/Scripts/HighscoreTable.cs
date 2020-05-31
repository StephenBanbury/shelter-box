using System;
using System.Collections.Generic;
using System.Linq;
using Com.MachineApps.PrepareAndDeploy.Models;
using Com.MachineApps.PrepareAndDeploy.Services;
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

        private ScoreService scoreService;

        public void Seed()
        {
            Debug.Log("Seed");
            scoreService.AddHighscoreEntry(100, "ABC");
        }

        private void Awake()
        {
            scoreService = new ScoreService(numberInHighscoresTable: 10);
            //FillHighscoresTable();
        }

        public void FillHighscoresTable(Highscores highscores)
        {
            entryContainer = transform.Find("highscoreEntryContainer");
            entryTemplate = entryContainer.Find("highscoreEntryTemplate");
            entryTemplate.gameObject.SetActive(false);

            Debug.Log($"FillHighscoresTable: {highscores.highscoreEntryList.Count}");

            if (highscores != null)
            {
                highscoreEntryTransformList = new List<Transform>();

                // Destroy any previous entries on the board
                for (var i = 1; i <= 10; i++)
                {
                    var tr = entryContainer.FindChildRecursive($"entryTransform{i}");
                    if (tr != null)
                    {
                        Destroy(tr.gameObject);
                    }
                }

                // Create new entries
                foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
                {
                    CreateHighscoreEntryTransform(highscoreEntry, entryContainer);
                }

            }
        }

        private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container) //, List<Transform> transformList)
        {
            float templateHeight = 0.1f;
            Transform entryTransform = Instantiate(entryTemplate, container);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * highscoreEntryTransformList.Count);
            entryTransform.gameObject.SetActive(true);

            int rank = highscoreEntryTransformList.Count + 1;

            entryTransform.name = $"entryTransform{rank}";

            string rankString;
            switch (rank)
            {
                default:
                    rankString = rank + "TH"; break;

                case 1: rankString = "1ST"; break;
                case 2: rankString = "2ND"; break;
                case 3: rankString = "3RD"; break;
            }

            //var existingTransform = highscoreEntryTransformList[rank - 1];

            //entryTransform.Find("posText").GetComponent<Text>().text = rankString;
            var posText = entryTransform.Find("posText").GetComponent<Text>();
            if (posText.text != rankString) posText.text = rankString;


            int score = highscoreEntry.score;
           // entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();
            var scoreText = entryTransform.Find("scoreText").GetComponent<Text>();
            if (scoreText.text != score.ToString()) scoreText.text = score.ToString();
            

            string name = highscoreEntry.name;
            entryTransform.Find("nameText").GetComponent<Text>().text = name;
            var nameText = entryTransform.Find("nameText").GetComponent<Text>();
            if (nameText.text != name) nameText.text = "zzzz";

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

            highscoreEntryTransformList.Add(entryTransform);
        }
    }
}