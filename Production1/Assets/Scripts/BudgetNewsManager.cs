using System;
using System.Collections.Generic;
using Com.MachineApps.PrepareAndDeploy.Models;
using Com.MachineApps.PrepareAndDeploy.Services;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class BudgetNewsManager : MonoBehaviour
    {
        public static BudgetNewsManager instance;
        public static List<FundRaisingEvent> FundRaisingEvents;

        //[SerializeField]
        public Text ComputerText;

        public int BudgetReportId0 = 0;
        //public int reportId1 = 1;
        //public int reportId2 = 2;
        //public int reportId3 = 3;

        private DateTime startDateTime = DateTime.UtcNow;
        private DateTime reviewDateTime;
        private int updateInterval = 15;

        private bool rotateFundingNews = false; 

        private FundRaisingEventService fundRaisingEventService = new FundRaisingEventService();

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                FundRaisingEvents = fundRaisingEventService.GetFundRaisingEvents();
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            Debug.Log("BudgetNewsManager Start");

            reviewDateTime = startDateTime.AddSeconds(updateInterval);

            List<int> randomNewsIndexes = new List<int>();

            // Randomize reports at start
            while(randomNewsIndexes.Count < 4)
            {
                Random random = new Random();
                int randomIndex = random.Next(0, FundRaisingEvents.Count);
                if (!randomNewsIndexes.Contains(randomIndex))
                {
                    randomNewsIndexes.Add((randomIndex));
                }
            }

            BudgetReportId0 = randomNewsIndexes[0];
            //reportId1 = randomNewsIndexes[1];
            //reportId2 = randomNewsIndexes[2];
            //reportId3 = randomNewsIndexes[3];

            AssignReportsToMonitors();
        }

        void Update()
        {
            // Only rotate reports if rotateReports = true
            if (DateTime.UtcNow >= reviewDateTime && rotateFundingNews)
            {
                BudgetReportId0 = BudgetReportId0 < FundRaisingEvents.Count - 1 ? BudgetReportId0 + 1 : 0;
                //reportId1 = reportId1 < reports.Count - 1 ? reportId1 + 1 : 0;
                //reportId2 = reportId2 < reports.Count - 1 ? reportId2 + 1 : 0;
                //reportId3 = reportId3 < reports.Count - 1 ? reportId3 + 1 : 0;

                AssignReportsToMonitors();

                reviewDateTime = DateTime.UtcNow.AddSeconds(updateInterval);
            }
        }
        
        public void AssignReportsToMonitors()
        {
            // Heading
            ComputerText.text = FundRaisingEvents[BudgetReportId0].Title;
            //monitor2aText.text = reports[reportId1].Title;
            //monitor3aText.text = reports[reportId2].Title;
            //monitor4aText.text = reports[reportId3].Title;

            // Subheading
            //monitor1bText.text = reports[reportId0].SubTitle;
            //monitor2bText.text = reports[reportId1].SubTitle;
            //monitor3bText.text = reports[reportId2].SubTitle;
            //monitor4bText.text = reports[reportId3].SubTitle;

            // Checklist
            //monitor1cText.text = ResourceListText(reportId0);
            //monitor2cText.text = ResourceListText(reportId1);
            //monitor3cText.text = ResourceListText(reportId2);
            //monitor4cText.text = ResourceListText(reportId3);
        }

        //public void CollectResource(int reportId, int resourceId)
        //{
        //    reports[reportId].CollectedResources.Add(resourceId);
        //}

        //public int[] RequiredResources(int reportId)
        //{
        //    return reports[reportId].RequiredResources;
        //}

        //public int[] CollectedResources(int reportId)
        //{
        //    var report = reports[reportId];
        //    return report.CollectedResources.ToArray();
        //}

        //public TripleState SelectedResourceIsRequired(int reportId, int resourceId)
        //{
        //    var response = TripleState.One; // Not required or collected

        //    var resourcesRequiredForDisaster = RequiredResources(reportId);
        //    var resourcesCollected = CollectedResources(reportId);

        //    if (resourcesCollected.Contains(resourceId))
        //    {
        //        response = TripleState.Two; // Already collected
        //    }
        //    else if (resourcesRequiredForDisaster.Contains(resourceId))
        //    {
        //        response = TripleState.Three; // Is required
        //    }

        //    return response;
        //}

        //private string ResourceListText(int reportId)
        //{
        //    var requiredResources = reports[reportId].RequiredResources;
        //    var collectedResources = reports[reportId].CollectedResources;

        //    var resourceText = "";
        //    foreach (var requiredResource in requiredResources)
        //    {
        //        if (resourceText != "")
        //        {
        //            resourceText += ", ";
        //        }

        //        if (collectedResources.Contains(requiredResource))
        //        {
        //            resourceText += $"{Regex.Replace(((Resource)requiredResource).ToString(), "(\\B[A-Z])", " $1")} [check]";
        //        }
        //        else
        //        {
        //            resourceText += Regex.Replace(((Resource)requiredResource).ToString(), "(\\B[A-Z])", " $1");
        //        }
        //    }

        //    return resourceText;
        //}
    }
}