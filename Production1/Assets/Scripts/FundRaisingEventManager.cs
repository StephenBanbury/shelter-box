﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


        [Tooltip("Maximum number of fundraising events allowed")]
        [SerializeField] private Text computerText;

        public int numberOfEventsAllowed = 5;
        public int numberOfEventsUsed;

        private static int currentEventId;

        private readonly FundRaisingEventService fundRaisingEventService = new FundRaisingEventService();
        private List<FundRaisingEvent> usedFundRaisingEvents = new List<FundRaisingEvent>();

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
            Debug.Log("FundRaisingEventManager Start()");

            NextEvent();
        }

        public int CurrentEventId()
        {
            return currentEventId;
        }

        public void MarkEventAsUsed(int eventId)
        {
            usedFundRaisingEvents.Add(fundRaisingEvents.FirstOrDefault(e => e.Id == eventId));
            numberOfEventsUsed++;
        }

        public FundRaisingEvent NextEvent()
        {
            var currentEvent = GetRandomEvent();
            computerText.text = currentEvent.Title + "\r\n" + currentEvent.SubTitle;
            currentEventId = currentEvent.Id;
            return currentEvent;
        }

        private FundRaisingEvent GetRandomEvent()
        {
            var eventsToSelectFrom =
                fundRaisingEvents
                    .Where(e => !usedFundRaisingEvents.Select(u => u.Id)
                        .Contains(e.Id));

            var rand = Random.value;
            int ix = (int) (eventsToSelectFrom.Count() * rand) - 1;

            return fundRaisingEvents[ix];
        }
    }
}