using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.MachineApps.PrepareAndDeploy
{
    public class HighscoreTable : MonoBehaviour
    {
        private Transform entryContainer;
        private Transform entryTemplate;
        
        private void Awake()
        {
            entryContainer = transform.Find("highscoreEntryContainer");
            entryTemplate = entryContainer.Find("highscoreEntryTemplate");

            entryTemplate.gameObject.SetActive(false);
        }
    }
}