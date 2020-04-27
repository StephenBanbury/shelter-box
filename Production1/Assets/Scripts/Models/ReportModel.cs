using System;
using System.Collections.Generic;
using Com.MachineApps.PrepareAndDeploy.Enums;

namespace Com.MachineApps.PrepareAndDeploy.Models
{
    public class Operation
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Text { get; set; }
        public DateTime ReportDate { get; set; }
        public bool Archived { get; set; }
        public List<int> RequiredResources { get; set; }
        public List<int> CollectedResources { get; set; }
        public OperationStatus OperationStatus { get; set; }
        public string MovieTitle { get; set; }
    }
}