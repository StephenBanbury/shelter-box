using System.Collections;
using Com.MachineApps.PrepareAndDeploy.Models;
using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class ComputerController : MonoBehaviour
    {
        //private List<FundRaisingEvent> fundRaisingEvents;
        private FundRaisingEvent fundRaisingEvent;
        private int currentIndex;
        private bool processingFundingEvent = false;
        public AudioSource audio1;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hand") && !processingFundingEvent)
            {
                StartCoroutine(AwaitFundingEventResults());
            }
        }

        private IEnumerator AwaitFundingEventResults()
        {
            processingFundingEvent = true;

            currentIndex = FundRaisingEventManager.currentIndex;

            var fundRaisingEvents = FundRaisingEventManager.fundRaisingEvents;
            fundRaisingEvent = fundRaisingEvents[currentIndex];

            GameManager.instance.HudMessage($"You selected {fundRaisingEvent.Title}", 4);

            audio1 = GetComponent<AudioSource>();
            audio1.Play();
            
            yield return new WaitForSeconds(10);

            audio1.Play(); // TODO different sound

            // TODO pseudo random value
            var amountMade = fundRaisingEvents[currentIndex].EstimatedFundsRaised;

            GameManager.instance.IncreaseBudget(amountMade);
            GameManager.instance.UpdateBudgetDisplay();

            GameManager.instance.HudMessage($"You made £{amountMade}!", 4);

            processingFundingEvent = false;
        }
    }
}