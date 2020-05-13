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
                    
                    GameManager.instance.ScorePanelOnOff(true);
                    GameManager.instance.BudgetLivesOnOff(true);
                    GameManager.instance.UpdateFundingEventLives();
                    GameManager.instance.UpdateBudgetDisplay();
                    GameManager.instance.CurrentOpsChartShowHide(true);
                    GameManager.instance.FundraisingEventsChartShowHide(true);
                    GameManager.instance.ActivateExitBlocker(true);

                    OperationsManager.instance.SetRotateOperations(true);

                    LightingManager.instance.BoxLightsFade(true, 4, 0, 7f);
                    LightingManager.instance.OverheadLightsFade(true, 4, 0, 3f);
                    LightingManager.instance.ScoreboardLightsFade(true, 4, 0, 4.5f);
                    LightingManager.instance.ComputerLightsFade(true, 4, 0, 6.5f);
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

                    //AnimationManager.instance.BoxesThruFloor(false);

                    //GameManager.instance.HudOnOff(false);

                    //GameManager.instance.ScorePanelOnOff(false);

                    //OperationsManager.instance.SetRotateOperations(false);

                    //var currentOps = GameObject.Find("CurrentOperations");
                    //currentOps.GetComponent<CanvasGroup>().alpha = 0;


                    LightingManager.instance.BoxLightsFade(false, 3, 0, 7f);
                    LightingManager.instance.OverheadLightsFade(false, 3, 0, 3f);
                    LightingManager.instance.ScoreboardLightsFade(false, 3, 0, 4.5f);
                    LightingManager.instance.ComputerLightsFade(false, 3, 0, 6.5f);

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