using System;
using System.Collections.Generic;
using Assets.Scripts.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Reports : MonoBehaviour
    {
        public static Reports instance;

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
        private int intervalSeconds = 15;

        private ReportService reportService = new ReportService();


        void Awake()
        {
            // Check that it exists
            if (instance == null)
            {
                //assign it to the current object
                instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            reviewDateTime = startDateTime.AddSeconds(intervalSeconds);
            reports = reportService.GetReports();
            AssignReportsToMonitors();
        }

        // Update is called once per frame
        void Update()
        {
            if (DateTime.UtcNow >= reviewDateTime)
            {
                reportIndex0 = reportIndex0 < 3 ? reportIndex0 + 1 : 0;
                reportIndex1 = reportIndex1 < 3 ? reportIndex1 + 1 : 0;
                reportIndex2 = reportIndex2 < 3 ? reportIndex2 + 1 : 0;
                reportIndex3 = reportIndex3 < 3 ? reportIndex3 + 1 : 0;

                AssignReportsToMonitors();

                reviewDateTime = DateTime.UtcNow.AddSeconds(intervalSeconds);
            }
        }

        void AssignReportsToMonitors()
        {
            monitor1aText.text = reports[reportIndex0].Title;
            monitor2aText.text = reports[reportIndex1].Title;
            monitor3aText.text = reports[reportIndex2].Title;
            monitor4aText.text = reports[reportIndex3].Title;
            monitor1bText.text = reports[reportIndex0].SubTitle;
            monitor2bText.text = reports[reportIndex1].SubTitle;
            monitor3bText.text = reports[reportIndex2].SubTitle;
            monitor4bText.text = reports[reportIndex3].SubTitle;
            monitor1cText.text = reports[reportIndex0].Text;
            monitor2cText.text = reports[reportIndex1].Text;
            monitor3cText.text = reports[reportIndex2].Text;
            monitor4cText.text = reports[reportIndex3].Text;
        }
    }
}