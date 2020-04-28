using System;
using System.Linq;
using Com.MachineApps.PrepareAndDeploy.Enums;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class ResourceManager : MonoBehaviour
    {

        [Tooltip("Resource grabber countdown timer text")]
        [SerializeField]
        private Text countdownDisplay;
        [SerializeField]
        private Canvas priceTag;

        private int myResourceId = 0;
        private float countdown;
        private bool countdownStarted;

        public int MyResourceId => myResourceId;

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
                myResourceId = (int)Resource.FirstAid;
            }
            else if (gameObject.CompareTag("Boat"))
            {
                myResourceId = (int)Resource.Boats;
            }
            else if (gameObject.CompareTag("Toy"))
            {
                myResourceId = (int)Resource.Toys;
            }

            if (myResourceId != 0)
            {
                Debug.Log($"ResourceManager Start - Name/Tag: {gameObject.name}/{gameObject.tag}");

                var priceText = gameObject.GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "PriceText");

                var resourceCost = GameManager.instance.GetResourceCost((Resource)myResourceId);
                priceText.text = $"£{resourceCost.ToString()}";

                //var countdownText = gameObject.GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "CountdownText");
                countdownDisplay.text = "";
                countdown = GameManager.instance.initialResourceObjectCountdown;
            }

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
                    ResetResourceObject(true);
                }
            }
        }

        public void Grabbed(bool grabState)
        {
            //Debug.Log($"I've been grabbed: {gameObject.name}");
            if (grabState)
            {
                if (GameManager.instance.BudgetRemaining() <= 0)
                {
                    GameManager.instance.HudMessage("You do not have any funds left!", 3);
                    ResetResourceObject(false);
                }
                else
                {
                    countdown = GameManager.instance.initialResourceObjectCountdown;
                    countdownStarted = true;
                    priceTag.gameObject.SetActive(false);
                }
            }
            else
            {
                countdownDisplay.text = "";
                countdownStarted = false;
            }
        }

        private void ResetResourceObject(bool dropFromHeight)
        {
            var resourceObjectName = gameObject.gameObject.name.Replace("(Clone)", "");
            Destroy(gameObject);
            ResourceInstantiator.instance.CreateResourceObject(resourceObjectName, dropFromHeight);
        }
    }

}