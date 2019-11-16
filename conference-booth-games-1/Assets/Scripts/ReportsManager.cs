using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.Enums;
using Assets.Scripts.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ReportsManager : MonoBehaviour
    {
        public static ReportsManager instance;

        public Text monitor1aText;
        public Text monitor2aText;
        public Text monitor3aText;
        public Text monitor4aText;
        public Text monitor1bText;
        public Text monitor2bText;
        public Text monitor3bText;
        public Text monitor4bText;
        public Text monitor1cText;
        public Text monitor2cText;
        public Text monitor3cText;
        public Text monitor4cText;

        public List<Report> reports;

        public int reportIndex0 = 0;
        public int reportIndex1 = 1;
        public int reportIndex2 = 2;
        public int reportIndex3 = 3;

        private DateTime startDateTime = DateTime.UtcNow;
        private DateTime reviewDateTime;
        private int updateReportsInterval = 15;

        private ReportService reportService = new ReportService();


        void Awake()
        {
            // Check that it exists
            if (instance == null)
            {
                //assign it to the current object
                instance = this;
            }
            // make sure that it is equal to the current object
            else if (instance != this)
            {
                // Destroy current game object - we only need one and we already have it
                Destroy(gameObject);
            }

            // don't destroy the object when changing scenes!
            DontDestroyOnLoad(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            reviewDateTime = startDateTime.AddSeconds(updateReportsInterval);
            reports = reportService.GetReports();
            AssignReportsToMonitors();
        }

        // Update is called once per frame
        void Update()
        {
            if (DateTime.UtcNow >= reviewDateTime)
            {
                reportIndex0 = reportIndex0 < reports.Count - 1 ? reportIndex0 + 1 : 0;
                reportIndex1 = reportIndex1 < reports.Count - 1 ? reportIndex1 + 1 : 0;
                reportIndex2 = reportIndex2 < reports.Count - 1 ? reportIndex2 + 1 : 0;
                reportIndex3 = reportIndex3 < reports.Count - 1 ? reportIndex3 + 1 : 0;

                AssignReportsToMonitors();

                reviewDateTime = DateTime.UtcNow.AddSeconds(updateReportsInterval);
            }
        }
        
        public void AssignReportsToMonitors()
        {
            monitor1aText.text = reports[reportIndex0].Title;
            monitor2aText.text = reports[reportIndex1].Title;
            monitor3aText.text = reports[reportIndex2].Title;
            monitor4aText.text = reports[reportIndex3].Title;

            monitor1bText.text = reports[reportIndex0].SubTitle;
            monitor2bText.text = reports[reportIndex1].SubTitle;
            monitor3bText.text = reports[reportIndex2].SubTitle;
            monitor4bText.text = reports[reportIndex3].SubTitle;

            monitor1cText.text = resourceListText(reportIndex0);
            monitor2cText.text = resourceListText(reportIndex1);
            monitor3cText.text = resourceListText(reportIndex2);
            monitor4cText.text = resourceListText(reportIndex3);
            //monitor1cText.text = resourceListText(reports[reportIndex0].RequiredResources);
            //monitor2cText.text = resourceListText(reports[reportIndex1].RequiredResources);
            //monitor3cText.text = resourceListText(reports[reportIndex2].RequiredResources);
            //monitor4cText.text = resourceListText(reports[reportIndex3].RequiredResources);
        }

        public void CollectResource(int reportId, int resourceId)
        {
            //var report = reports.Find(r => r.Id == reportId);
            //if (report.RequiredResourcesCollected.All(r => r != resourceId))
            //{
            //report.RequiredResourcesCollected.Add(resourceId);
            //}
            reports[reportId].CollectedResources.Add(resourceId);
        }

        public int[] CollectedResources(int reportIndex)
        {
            var report = reports.First(r => r.Id == reportIndex);
            var resourceIds = report.RequiredResources;
            return resourceIds;
        }

        public int[] RequiredResources(int reportIndex)
        {
            return reports[reportIndex].RequiredResources;
        }

        private string resourceListText(int reportIndex) //int[] requiredResources)
        {
            var requiredResources = reports[reportIndex].RequiredResources;
            var collectedResources = reports[reportIndex].CollectedResources;

            var resourceText = "";
            //foreach (var resource in requiredResources)
            foreach (var requiredResource in requiredResources)
            {
                if (resourceText != "")
                {
                    resourceText += ", ";
                }

                if (collectedResources.Contains(requiredResource))
                {
                    resourceText += $"{Regex.Replace(((Resource)requiredResource).ToString(), "(\\B[A-Z])", " $1")} (checked)";
                }
                else
                {
                    resourceText += Regex.Replace(((Resource)requiredResource).ToString(), "(\\B[A-Z])", " $1");
                }
            }

            return $"Required: {resourceText}";
        }

        //private string resourceListText(int[] requiredResources)
        //{
        //    //var requiredResources = reports[reportIndex1].RequiredResources;
        //    //var collectedResources = reports[reportIndex].CollectedResources;

        //    var resourceText = "";
        //    //foreach (var resource in requiredResources)
        //    foreach (var requiredResource in requiredResources)
        //    {
        //        if (resourceText != "")
        //        {
        //            resourceText += ", ";
        //        }

        //        resourceText += Regex.Replace(((Resource) requiredResource).ToString(), "(\\B[A-Z])", " $1");
        //    }

        //    return $"Required: {resourceText}";
        //}
    }
}