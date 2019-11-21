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
        public static List<Report> reports;

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


        public int reportId0 = 0;
        public int reportId1 = 1;
        public int reportId2 = 2;
        public int reportId3 = 3;

        private DateTime startDateTime = DateTime.UtcNow;
        private DateTime reviewDateTime;
        private int updateReportsInterval = 15;

        private bool rotateReports = false; 

        private ReportService reportService = new ReportService();

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                reports = reportService.GetReports();
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            reviewDateTime = startDateTime.AddSeconds(updateReportsInterval);
            AssignReportsToMonitors();
        }

        void Update()
        {
            if (DateTime.UtcNow >= reviewDateTime && rotateReports)
            {
                reportId0 = reportId0 < reports.Count - 1 ? reportId0 + 1 : 0;
                reportId1 = reportId1 < reports.Count - 1 ? reportId1 + 1 : 0;
                reportId2 = reportId2 < reports.Count - 1 ? reportId2 + 1 : 0;
                reportId3 = reportId3 < reports.Count - 1 ? reportId3 + 1 : 0;

                AssignReportsToMonitors();

                reviewDateTime = DateTime.UtcNow.AddSeconds(updateReportsInterval);
            }
        }
        
        public void AssignReportsToMonitors()
        {
            monitor1aText.text = reports[reportId0].Title;
            monitor2aText.text = reports[reportId1].Title;
            monitor3aText.text = reports[reportId2].Title;
            monitor4aText.text = reports[reportId3].Title;

            monitor1bText.text = reports[reportId0].SubTitle;
            monitor2bText.text = reports[reportId1].SubTitle;
            monitor3bText.text = reports[reportId2].SubTitle;
            monitor4bText.text = reports[reportId3].SubTitle;

            monitor1cText.text = ResourceListText(reportId0);
            monitor2cText.text = ResourceListText(reportId1);
            monitor3cText.text = ResourceListText(reportId2);
            monitor4cText.text = ResourceListText(reportId3);
        }

        public void CollectResource(int reportId, int resourceId)
        {
            reports[reportId].CollectedResources.Add(resourceId);
        }

        public int[] RequiredResources(int reportId)
        {
            return reports[reportId].RequiredResources;
        }

        public int[] CollectedResources(int reportId)
        {
            var report = reports[reportId];
            return report.CollectedResources.ToArray();
        }

        public TripleState SelectedResourceIsRequired(int reportId, int resourceId)
        {
            var response = TripleState.One; // Not required or collected

            var resourcesRequiredForDisaster = RequiredResources(reportId);
            var resourcesCollected = CollectedResources(reportId);

            if (resourcesCollected.Contains(resourceId))
            {
                response = TripleState.Two; // Already collected
            }
            else if (resourcesRequiredForDisaster.Contains(resourceId))
            {
                response = TripleState.Three; // Is required
            }

            return response;
        }

        private string ResourceListText(int reportId)
        {
            var requiredResources = reports[reportId].RequiredResources;
            var collectedResources = reports[reportId].CollectedResources;

            var resourceText = "";
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
    }
}