using System;
using Com.MachineApps.PrepareAndDeploy.Enums;

namespace Com.MachineApps.PrepareAndDeploy.Models
{
    public class Player
    {
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int CurrentScore { get; set; }
        public int HighScore { get; set; }
    }
}