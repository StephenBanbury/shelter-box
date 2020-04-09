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
            if (other.CompareTag("Player"))
            {
                Debug.Log("Inside Action Zone");

                AnimationManager.instance.ActivateMonitor("monitor1", "open");
                AnimationManager.instance.ActivateMonitor("monitor2", "open");
                AnimationManager.instance.ActivateMonitor("monitor3", "open");
                AnimationManager.instance.ActivateMonitor("monitor4", "open");

                //AnimateMonitor.instance.FadeMonitorText(true);

                GameManager.instance.HudOnOff(true);
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Outside Action Zone");

                AnimationManager.instance.ActivateMonitor("monitor1", "close");
                AnimationManager.instance.ActivateMonitor("monitor2", "close");
                AnimationManager.instance.ActivateMonitor("monitor3", "close");
                AnimationManager.instance.ActivateMonitor("monitor4", "close");

                //AnimateMonitor.instance.FadeMonitorText(false);

                GameManager.instance.HudOnOff(false);
            }
        }
    }
}