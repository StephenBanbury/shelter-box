using System.Text.RegularExpressions;
using Com.MachineApps.PrepareAndDeploy.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class ResourceCollector : MonoBehaviour
    {
        public Text BoxMessage1Text;
        public Text BoxMessage2Text;
        public Text BoxMessage3Text;
        public Text BoxMessage4Text;
        public Text GrandScoreText;

        private AudioSource audioSource1;
        private AudioSource audioSource2;
        private AudioSource audioSource3;

        private bool noMoneyLeft;
        void Start()
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSource1 = audioSources[0]; // Audio source = if this is a box then it's is a beep; if it's the floor then it's a thud
            if (audioSources.Length > 1)
            {
                audioSource2 = audioSources[1];
                audioSource3 = audioSources[2];
            }
        }

        void OnTriggerEnter(Collider other)
        {
            //Debug.Log($"Detected resource: {other.tag}");

            if (other.CompareTag("Water") || 
                other.CompareTag("Tent") || 
                other.CompareTag("Food") ||
                other.CompareTag("FirstAidKit") ||
                other.CompareTag("Boat") ||
                other.CompareTag("Toy")
                )
            {
                DetectHitOrMiss(other);
            }
        }

        void DetectHitOrMiss(Collider other)
        {
            string infoMessage = "";

            int reportId = 0;

            // Get suitable resources list from report object
            // First get report index of report currently associated with hit box

            //Debug.Log($"transform.parent.name: {transform.parent.name}");

            BoxMessage1Text.text = "";
            BoxMessage2Text.text = "";
            BoxMessage3Text.text = "";
            BoxMessage4Text.text = "";

            if (gameObject.name != "Floor")
            {
                // Then get collection of resource IDs from indexed report
                
                Debug.Log($"{other.gameObject.name} collected!!");

                switch (transform.parent.name)
                {
                    case "Box1":
                        reportId = ReportsManager.instance.reportId0;
                        break;
                    case "Box2":
                        reportId = ReportsManager.instance.reportId1;
                        break;
                    case "Box3":
                        reportId = ReportsManager.instance.reportId2;
                        break;
                    case "Box4":
                        reportId = ReportsManager.instance.reportId3;
                        break;
                }

                var resourceManager = other.gameObject.GetComponent<ResourceManager>();
                var myResourceId = resourceManager.myResourceId;

                var selectedIsRequiredResource = ReportsManager.instance.SelectedResourceIsRequired(reportId, myResourceId);

                switch (selectedIsRequiredResource)
                {
                    // Resource not required
                    case TripleState.One:

                        //Debug.Log("Resource not required");

                        infoMessage =
                            $"{Regex.Replace(((Resource) myResourceId).ToString(), "(\\B[A-Z])", " $1")} not required";
                        audioSource2.Play();
                        break;

                    // Resource already collected
                    case TripleState.Two:

                        //Debug.Log("Resource already collected");

                        infoMessage =
                            $"You have already collected {Regex.Replace(((Resource) myResourceId).ToString(), "(\\B[A-Z])", " $1")}";
                        audioSource2.Play();
                        break;

                    // Resource is required
                    case TripleState.Three:

                        //Debug.Log("Resource is required");

                        ReportsManager.instance.CollectResource(reportId, myResourceId);

                        // Here we need to reduce the budget by the cost of the resource
                        var resourceCost = GameManager.instance.GetResourceCost((Resource) myResourceId);
                        var budgetRemaining = GameManager.instance.BudgetRemaining;

                        if (budgetRemaining - resourceCost <= 0)
                        {
                            // Let user now they need more funds
                            Debug.Log("Budget all used up!");
                            noMoneyLeft = true;
                        }

                        //Debug.Log($"Resource Cost: {resourceCost}");
                        GameManager.instance.ReduceBudget(resourceCost);
                        ReportsManager.instance.AssignReportsToMonitors();


                        // Have all resources been collected?
                        var numberRequired = ReportsManager.instance.RequiredResources(reportId).Length;
                        var numberCollected = ReportsManager.instance.CollectedResources(reportId).Length;

                        if (numberRequired == numberCollected)
                        {
                            //ChangeMaterial(gameObject, 1);
                            //ChangeMaterial(gameObject.transform.GetChild(0).gameObject, 1);

                            Debug.Log($"All resources collected for reportId {reportId}");

                            infoMessage = "Congratulations! You have collected everything.";

                            ReportsManager.instance.PlayCongratulationsVideo(reportId);

                            audioSource3.Play();
                        }
                        else
                        {
                            infoMessage =
                                $"Thanks for the {Regex.Replace(((Resource) myResourceId).ToString(), "(\\B[A-Z])", " $1")}";

                            audioSource1
                                .Play(); // In this instance this is audio source component of the current Box GameObject
                        }

                        if (noMoneyLeft)
                        {
                            GameManager.instance.HudMessage(
                                "You have used your entire budget! Stage a fundraising event to increase your available funds.",
                                10);
                        }

                        break;
                }

                switch (transform.parent.name)
                {
                    case "Box1":
                        BoxMessage1Text.text = infoMessage;
                        break;
                    case "Box2":
                        BoxMessage2Text.text = infoMessage;
                        break;
                    case "Box3":
                        BoxMessage3Text.text = infoMessage;
                        break;
                    case "Box4":
                        BoxMessage4Text.text = infoMessage;
                        break;
                }
            }
            else
            { 
                Debug.Log($"{other.gameObject.name} hit floor!!");
                audioSource1.Play(); // In this instance this is audio source component of the Floor GameObject
            }

            var resourceObjectName = other.gameObject.name.Replace("(Clone)", "");
            
            Destroy(other.gameObject);

            ResourceInstantiator.instance.CreateResourceObject(resourceObjectName);
        }

        private void ChangeMaterial(GameObject gameObjectToAffect, int matIndex)
        {
            var mats = gameObjectToAffect.GetComponent<Renderer>().materials;
            var newMat = mats[matIndex];

            mats[0] = newMat;
            gameObjectToAffect.GetComponent<Renderer>().materials = mats;
        }
    }
}
