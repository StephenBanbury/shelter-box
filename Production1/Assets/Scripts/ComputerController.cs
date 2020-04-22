using System.Collections;
using System.Linq;
using Com.MachineApps.PrepareAndDeploy.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class ComputerController : MonoBehaviour
    {
        public AudioSource audio1;

        private FundRaisingEvent fundRaisingEvent;
        private bool processingFundingEvent = false;

        void OnTriggerEnter(Collider other)
        {
            var used = FundRaisingEventManager.instance.numberOfEventsUsed;
            var allowed = FundRaisingEventManager.instance.numberOfEventsAllowed;

            //Debug.Log($"numberOfEventsAllowed: {allowed}, numberOfEventsUsed: {used}");

            if (other.CompareTag("Hand") && !processingFundingEvent && used <= allowed)
            {
                StartCoroutine(AwaitFundingEventResults(7));
            }
        }

        private void ShowMessageOnScreen(string message)
        {
            var screenText = GameObject.Find("ScreenText");
            screenText.GetComponent<Text>().text = message;
        }

        private IEnumerator AwaitFundingEventResults(int waitFor)
        {
            processingFundingEvent = true;

            var fundRaisingEvents = FundRaisingEventManager.fundRaisingEvents;
            var currentEventId = FundRaisingEventManager.instance.CurrentEventId();

            fundRaisingEvent = fundRaisingEvents.FirstOrDefault(e => e.Id == currentEventId);

            GameManager.instance.HudMessage($"You selected {fundRaisingEvent.Title}", 6);
            ShowMessageOnScreen($"You selected {fundRaisingEvent.Title}");

            audio1 = GetComponent<AudioSource>();
            audio1.Play();

            VibrationManager.instance.TriggerVibration(audio1.clip, OVRInput.Controller.RTouch);

            FundRaisingEventManager.instance.MarkEventAsUsed(currentEventId);

            // Time delay
            yield return new WaitForSeconds(waitFor);

            //FundRaisingEventManager.instance.MarkEventAsUsed(currentEventId);

            audio1.Play(); // TODO different sound

            // Amount raised = estimated +/- 30%
            var estimated = fundRaisingEvent.EstimatedFundsRaised;
            var rand = Random.Range(-estimated/3, estimated/3);

            var amountRaised = estimated + rand;

            Debug.Log($"Estimated: {estimated}, Random: {rand}, Amount made: {amountRaised}");

            GameManager.instance.IncreaseBudget(amountRaised);
            GameManager.instance.UpdateBudgetDisplay();
            GameManager.instance.HudMessage($"You made £{amountRaised}!", 5);

            processingFundingEvent = false;
        }
    }
}