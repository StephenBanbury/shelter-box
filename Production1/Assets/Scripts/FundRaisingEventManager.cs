using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Com.MachineApps.PrepareAndDeploy.Enums;
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

        [SerializeField] private Text computerText;

        [Tooltip("Maximum number of fundraising events allowed")]
        [SerializeField] private int numberOfEventsAllowed = 6;


        private List<FundRaisingEvent> fundRaisingEvents;

        public List<FundRaisingEvent> FundRaisingEvents => fundRaisingEvents;

        public int NumberOfEventsAllowed => numberOfEventsAllowed;

        //public int NumberOfEventsUsed { get { return numberOfEventsUsed; } }
        public int NumberOfEventsUsed
        {
            get { return fundRaisingEvents.Count(f => f.FundRaisingEventStatus == FundRaisingEventStatus.Completed); }
        }



        //private static int numberOfEventsUsed;
        //private static int currentEventId;

        private readonly FundRaisingEventService fundRaisingEventService = new FundRaisingEventService();
        //private List<FundRaisingEvent> usedFundRaisingEvents = new List<FundRaisingEvent>();

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
            NextDisplayedEvent();
            UpdateFundraisingEventsChart();
        }

        public int? DisplayedEventId()
        {
            //return currentEventId;
            var currentEvent =
                fundRaisingEvents.FirstOrDefault(f => f.FundRaisingEventStatus == FundRaisingEventStatus.OnDisplay);

            return currentEvent?.Id;
        }

        public void MarkEvent(int? eventId, FundRaisingEventStatus status)
        {
            if (eventId != null)
            {
                var thisEvent = fundRaisingEvents.FirstOrDefault(e => e.Id == eventId);
                thisEvent.FundRaisingEventStatus = status;

                //numberOfEventsUsed++;
                GameManager.instance.UpdateFundingEventLives();

                UpdateFundraisingEventsChart();
            }
        }

        //public void MarkEventAsUsed(int? eventId)
        //{
        //    if (eventId != null)
        //    {
        //        var currentEvent = fundRaisingEvents.FirstOrDefault(e => e.Id == eventId);
        //        currentEvent.FundRaisingEventStatus = FundRaisingEventStatus.Completed;

        //        // TODO remove this once happy
        //        //usedFundRaisingEvents.Add(currentEvent);

        //        //numberOfEventsUsed++;
        //        GameManager.instance.UpdateFundingEventLives();
        //    }
        //}

        public void NextDisplayedEvent()
        {
            if (NumberOfEventsUsed < numberOfEventsAllowed)
            {
                var displayedEvent =
                    fundRaisingEvents.FirstOrDefault(f => f.FundRaisingEventStatus == FundRaisingEventStatus.OnDisplay);
                if (displayedEvent != null)
                {
                    displayedEvent.FundRaisingEventStatus = FundRaisingEventStatus.Pending;
                }

                var nextDisplayedEvent = GetRandomEvent();

                computerText.text = nextDisplayedEvent.Title + "\n\n" +
                                    nextDisplayedEvent.SubTitle + "\n\n" +
                                    //"Estimated funds raised: " + currentEvent.EstimatedFundsRaised.ToString("C", CultureInfo.CurrentCulture).Replace(".00", "") + "\n\n" +
                                    "Estimated funds raised: £" +
                                    nextDisplayedEvent.EstimatedFundsRaised.ToString().Replace(".00", "") + "\n\n" +
                                    "Number of possible events remaining: " +
                                    (numberOfEventsAllowed - NumberOfEventsUsed);

                nextDisplayedEvent.FundRaisingEventStatus = FundRaisingEventStatus.OnDisplay;


                Debug.Log($"NextDisplayedEvent: {nextDisplayedEvent.Title} (Id={nextDisplayedEvent.Id})");
            }
            else
            {
                computerText.text = "Sorry - you have used up your allocation of fundraising events";
            }

            //currentEventId = currentEvent.Id;
            //return currentEvent;
        }

        private FundRaisingEvent GetRandomEvent()
        {
            //var eventsToSelectFrom =
            //    fundRaisingEvents
            //        .Where(e => !usedFundRaisingEvents.Select(u => u.Id)
            //            .Contains(e.Id));

            var eventsToSelectFrom =
                fundRaisingEvents.Where(e => e.FundRaisingEventStatus == FundRaisingEventStatus.Pending).ToList();

            var rand = Random.value;
            int ix = (int) (eventsToSelectFrom.Count() * rand);

            //var currentEvent = fundRaisingEvents[ix];
            //currentEvent.FundRaisingEventStatus = FundRaisingEventStatus.OnDisplay;

            //eventsToSelectFrom.ForEach(e => { Debug.Log($"Pending event: {e.Title}"); });

            //return fundRaisingEvents[ix];
            return eventsToSelectFrom[ix];
        }

        private void UpdateFundraisingEventsChart()
        {
            string pendingList = "";
            string current = "";
            string completedList = "";

            foreach (var fundingEvent in fundRaisingEvents
                .Where(e => e.FundRaisingEventStatus == FundRaisingEventStatus.Pending || e.FundRaisingEventStatus == FundRaisingEventStatus.OnDisplay)
                .OrderBy(r => r.Title))
            {
                pendingList += $"{fundingEvent.Title}\n";
            }

            foreach (var fundingEvent in fundRaisingEvents
                .Where(e => e.FundRaisingEventStatus == FundRaisingEventStatus.Current)
                .OrderBy(r => r.Title))
            {
                current += $"{fundingEvent.Title}\n";
            }

            foreach (var fundingEvent in fundRaisingEvents
                .Where(e => e.FundRaisingEventStatus == FundRaisingEventStatus.Completed)
                .OrderBy(r => r.Title))
            {
                completedList += $"{fundingEvent.Title}\n";
            }

            GameManager.instance.FundraisingEventStatusText(pendingList, current, completedList);

        }
    }
}