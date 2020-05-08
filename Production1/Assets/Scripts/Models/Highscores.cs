using System.Collections.Generic;

namespace Com.MachineApps.PrepareAndDeploy.Models
{
    public class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    public class HighscoreEntry
    {
        public int score;
        public string name;
    }
}