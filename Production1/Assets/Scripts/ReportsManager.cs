using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using Com.MachineApps.PrepareAndDeploy.Enums;
using Com.MachineApps.PrepareAndDeploy.Models;
using Com.MachineApps.PrepareAndDeploy.Services;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Random = System.Random;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class ReportsManager : MonoBehaviour
    {
        public static ReportsManager instance;

        [SerializeField] private GameObject monitor1;
        [SerializeField] private GameObject monitor2;
        [SerializeField] private GameObject monitor3;
        [SerializeField] private GameObject monitor4;

        [SerializeField] private Text monitor1aText;
        [SerializeField] private Text monitor2aText;
        [SerializeField] private Text monitor3aText;
        [SerializeField] private Text monitor4aText;
        [SerializeField] private Text monitor1bText;
        [SerializeField] private Text monitor2bText;
        [SerializeField] private Text monitor3bText;
        [SerializeField] private Text monitor4bText;
        [SerializeField] private Text monitor1cText;
        [SerializeField] private Text monitor2cText;
        [SerializeField] private Text monitor3cText;
        [SerializeField] private Text monitor4cText;

        [SerializeField] private VideoPlayer video1;
        [SerializeField] private VideoPlayer video2;
        [SerializeField] private VideoPlayer video3;
        [SerializeField] private VideoPlayer video4;

        public static List<Report> reports;

        public int reportId0 = 0;
        public int reportId1 = 1;
        public int reportId2 = 2;
        public int reportId3 = 3;

        private List<Report> usedReports = new List<Report>();
        private DateTime startDateTime = DateTime.UtcNow;
        private DateTime reviewDateTime;
        private int updateInterval = 15;
        private bool rotateReports = false;
        private Animation anim;
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
            Debug.Log("ReportManager Start");

            reviewDateTime = startDateTime.AddSeconds(updateInterval);

            List<int> randomReportIndexes = new List<int>();

            // Randomize reports at start
            while (randomReportIndexes.Count < 4)
            {
                Random random = new Random();
                int randomIndex = random.Next(0, reports.Count);
                if (!randomReportIndexes.Contains(randomIndex))
                {
                    randomReportIndexes.Add((randomIndex));
                    var usedReport = reports.FirstOrDefault(r => r.Id == randomIndex);
                    usedReports.Add(usedReport);
                }
            }

            reportId0 = randomReportIndexes[0];
            reportId1 = randomReportIndexes[1];
            reportId2 = randomReportIndexes[2];
            reportId3 = randomReportIndexes[3];

            AssignReportsToMonitors();
        }

        void Update()
        {
            // Only rotate reports if rotateReports = true
            if (DateTime.UtcNow >= reviewDateTime && rotateReports)
            {
                reportId0 = reportId0 < reports.Count - 1 ? reportId0 + 1 : 0;
                reportId1 = reportId1 < reports.Count - 1 ? reportId1 + 1 : 0;
                reportId2 = reportId2 < reports.Count - 1 ? reportId2 + 1 : 0;
                reportId3 = reportId3 < reports.Count - 1 ? reportId3 + 1 : 0;

                AssignReportsToMonitors();

                reviewDateTime = DateTime.UtcNow.AddSeconds(updateInterval);
            }
        }

        public void AssignReportsToMonitors()
        {
            // Heading
            monitor1aText.text = reports[reportId0].Title;
            monitor2aText.text = reports[reportId1].Title;
            monitor3aText.text = reports[reportId2].Title;
            monitor4aText.text = reports[reportId3].Title;

            // Subheading
            monitor1bText.text = reports[reportId0].SubTitle;
            monitor2bText.text = reports[reportId1].SubTitle;
            monitor3bText.text = reports[reportId2].SubTitle;
            monitor4bText.text = reports[reportId3].SubTitle;

            // Checklist
            monitor1cText.text = ResourceListText(reportId0);
            monitor2cText.text = ResourceListText(reportId1);
            monitor3cText.text = ResourceListText(reportId2);
            monitor4cText.text = ResourceListText(reportId3);
        }

        public void DisasterScenarioDeployed(int reportId)
        {
            StartCoroutine(DeployedRoutine(reportId));
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

        private IEnumerator DeployedRoutine(int reportId)
        {
            Debug.Log($"StartCounter - reportId: {reportId}");

            string monitor = "";

            if (reportId == reportId0)
            {
                monitor = "monitor1";
                video1.gameObject.SetActive(true);
                video1.Play();
            }
            else if (reportId == reportId1)
            {
                monitor = "monitor2";
                video2.gameObject.SetActive(true);
                video2.Play();
            }
            else if (reportId == reportId2)
            {
                monitor = "monitor3";
                video3.gameObject.SetActive(true);
                video3.Play();
            }
            else if (reportId == reportId3)
            {
                monitor = "monitor4";
                video4.gameObject.SetActive(true);
                video4.Play();
            }

            yield return new WaitForSeconds(5);

            video1.Stop();
            video2.Stop();
            video3.Stop();
            video4.Stop();

            video1.gameObject.SetActive(false);
            video2.gameObject.SetActive(false);
            video3.gameObject.SetActive(false);
            video4.gameObject.SetActive(false);

            Debug.Log($"Monitor: {monitor}");

            // TODO either...
            // Replace existing report with a new unused one.
            // ReplaceReport(reportId);

            // TODO or...
            AnimationManager.instance.ActivateMonitor(monitor, false);

        }

        private void ReplaceReport(int reportId)
        {
            foreach (var usedReport in usedReports)
            {
                Debug.Log($"usedReportId: {usedReport.Id}");
            }
            
            foreach (var report in reports)
            {
                Debug.Log($"reportId: {report.Id}");
            }

            var newReport =
                reports.FirstOrDefault(r => !usedReports.Select(u => u.Id).Contains(r.Id));

            if (newReport != null)
            {
                var newReportId = newReport.Id;
                usedReports.Add(newReport);

                if (reportId0 == reportId)
                {
                    reportId0 = newReportId;
                }
                else if (reportId1 == reportId)
                {
                    reportId1 = newReportId;
                }
                else if (reportId2 == reportId)
                {
                    reportId2 = newReportId;
                }
                else if (reportId3 == reportId)
                {
                    reportId3 = newReportId;
                }
            }
            else
            {
                // TODO shut down monitor
            }

            AssignReportsToMonitors();
        }

        private string ResourceListText(int reportId)
        {
            var requiredResources = reports[reportId].RequiredResources;
            var collectedResources = reports[reportId].CollectedResources;

            var resourceText = "";
            foreach (var requiredResource in requiredResources)
            {
                if (!collectedResources.Contains(requiredResource))
                {
                    resourceText += Regex.Replace(((Resource)requiredResource).ToString(), "(\\B[A-Z])", " $1");
                    resourceText += "\r\n";
                }
            }

            return resourceText;
        }
    }
}