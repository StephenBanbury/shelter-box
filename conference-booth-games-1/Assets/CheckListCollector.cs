using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

public class CheckListCollector : MonoBehaviour
{

    private List<CheckListItem> collectedCheckListItems;
    public Text checkListText;

    private CheckListCollector()
    {
        //checkListService = new CheckListService();
        collectedCheckListItems = new List<CheckListItem>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            // TODO
            switch (gameObject.name)
            {
                case "CollectFirstAidKit":
                    collectedCheckListItems.Add(CheckListItem.FirstAidKit);
                    break;
                case "CollectPassport":
                    collectedCheckListItems.Add(CheckListItem.Passport);
                    break;
            }

            //CheckListManager.UpdateCheckListText();

            checkListText.text = gameObject.name;
        }
    }
}
