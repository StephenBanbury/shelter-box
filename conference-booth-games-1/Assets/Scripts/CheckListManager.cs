using System;
using System.Text.RegularExpressions;
using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CheckListManager: MonoBehaviour
    {
        public static CheckListManager instance;
        public Text checkListText;

        //private CheckListService checkListService;


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

        public void UpdateCheckListText()
        {
            Array checkListItems = Enum.GetValues(typeof(CheckListItem));

            checkListText.text = "";

            foreach (var checkListItem in checkListItems)
            {
                if ((int)checkListItem != 0)
                {
                    var item = Regex.Replace(((CheckListItem)checkListItem).ToString(), "(\\B[A-Z])", " $1");
                    checkListText.text += item + Environment.NewLine;
                }
            }
        }
    }
}
