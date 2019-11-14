using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class CheckListService : MonoBehaviour
    {
        //public List<CheckListItem> GetCheckList()
        //{
        //    var returnCheckListItems = new List<CheckListItem>();

        //    foreach (var checkListItem in returnCheckListItems)
        //    {
        //        if ((int)checkListItem != 0)
        //        {
        //            var item = Regex.Replace(((CheckListItem)checkListItem).ToString(), "(\\B[A-Z])", " $1");
        //            checkListText.text += item + Environment.NewLine;
        //        }
        //    }
        //    // Mock reports for testing
        //    returnCheckListItems.AddRange(MockReports());

        //    return returnReports;
        //}

        public CheckList CheckedList { get; set; }


        //public List<CheckList> GetCheckList()
        //{
        //    return new List<CheckList>();
        //}

    }
    public class CheckList
    {
        public CheckListItem CheckListItem { get; set; }
        public bool Checked { get; set; }
    }
}