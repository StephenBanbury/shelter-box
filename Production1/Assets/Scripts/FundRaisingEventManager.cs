using System;
using System.Collections;
using System.Collections.Generic;
using Com.MachineApps.PrepareAndDeploy.Models;
using Com.MachineApps.PrepareAndDeploy.Services;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class FundRaisingEventManager : MonoBehaviour
    {
        public static FundRaisingEventManager instance;
        public static List<FundRaisingEvent> fundRaisingEvents;

        //[SerializeField]
        public Text computerText;
        public static int currentIndex = -1;

        private FundRaisingEventService fundRaisingEventService = new FundRaisingEventService();

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                fundRaisingEvents = fundRaisingEventService.GetFundRaisingEvents();
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            NextEvent();
        }

        public void NextEvent()
        {
            var numEvents = fundRaisingEvents.Count;
            var rand = Random.value;

            var selected = (int) (numEvents * rand);
            //Debug.Log($"New event id: {selected}");

            currentIndex = selected - 1;

            var currentEvent = fundRaisingEvents[currentIndex];

            //Debug.Log($"Event Title: {currentEvent.Title}");

            computerText.text = currentEvent.Title + "\r\n" + currentEvent.SubTitle;
        }
    }
}