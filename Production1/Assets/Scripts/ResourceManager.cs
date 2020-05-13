using System.Collections;
using System.Linq;
using Com.MachineApps.PrepareAndDeploy.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class ResourceManager : MonoBehaviour
    {

        [Tooltip("Resource grabber countdown timer text")]
        [SerializeField] private Text countdownDisplay;
        [Tooltip("Switch on/off the use of resource grabber countdown")]
        [SerializeField] private bool useResourceGrabCountdown;
        [Tooltip("Initial countdown setting for resource objects (seconds)")]
        [SerializeField] private int initialResourceObjectCountdown = 20;
        [SerializeField] private Canvas priceTag;
        private float countdown;
        private bool countdownStarted;

        public int MyResourceId { get; private set; } = 0;

        void Start()
        {

            if (gameObject.CompareTag("Tent"))
            {
                MyResourceId = (int)Resource.Tents;
            }
            else if (gameObject.CompareTag("Water"))
            {
                MyResourceId = (int)Resource.Water;
            }
            else if (gameObject.CompareTag("Food"))
            {
                MyResourceId = (int)Resource.Food;
            }
            else if (gameObject.CompareTag("FirstAidKit"))
            {
                MyResourceId = (int)Resource.FirstAid;
            }
            else if (gameObject.CompareTag("Boat"))
            {
                MyResourceId = (int)Resource.Boats;
            }
            else if (gameObject.CompareTag("Toy"))
            {
                MyResourceId = (int)Resource.Toys;
            }

            if (MyResourceId != 0)
            {
                //Debug.Log($"ResourceManager Start - Name/Tag: {gameObject.name}/{gameObject.tag}");

                var priceText = gameObject.GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "PriceText");

                var resourceCost = GameManager.instance.GetResourceCost((Resource)MyResourceId);
                priceText.text = $"£{resourceCost.ToString()}";

                //var countdownText = gameObject.GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "CountdownText");
                countdownDisplay.text = "";
                countdown = initialResourceObjectCountdown;
            }

        }

        void FixedUpdate()
        {
            StartCoroutine(ResourceGrabbedCountdown());
        }

        private IEnumerator ResourceGrabbedCountdown()
        {
            if (countdownStarted && useResourceGrabCountdown)
            {
                countdown -= Time.deltaTime;
                float seconds = countdown % 60;
                countdownDisplay.text = $"{seconds:00}";

                if (countdown <= 0)
                {
                    ResetResourceObject(true);
                }
            }

            yield return new WaitForSeconds(0.1f);
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
                    countdown = initialResourceObjectCountdown;
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