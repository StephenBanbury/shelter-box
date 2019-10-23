using System;
using System.Collections.Generic;

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
                    SubTitle = "Several homes get wet",
                    Text = "It is not known how many people have been affected, but it is thought to be more than 5",
                    ReportDate = DateTime.UtcNow,
                    Archived = false
                },
                new Report
                {
                    Id = 2,
                    Title = "Record drought causes widespread famine in East Grinstead",
                    SubTitle = "Big Macs are scarce",
                    Text = "Queues for Krispy Kreme donuts reach record lengths",
                    ReportDate = DateTime.UtcNow,
                    Archived = false
                },
                new Report
                {
                    Id = 3,
                    Title = "Civil unrest in Barry",
                    SubTitle = "Barry Island under siege",
                    Text = "Chip suppers having trouble getting through the barricades",
                    ReportDate = DateTime.UtcNow,
                    Archived = false
                },
                new Report
                {
                    Id = 4,
                    Title = "Electricity cut in Penryn",
                    SubTitle = "University campus offline for whole lunchtime",
                    Text = "The deprivation is appalling",
                    ReportDate = DateTime.UtcNow,
                    Archived = false
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
    }

}
