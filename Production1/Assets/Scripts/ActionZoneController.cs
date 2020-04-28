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

                    AnimationManager.instance.ActivateMonitor("Monitor1", true);
                    AnimationManager.instance.ActivateMonitor("Monitor2", true);
                    AnimationManager.instance.ActivateMonitor("Monitor3", true);
                    AnimationManager.instance.ActivateMonitor("Monitor4", true);

                    AnimationManager.instance.BoxesThruFloor(true);

                    GameManager.instance.HudOnOff(true);

                    //OperationsManager.instance.SetRotateOperations(true);

                    var currentOps = GameObject.Find("CurrentOperations");
                    currentOps.GetComponent<CanvasGroup>().alpha = 1f;
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

                    AnimationManager.instance.BoxesThruFloor(false);

                    GameManager.instance.HudOnOff(false);

                    OperationsManager.instance.SetRotateOperations(false);

                    var currentOps = GameObject.Find("CurrentOperations");
                    currentOps.GetComponent<CanvasGroup>().alpha = 0;
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