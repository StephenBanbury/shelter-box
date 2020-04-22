using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Com.MachineApps.PrepareAndDeploy.Enums;
using Com.MachineApps.PrepareAndDeploy.Models;
using Com.MachineApps.PrepareAndDeploy.Services;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Video;
using Random = UnityEngine.Random;

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

        [SerializeField] private int updateInterval = 60;
        [SerializeField] private bool rotateReports = true;

        public static List<Report> reports;

        public int reportId0 = 0;
        public int reportId1 = 1;
        public int reportId2 = 2;
        public int reportId3 = 3;

        private DateTime startDateTime = DateTime.UtcNow;
        private DateTime reviewDateTime;
        private ReportService reportService = new ReportService();
        //private List<Report> usedReports = new List<Report>();
        private List<int> usedIndexes = new List<int>();

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

            // Randomize reports at start
            while (usedIndexes.Count < 4)
            {
                int randomIndex = RandomReportIndex();
                
                if (!usedIndexes.Contains(randomIndex))
                {
                    usedIndexes.Add(randomIndex);
                    var report = reports.FirstOrDefault(r => r.Id == randomIndex);
                    report.DisasterStatus = DisasterStatus.Pending;
                }
            }

            reportId0 = usedIndexes[0];
            reportId1 = usedIndexes[1];
            reportId2 = usedIndexes[2];
            reportId3 = usedIndexes[3];

            AssignReportsToMonitors();
        }

        void FixedUpdate()
        {
            // Only rotate reports if rotateReports = true
            //if (DateTime.UtcNow >= reviewDateTime && rotateReports)
            //{
            //    reportId0 = reportId0 < reports.Count - 1 ? reportId0 + 1 : 0;
            //    reportId1 = reportId1 < reports.Count - 1 ? reportId1 + 1 : 0;
            //    reportId2 = reportId2 < reports.Count - 1 ? reportId2 + 1 : 0;
            //    reportId3 = reportId3 < reports.Count - 1 ? reportId3 + 1 : 0;

            //    AssignReportsToMonitors();

            //    reviewDateTime = DateTime.UtcNow.AddSeconds(updateInterval);
            //}

            // Select random report and replace it with a new report
            if (DateTime.UtcNow >= reviewDateTime && rotateReports)
            {
                ReplaceReport();
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

            // Debug text
            string debugList = "";
            foreach (var report in reports) //.Where(r => usedIndexes.Contains(r.Id)))
            {
                debugList +=  $"({report.Title.Replace("!","")} ({report.DisasterStatus.ToString()}) \n";
            }

            GameManager.instance.DebugText(debugList);
        }

        public void DisasterScenarioDeployed(int reportId)
        {
            var thisReport = reports.FirstOrDefault(r => r.Id == reportId);
            thisReport.DisasterStatus = DisasterStatus.Success;

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

        private int RandomReportIndex()
        {
            var randomIndex = (int) (reports.Count * Random.value);

            // use random index if not used before, otherwise recursively generate a new one
            var returnValue = !usedIndexes.Contains(randomIndex)
                ? randomIndex
                : RandomReportIndex();

            Debug.Log($"randomIndex: {returnValue}");
            return returnValue;
        }

        private void ReplaceReport()
        {
            var newReportIndex = RandomReportIndex();
            var randomMonitor = (int) (4 * Random.value);

            Report report;

            switch (randomMonitor)
            {
                case 1:
                    report = reports.FirstOrDefault(r => r.Id == reportId0);
                    report.DisasterStatus = DisasterStatus.Fail;
                    reportId0 = newReportIndex;
                    break;
                case 2:
                    report = reports.FirstOrDefault(r => r.Id == reportId1);
                    report.DisasterStatus = DisasterStatus.Fail;
                    reportId1 = newReportIndex;
                    break;
                case 3:
                    report = reports.FirstOrDefault(r => r.Id == reportId2);
                    report.DisasterStatus = DisasterStatus.Fail;
                    reportId2 = newReportIndex;
                    break;
                    report = reports.FirstOrDefault(r => r.Id == reportId3);
                    report.DisasterStatus = DisasterStatus.Fail;
                    reportId3 = newReportIndex;
                    break;
            }

            report = reports.FirstOrDefault(r => r.Id == newReportIndex);
            report.DisasterStatus = DisasterStatus.Pending;

            Debug.Log($"Monitor {randomMonitor} - report replaced with {newReportIndex}");
            //usedIndexes.Add(newReportIndex);
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

            Debug.Log($"Monitor: {monitor}");

            // TODO either...
            // Replace existing report with a new unused one.
            // ReplaceReport(reportId);

            // TODO or...
            AnimationManager.instance.ActivateMonitor(monitor, false);

            video1.gameObject.SetActive(false);
            video2.gameObject.SetActive(false);
            video3.gameObject.SetActive(false);
            video4.gameObject.SetActive(false);
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