using System;
using Com.MachineApps.PrepareAndDeploy.Enums;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class ResourceManager : MonoBehaviour
    {
        public int myResourceId;

        [Tooltip("Resource grabb countdown timer text")]
        //[SerializeField]
        public Text countdownDisplay;

        public float grabbedCountdown;
        private bool grabbedCountdownStarted;
        private bool updatingEvent;

        void Start()
        {
            //if (gameObject.CompareTag("Cassette"))
            //{
            //    var randomResource = RandomResource();
            //    myResourceId = (int) randomResource;

            //    var displayText = Regex.Replace(randomResource.ToString(), "(\\B[A-Z])", " $1");
            //    cassetteText.text = displayText;
            //}
            //else 

            if (gameObject.CompareTag("Tent"))
            {
                myResourceId = (int)Resource.Tents;
            }
            else if (gameObject.CompareTag("Water"))
            {
                myResourceId = (int)Resource.Water;
            }
            else if (gameObject.CompareTag("Food"))
            {
                myResourceId = (int)Resource.Food;
            }
            else if (gameObject.CompareTag("FirstAidKit"))
            {
                myResourceId = (int)Resource.FirstAidKits;
            }
            else if (gameObject.CompareTag("Boat"))
            {
                myResourceId = (int)Resource.Boats;
            }
            else if (gameObject.CompareTag("Toy"))
            {
                myResourceId = (int)Resource.Toys;
            }
        }


        void FixedUpdate()
        {
            if (grabbedCountdownStarted)
            {
                grabbedCountdown -= Time.deltaTime;
                float seconds = grabbedCountdown % 60;
                countdownDisplay.text = $"{seconds:00}";
                
                if (grabbedCountdown <= 0)
                {
                    grabbedCountdown = 10;
                    var resourceObjectName = gameObject.gameObject.name.Replace("(Clone)", "");
                    Destroy(gameObject);
                    ResourceInstantiator.instance.CreateResourceObject(resourceObjectName);
                }
            }
        }

        public void Grabbed(bool grabState)
        {
            Debug.Log($"I've been grabbed: {gameObject.name}");

            var resourceTextObject = gameObject.GetComponentInChildren<Text>();
            if (resourceTextObject != null)
            {
                if (grabState)
                {
                    resourceTextObject.text ="Gotcha!";
                    grabbedCountdown = 10;
                    grabbedCountdownStarted = true;
                }
                else
                {
                    resourceTextObject.text = "Info";
                    grabbedCountdownStarted = false;
                }
            }
        }

        //private void CountdownEvent(int regularity)
        //{
        //    //Debug.Log($"updatingFundRaisingEvent { updatingFundRaisingEvent } { Math.Floor(seconds) % 10 }");

        //    //float minutes = Mathf.Floor(countdown / 60);
        //    float seconds = grabbedCountdown % 60;
        //    countdownDisplay.text = $"{seconds:00}";

        //    if (Math.Floor(seconds) % regularity == 0 && !updatingEvent)
        //    {
        //        FundRaisingEventManager.instance.NextEvent();
        //        updatingEvent = true;
        //    }
        //    else if (Math.Floor(seconds) % 10 > 0)
        //    {
        //        updatingEvent = false;
        //    }
        //}

        private Resource RandomResource()
        {
            Array values = Enum.GetValues(typeof(Resource));
            Random random = new Random();
            Resource randomValue = (Resource)values.GetValue(random.Next(1, values.Length - 1));
            return randomValue;
        }

    }

}