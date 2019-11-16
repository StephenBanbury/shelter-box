using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts
{
    public class CheckListCollector : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hand"))
            {
                var item = CheckListItem.None;

                switch (gameObject.name)
                {
                    case "CollectFirstAidKit":
                        item = CheckListItem.FirstAidKit;
                        break;
                    case "CollectPassport":
                        item = CheckListItem.Passport;
                        break;
                }

                CheckListManager.instance.UpdateCollectedList(item);
                CheckListManager.instance.UpdateCheckListText();
            }
        }
    }
}