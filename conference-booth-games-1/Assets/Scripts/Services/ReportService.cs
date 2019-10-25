using System;
using System.Collections.Generic;
using Assets.Scripts.Enums;

namespace Assets.Scripts.Services
{
    public class ReportService
    {
        public List<Report> GetReports()
        {
            var returnReports = new List<Report>();

            // Mock reports for testing
            returnReports.AddRange(MockReports());

            return returnReports;
        }

        private List<Report> MockReports()
        {
            var returnReports = new List<Report>
            {
                new Report
                {
                    Id = 1,
                    Title = "Storm Gerry makes landfall",
                    SubTitle = "Several homes uninhabitable",
                    Text = "Whole families severely affected",
                    ReportDate = DateTime.UtcNow,
                    Archived = false,
                    RequiredResources = new int[] { (int) Resource.Clothing, (int) Resource.Food, (int) Resource.MedicalSupplies, (int) Resource.Tents, (int) Resource.Toys, (int) Resource.Water }
                },
                new Report
                {
                    Id = 2,
                    Title = "Record drought causes widespread famine in East Grinstead",
                    SubTitle = "Water supplies and fast food outlets running dry",
                    Text = "Queues reach record lengths at local shops and GP surgeries",
                    ReportDate = DateTime.UtcNow,
                    Archived = false,
                    RequiredResources = new int[] { (int) Resource.Food, (int) Resource.MedicalSupplies, (int) Resource.Water }
                },
                new Report
                {
                    Id = 3,
                    Title = "Civil unrest in Barry",
                    SubTitle = "Barry Island under siege",
                    Text = "Some people badly injured",
                    ReportDate = DateTime.UtcNow,
                    Archived = false,
                    RequiredResources = new int[] { (int) Resource.MedicalSupplies }
                },
                new Report
                {
                    Id = 4,
                    Title = "Electricity cut in Penryn",
                    SubTitle = "University campus offline for whole lunchtime",
                    Text = "Kitchen appliances and cashpoints not working",
                    ReportDate = DateTime.UtcNow,
                    Archived = false,
                    RequiredResources = new int[] { (int) Resource.Food, (int) Resource.Water }
                }

            };

            return returnReports;
        }
    }


    public class Report
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Text { get; set; }
        public DateTime ReportDate { get; set; }
        public bool Archived { get; set; }
        public int[] RequiredResources { get; set; }
    }

}
