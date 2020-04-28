using System;
using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class GuidHelper
    {
        public static string GetUniqueID()
        {
            string key = "ID";

            var random = new System.Random();
            DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
            double timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;

            string uniqueID = Application.systemLanguage //Language
                              //+ "-" + GetPlatform() //Device    
                              + "-" + String.Format("{0:X}", Convert.ToInt32(timestamp)) //Time
                              + "-" + String.Format("{0:X}", Convert.ToInt32(Time.time * 1000000)) //Time in game
                              + "-" + String.Format("{0:X}", random.Next(1000000000)); //random number

            Debug.Log("Generated Unique ID: " + uniqueID);

            if (PlayerPrefs.HasKey(key))
            {
                uniqueID = PlayerPrefs.GetString(key);
            }
            else
            {
                PlayerPrefs.SetString(key, uniqueID);
                PlayerPrefs.Save();
            }

            return uniqueID;
        }
    }
}