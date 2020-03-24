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

        [Tooltip("Resource grabber countdown timer text")]

        //[SerializeField]
        public Text countdownDisplay;

        private float countdown;
        private bool countdownStarted;

        void Start()
        {
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

            countdown = GameManager.instance.initialResourceObjectCountdown;
        }

        void FixedUpdate()
        {
            if (countdownStarted)
            {
                countdown -= Time.deltaTime;
                float seconds = countdown % 60;
                countdownDisplay.text = $"{seconds:00}";
                
                if (countdown <= 0)
                {
                    var resourceObjectName = gameObject.gameObject.name.Replace("(Clone)", "");
                    Destroy(gameObject);
                    ResourceInstantiator.instance.CreateResourceObject(resourceObjectName);
                }
            }
        }

        public void Grabbed(bool grabState)
        {
            //Debug.Log($"I've been grabbed: {gameObject.name}");

            var resourceTextObject = gameObject.GetComponentInChildren<Text>();
            if (resourceTextObject != null)
            {
                if (grabState)
                {
                    //resourceTextObject.text ="Gotcha!";
                    countdown = GameManager.instance.initialResourceObjectCountdown;
                    countdownStarted = true;
                }
                else
                {
                    //resourceTextObject.text = "Info";
                    countdownStarted = false;
                }
            }
        }

        //private Resource RandomResource()
        //{
        //    Array values = Enum.GetValues(typeof(Resource));
        //    Random random = new Random();
        //    Resource randomValue = (Resource)values.GetValue(random.Next(1, values.Length - 1));
        //    return randomValue;
        //}

    }

}