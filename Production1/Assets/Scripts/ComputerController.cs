using System.Collections;
using System.Collections.Generic;
using Com.MachineApps.PrepareAndDeploy.Models;
using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class ComputerController : MonoBehaviour
    {
        //private List<FundRaisingEvent> fundRaisingEvents;
        private FundRaisingEvent fundRaisingEvent;
        private int currentIndex;
        public AudioSource audio1;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hand"))
            {
                StartCoroutine(AwaitFundingEventResults());
            }
        }

        private IEnumerator AwaitFundingEventResults()
        {
            currentIndex = FundRaisingEventManager.currentIndex;

            Debug.Log($"Event selected: {currentIndex}");

            var fundRaisingEvents = FundRaisingEventManager.fundRaisingEvents;

            Debug.Log($"fundRaisingEvents: {fundRaisingEvents.Count}");

            fundRaisingEvent = fundRaisingEvents[currentIndex];
            Debug.Log($"fundRaisingEvent: {fundRaisingEvent.Title}");

            GameManager.instance.HudMessage($"You selected {fundRaisingEvent.Title}", 4);
            Debug.Log($"You selected {fundRaisingEvent.Title}");


            audio1 = GetComponent<AudioSource>();
            audio1.Play();


            yield return new WaitForSeconds(10);

            //var budgetRemaining = GameManager.instance.BudgetRemaining;

            audio1.Play(); // TODO different sound

            // TODO pseudo random value
            var amountMade = fundRaisingEvents[currentIndex].EstimatedFundsRaised;

            GameManager.instance.IncreaseBudget(amountMade);
            GameManager.instance.UpdateBudgetDisplay();

            GameManager.instance.HudMessage($"You made £{amountMade}!", 4);
        }
    }
}