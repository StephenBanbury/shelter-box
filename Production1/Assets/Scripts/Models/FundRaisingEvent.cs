using System;
using Com.MachineApps.PrepareAndDeploy.Enums;

namespace Com.MachineApps.PrepareAndDeploy.Models
{
    public class FundRaisingEvent
    {
        public int Id { get; set; }
        public FundRaisingEventType FundRaisingEventType { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Text { get; set; }
        public DateTime EventDate { get; set; }
        public int EstimatedFundsRaised { get; set; }
        public int FundsRaised { get; set; }
        public FundRaisingEventStatus FundRaisingEventStatus { get; set; }
    }
}