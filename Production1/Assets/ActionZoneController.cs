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

                AnimateMonitor.instance.ActivateMonitor("monitor1", "open");
                AnimateMonitor.instance.ActivateMonitor("monitor2", "open");
                AnimateMonitor.instance.ActivateMonitor("monitor3", "open");
                AnimateMonitor.instance.ActivateMonitor("monitor4", "open");

                //AnimateMonitor.instance.FadeMonitorText(true);
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Outside Action Zone");

                AnimateMonitor.instance.ActivateMonitor("monitor1", "close");
                AnimateMonitor.instance.ActivateMonitor("monitor2", "close");
                AnimateMonitor.instance.ActivateMonitor("monitor3", "close");
                AnimateMonitor.instance.ActivateMonitor("monitor4", "close");

                //AnimateMonitor.instance.FadeMonitorText(false);
            }
        }
    }
}