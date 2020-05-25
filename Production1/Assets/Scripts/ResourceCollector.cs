using System.Text.RegularExpressions;
using Com.MachineApps.PrepareAndDeploy.Enums;
using Com.MachineApps.PrepareAndDeploy.Services;
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
        //public Text GrandScoreText;

        [SerializeField] private AudioSource floorAudio;
        [SerializeField] private AudioSource requiredAudio;
        [SerializeField] private AudioSource notRequiredAudio;

        private bool isGameOver;

        void OnTriggerEnter(Collider other)
        {
            //Debug.Log($"Detected resource: {other.tag}");

            if (!isGameOver)
            {
                isGameOver = GameManager.instance.IsGameOver;
            }

            if (!isGameOver &&
                (other.CompareTag("Water") ||
                 other.CompareTag("Tent") ||
                 other.CompareTag("Food") ||
                 other.CompareTag("FirstAidKit") ||
                 other.CompareTag("Boat") ||
                 other.CompareTag("Toy"))
            )
            {
                DetectHitOrMiss(other);
            }
        }

        void DetectHitOrMiss(Collider other)
        {
            string infoMessage = "";
            int operationId = 0;
            var currentPlayer = PlayerManager.instance.GetCurrentPlayer();

            // Get suitable resources list from operation object
            // First get operation index of operation currently associated with hit box


            BoxMessage1Text.text = "";
            BoxMessage2Text.text = "";
            BoxMessage3Text.text = "";
            BoxMessage4Text.text = "";

            ScoreType scoreType = ScoreType.None;
            var scoreService = new ScoreService();
            int scoreValue;

            if (gameObject.name != "Floor")
            {
                // Then get collection of resource IDs from indexed operation
                
                Debug.Log($"{other.gameObject.name} collected!!");
                Debug.Log($"Collected by: {transform.parent.name}");

                switch (transform.parent.name)
                {
                    case "Box1":
                        operationId = OperationsManager.instance.OperationId(0);
                        break;
                    case "Box2":
                        operationId = OperationsManager.instance.OperationId(1);
                        break;
                    case "Box3":
                        operationId = OperationsManager.instance.OperationId(2);
                        break;
                    case "Box4":
                        operationId = OperationsManager.instance.OperationId(3);
                        break;
                }

                var resourceManager = other.gameObject.GetComponent<ResourceManager>();
                var myResourceId = resourceManager.MyResourceId;

                var selectedIsRequiredResource = OperationsManager.instance.SelectedResourceIsRequired(operationId, myResourceId);



                switch (selectedIsRequiredResource)
                {
                    // Resource not required
                    case TripleState.One:

                        Debug.Log("Resource not required");

                        scoreValue = scoreService.GetScoreValue(ScoreType.ItemNotRequired);
                        GameManager.instance.UpdateScore(scoreValue);
                        GameManager.instance.UpdateScoresRegister(ScoreType.ItemNotRequired);

                        infoMessage =
                            $"{Regex.Replace(((Resource) myResourceId).ToString(), "(\\B[A-Z])", " $1")} not required";
                        notRequiredAudio.Play();
                        break;

                    // Resource already collected
                    case TripleState.Two:

                        Debug.Log("Resource already collected");

                        scoreValue = scoreService.GetScoreValue(ScoreType.ItemAlreadyAssigned);
                        GameManager.instance.UpdateScore(scoreValue);
                        GameManager.instance.UpdateScoresRegister(ScoreType.ItemAlreadyAssigned);

                        infoMessage =
                            $"{currentPlayer.PlayerName}, you have already collected {Regex.Replace(((Resource) myResourceId).ToString(), "(\\B[A-Z])", " $1")}";
                        notRequiredAudio.Play();
                        break;

                    // Resource is required
                    case TripleState.Three:

                        Debug.Log($"Resource {myResourceId} is required for operationId {operationId}");

                        OperationsManager.instance.CollectResource(operationId, myResourceId);

                        // Here we need to reduce the budget by the cost of the resource
                        var resourceCost = GameManager.instance.GetResourceCost((Resource) myResourceId);

                        //Debug.Log($"Resource Cost: {resourceCost}");
                        GameManager.instance.ReduceBudget(resourceCost);

                        scoreValue = scoreService.GetScoreValue(ScoreType.ItemAssigned);
                        GameManager.instance.UpdateScore(scoreValue);
                        GameManager.instance.UpdateScoresRegister(ScoreType.ItemAssigned);

                        OperationsManager.instance.AssignOperationsToMonitors();

                        // Have all resources been collected?
                        var numberRequired = OperationsManager.instance.RequiredResources(operationId).Length;
                        var numberCollected = OperationsManager.instance.CollectedResources(operationId).Length;

                        //Debug.Log($"numberRequired: {numberRequired}; numberCollected: {numberCollected}");

                        if (numberRequired == numberCollected)
                        {
                            //ChangeMaterial(gameObject, 1);
                            //ChangeMaterial(gameObject.transform.GetChild(0).gameObject, 1);

                            scoreValue = scoreService.GetScoreValue(ScoreType.OperationSuccessful);
                            GameManager.instance.UpdateScore(scoreValue);
                            GameManager.instance.UpdateScoresRegister(ScoreType.OperationSuccessful);

                            Debug.Log($"All resources collected for operationId {operationId}");

                            //infoMessage = $"Thank you so much, {currentPlayer.PlayerName}! This deployment has been a success!";

                            OperationsManager.instance.OperationSuccessfullyDeployed(operationId);

                            GameManager.instance.PlayAudio("successfulDeployment");


                        }
                        else
                        {
                            infoMessage =
                                $"Thanks for the {Regex.Replace(((Resource) myResourceId).ToString(), "(\\B[A-Z])", " $1")}";

                            requiredAudio.Play(); // In this instance this is audio source component of the current Box GameObject
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
               //Debug.Log($"{other.gameObject.name} hit floor!!");
                floorAudio.Play(); 

                scoreValue = scoreService.GetScoreValue(ScoreType.ItemDropped);
                GameManager.instance.UpdateScore(scoreValue);
                GameManager.instance.UpdateScoresRegister(ScoreType.ItemDropped);
            }

            var resourceObjectName = other.gameObject.name.Replace("(Clone)", "");
            
            Destroy(other.gameObject);

            ResourceInstantiator.instance.CreateResourceObject(resourceObjectName, true);
        }
    }
}
