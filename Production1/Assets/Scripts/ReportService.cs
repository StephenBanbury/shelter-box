using System;
using System.Collections.Generic;
using Com.MachineApps.PrepareAndDeploy.Enums;

namespace Com.MachineApps.PrepareAndDeploy.Services
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
                    Title = "Hurricane!",
                    SubTitle = "Thousands of homes destroyed",
                    Text = "Hundreds of families affected",
                    ReportDate = DateTime.UtcNow,
                    Archived = false,
                    RequiredResources = new int[]
                    {
                        //(int) Resource.Clothing, 
                        (int) Resource.Food, 
                        (int) Resource.FirstAidKits, 
                        (int) Resource.Tents, 
                        (int) Resource.Toys, 
                        (int) Resource.Water,
                        //(int) Resource.Blankets
                    },
                    CollectedResources = new List<int>()
                },
                new Report
                {
                    Id = 2,
                    Title = "Drought!",
                    SubTitle = "Water supplies depleted and crops failing",
                    Text = "UN declares it a Humanitarian Crisis",
                    ReportDate = DateTime.UtcNow,
                    Archived = false,
                    RequiredResources = new int[]
                    {
                        //(int) Resource.Clothing,
                        (int) Resource.Food,
                        (int) Resource.FirstAidKits,
                        (int) Resource.Tents,
                        (int) Resource.Toys,
                        (int) Resource.Water
                    },
                    CollectedResources = new List<int>()
                },
                new Report
                {
                    Id = 3,
                    Title = "Civil unrest!",
                    SubTitle = "Whole villages forced to leave homes",
                    Text = "It's not known how many have found safety'",
                    ReportDate = DateTime.UtcNow,
                    Archived = false,
                    RequiredResources = new int[]
                    {
                        //(int) Resource.Clothing,
                        (int) Resource.Food,
                        (int) Resource.FirstAidKits,
                        (int) Resource.Tents,
                        (int) Resource.Toys,
                        (int) Resource.Water,
                        //(int) Resource.Blankets
                    },
                    CollectedResources = new List<int>()
                },
                new Report
                {
                    Id = 4,
                    Title = "Flooding!",
                    SubTitle = "Widespread displacement of people",
                    Text = "Neighbouring countries say they can provide limited assistance",
                    ReportDate = DateTime.UtcNow,
                    Archived = false,
                    RequiredResources = new int[]
                    {
                        //(int) Resource.Clothing,
                        (int) Resource.Food,
                        (int) Resource.FirstAidKits,
                        (int) Resource.Tents,
                        (int) Resource.Toys,
                        (int) Resource.Water,
                        (int) Resource.Boats,
                        //(int) Resource.Blankets
                    },
                    CollectedResources = new List<int>()
                },
                new Report
                {
                    Id = 5,
                    Title = "Tsunami!",
                    SubTitle = "As many as 2000 people affected",
                    Text = "Rescue is underway",
                    ReportDate = DateTime.UtcNow,
                    Archived = false,
                    RequiredResources = new int[]
                    {
                        //(int) Resource.Clothing,
                        (int) Resource.Food,
                        (int) Resource.FirstAidKits,
                        (int) Resource.Tents,
                        (int) Resource.Toys,
                        (int) Resource.Water,
                        (int) Resource.Boats,
                        //(int) Resource.Blankets
                    },
                    CollectedResources = new List<int>()
                },
                new Report
                {
                    Id = 6,
                    Title = "Earthquake!",
                    SubTitle = "Five villages evacuated",
                    Text = "Hundreds of people unaccounted for",
                    ReportDate = DateTime.UtcNow,
                    Archived = false,
                    RequiredResources = new int[]
                    {
                        //(int) Resource.Clothing,
                        (int) Resource.Food,
                        (int) Resource.FirstAidKits,
                        (int) Resource.Tents,
                        (int) Resource.Toys,
                        (int) Resource.Water,
                        //(int) Resource.Blankets
                    },
                    CollectedResources = new List<int>()
                },
                new Report
                {
                    Id = 7,
                    Title = "Volcano!",
                    SubTitle = "East side of island evacuated",
                    Text = "Thousands of people in search of shelter",
                    ReportDate = DateTime.UtcNow,
                    Archived = false,
                    RequiredResources = new int[]
                    {
                        //(int) Resource.Clothing,
                        (int) Resource.Food,
                        (int) Resource.FirstAidKits,
                        (int) Resource.Tents,
                        (int) Resource.Toys,
                        (int) Resource.Water,
                        //(int) Resource.Blankets
                    },
                    CollectedResources = new List<int>()
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
        public List<int> CollectedResources { get; set; }
    }

}
