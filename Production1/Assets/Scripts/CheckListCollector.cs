using Com.MachineApps.PrepareAndDeploy;
using Com.MachineApps.PrepareAndDeploy.Enums;
using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
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
                    case "CollectVisa":
                        item = CheckListItem.Visa;
                        break;
                    case "CollectVaccinations":
                        item = CheckListItem.Vaccinations;
                        break;
                    case "CollectMobilePhone":
                        item = CheckListItem.MobilePhone;
                        break;
                    case "CollectBatteryCharger":
                        item = CheckListItem.BatteryCharger;
                        break;
                    case "CollectSunBlock":
                        item = CheckListItem.SunBlock;
                        break;
                }

                var collectedSound = GetComponent<AudioSource>();
                collectedSound.Play();

                gameObject.GetComponent<Renderer>().material.color = new Color(0, 196, 255, 255);

                CheckListManager.instance.UpdateCollectedList(item);
                CheckListManager.instance.UpdateCheckListText();
                CheckListManager.instance.CheckIfAllCollected();
            }
        }
    }
}