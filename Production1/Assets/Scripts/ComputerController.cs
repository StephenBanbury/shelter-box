using System.Collections;
using System.Linq;
using Com.MachineApps.PrepareAndDeploy.Enums;
using Com.MachineApps.PrepareAndDeploy.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class ComputerController : MonoBehaviour
    {
        public AudioSource audio1;

        //private FundRaisingEvent fundRaisingEvent;
        private bool processingFundingEvent = false;

        void OnTriggerEnter(Collider other)
        {
            var used = FundRaisingEventManager.instance.NumberOfEventsUsed;
            var allowed = FundRaisingEventManager.instance.NumberOfEventsAllowed;

            //Debug.Log($"numberOfEventsAllowed: {allowed}, numberOfEventsUsed: {used}");

            if (other.CompareTag("Hand") && !processingFundingEvent && used < allowed)
            {
                StartCoroutine(AwaitFundingEventResults(7, other.name));
            }
        }

        private void ShowMessageOnScreen(string message)
        {
            var screenText = GameObject.Find("ScreenText");
            screenText.GetComponent<Text>().text = message;
        }

        private IEnumerator AwaitFundingEventResults(int waitFor, string handName)
        {
            processingFundingEvent = true;

            var fundRaisingEvents = FundRaisingEventManager.instance.FundRaisingEvents;
            var displayedEventId = FundRaisingEventManager.instance.DisplayedEventId();

            if (displayedEventId != null)
            {
                var selectedEvent = fundRaisingEvents.FirstOrDefault(e => e.Id == displayedEventId);

                Debug.Log($"selectedEvent: {selectedEvent.Title} (Id={displayedEventId})");

                GameManager.instance.HudMessage($"You selected {selectedEvent.Title}", 6);
                ShowMessageOnScreen($"You selected {selectedEvent.Title}");

                audio1 = GetComponent<AudioSource>();
                audio1.Play();

                VibrationManager.instance.TriggerVibration(audio1.clip, handName == "LHandCollider"
                    ? OVRInput.Controller.LTouch
                    : OVRInput.Controller.RTouch);

                FundRaisingEventManager.instance.MarkEvent(displayedEventId, FundRaisingEventStatus.Current);

                // Time delay
                yield return new WaitForSeconds(waitFor);

                //FundRaisingEventManager.instance.MarkEventAsUsed(currentEventId);

                audio1.Play(); // TODO different sound

                // Amount raised = estimated +/- 30%
                var estimated = selectedEvent.EstimatedFundsRaised;
                var rand = Random.Range(-estimated / 3, estimated / 3);

                var amountRaised = estimated + rand;

                Debug.Log($"Estimated: {estimated}, Random: {rand}, Amount made: {amountRaised}");

                GameManager.instance.IncreaseBudget(amountRaised);
                GameManager.instance.UpdateBudgetDisplay();
                GameManager.instance.HudMessage($"You made £{amountRaised}!", 5);

                FundRaisingEventManager.instance.MarkEvent(displayedEventId, FundRaisingEventStatus.Completed);
            }

            processingFundingEvent = false;
        }
    }
}