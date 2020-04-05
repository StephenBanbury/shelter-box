using System.Collections;
using Com.MachineApps.PrepareAndDeploy.Models;
using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class ComputerController : MonoBehaviour
    {
        public AudioSource audio1;

        //private List<FundRaisingEvent> fundRaisingEvents;
        private FundRaisingEvent fundRaisingEvent;
        private int currentIndex;
        private bool processingFundingEvent = false;

        private Vector3 handTransform;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hand") && !processingFundingEvent)
            {
                // Prevent hand moving any further until hand comes back out again.
                //var parent = other.GetComponentInParent<Rigidbody>();

                //handTransform = parent.position;

                //Debug.Log($"Hand Vector3: {handTransform}");

                StartCoroutine(AwaitFundingEventResults());
            }
        }

        //void OnTriggerStay(Collider other)
        //{
        //    //if (other.attachedRigidbody)
        //    //{
        //    //    other.attachedRigidbody.AddForce(Vector3.up * 10);
        //    //}

        //    Debug.Log("OnTriggerStay");

        //    if (other.CompareTag("Hand")) // && processingFundingEvent)
        //    {

        //        var parent = other.GetComponentInParent<Rigidbody>();
               
        //        Debug.Log($"handTransform: {parent.position}");

        //        parent.position = handTransform;


        //        Debug.Log($"new handTransform: {parent.position}");
        //    }

        //}

        private IEnumerator AwaitFundingEventResults()
        {
            processingFundingEvent = true;

            currentIndex = FundRaisingEventManager.currentIndex;

            var fundRaisingEvents = FundRaisingEventManager.fundRaisingEvents;
            fundRaisingEvent = fundRaisingEvents[currentIndex];

            GameManager.instance.HudMessage($"You selected {fundRaisingEvent.Title}", 4);

            audio1 = GetComponent<AudioSource>();
            audio1.Play();

            VibrationManager.instance.TriggerVibration(audio1.clip, OVRInput.Controller.RTouch);

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