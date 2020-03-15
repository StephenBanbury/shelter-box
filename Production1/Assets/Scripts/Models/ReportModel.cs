using System;
using System.Collections.Generic;

namespace Com.MachineApps.PrepareAndDeploy.Models
{
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