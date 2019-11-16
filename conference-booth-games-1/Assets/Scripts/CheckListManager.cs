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

        public Text checkListText;

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
            UpdateCheckListText();
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