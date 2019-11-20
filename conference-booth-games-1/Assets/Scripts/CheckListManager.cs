using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CheckListManager : MonoBehaviour
    {
        public static CheckListManager instance;
        public static List<CheckListItem> collectedCheckListItems = new List<CheckListItem>();
        public static DeploymentStatus deploymentStatus;

        public Text checkListText;
        public Text deploymentStatusText;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            // Here we need to ensure the respective text is updated to display the static variables in order for them to appear to user as we move scenes
            UpdateCheckListText();
            UpdateDeploymentStatus(0);
        }

        public void UpdateDeploymentStatus(int alterStatusBy)
        {
            deploymentStatus = deploymentStatus + alterStatusBy;
            var statusText = Regex.Replace((deploymentStatus).ToString(), "(\\B[A-Z])", " $1");
            deploymentStatusText.text = $"Deployment status: {statusText}";
        }

        public void UpdateCollectedList(CheckListItem item)
        {
            collectedCheckListItems.Add(item);
        }

        public void UpdateCheckListText()
        {
            Array checkListItems = Enum.GetValues(typeof(CheckListItem));

            checkListText.text = "";

            foreach (var checkListItem in checkListItems)
            {
                if ((int) checkListItem != 0)
                {
                    var collected = collectedCheckListItems.Contains((CheckListItem) checkListItem) ? "(checked)" : "";
                    var itemText = Regex.Replace(((CheckListItem) checkListItem).ToString(), "(\\B[A-Z])", " $1");
                    checkListText.text += $"{itemText} {collected}" + Environment.NewLine;
                }
            }
        }
    }
}