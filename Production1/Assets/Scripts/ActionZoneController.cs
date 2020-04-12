using System.Collections;
using System.Collections.Generic;
using Com.MachineApps.PrepareAndDeploy;
using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class ActionZoneController : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (gameObject.name == "ActionZone")
            {
                if (other.CompareTag("Player"))
                {
                    Debug.Log("Inside Action Zone");

                    AnimationManager.instance.ActivateMonitor("monitor1", true);
                    AnimationManager.instance.ActivateMonitor("monitor2", true);
                    AnimationManager.instance.ActivateMonitor("monitor3", true);
                    AnimationManager.instance.ActivateMonitor("monitor4", true);

                    GameManager.instance.HudOnOff(true);
                }
            }
            //}else if (gameObject.name == "EntranceActionZone")
            //{
            //    Debug.Log("Inside Entrance EntranceActionZone");
            //    AnimationManager.instance.RaiseCentrePartition(true);
        }
        void OnTriggerExit(Collider other)
        {
            if (gameObject.name == "ActionZone")
            {
                if (other.CompareTag("Player"))
                {
                    Debug.Log("Outside Action Zone");

                    //AnimationManager.instance.ActivateMonitor("monitor1", false);
                    //AnimationManager.instance.ActivateMonitor("monitor2", false);
                    //AnimationManager.instance.ActivateMonitor("monitor3", false);
                    //AnimationManager.instance.ActivateMonitor("monitor4", false);

                    GameManager.instance.HudOnOff(false);
                }
            }
            //else if (gameObject.name == "EntranceActionZone")
            //{
            //    Debug.Log("Outside Entrance EntranceActionZone");
            //    AnimationManager.instance.RaiseCentrePartition(false);
            //}
        }
    }
}