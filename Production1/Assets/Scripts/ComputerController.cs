using System.Collections;
using System.Linq;
using Com.MachineApps.PrepareAndDeploy.Models;
using UnityEngine;

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
                StartCoroutine(AwaitFundingEventResults());
            }
        }

        private IEnumerator AwaitFundingEventResults()
        {
            processingFundingEvent = true;

            var fundRaisingEvents = FundRaisingEventManager.fundRaisingEvents;
            var currentEventId = FundRaisingEventManager.instance.CurrentEventId();

            fundRaisingEvent = fundRaisingEvents.FirstOrDefault(e => e.Id == currentEventId);

            GameManager.instance.HudMessage($"You selected {fundRaisingEvent.Title}", 6);

            audio1 = GetComponent<AudioSource>();
            audio1.Play();

            VibrationManager.instance.TriggerVibration(audio1.clip, OVRInput.Controller.RTouch);

            yield return new WaitForSeconds(10);

            FundRaisingEventManager.instance.MarkEventAsUsed(currentEventId);

            audio1.Play(); // TODO different sound

            // TODO pseudo random value
            var estimated = fundRaisingEvent.EstimatedFundsRaised;

            var rand = Random.Range(-estimated/3, estimated/3);

            var amountMade = estimated + rand;

            Debug.Log($"Estimated: {estimated}, Random: {rand}, Amount made: {amountMade}");

            GameManager.instance.IncreaseBudget(amountMade);
            GameManager.instance.UpdateBudgetDisplay();

            GameManager.instance.HudMessage($"You made £{amountMade}!", 5);

            processingFundingEvent = false;
        }
    }
}