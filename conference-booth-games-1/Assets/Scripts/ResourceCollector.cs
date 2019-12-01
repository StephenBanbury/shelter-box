using System.Text.RegularExpressions;
using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ResourceCollector : MonoBehaviour
    {
        public Text BoxMessage1Text;
        public Text BoxMessage2Text;
        public Text BoxMessage3Text;
        public Text BoxMessage4Text;
        public Text grandScoreText;

        private AudioSource audioSource1;
        private AudioSource audioSource2;

        //public int grandScore;

        void Start()
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSource1 = audioSources[0]; // Audio source = if this is a box then it's is a beep; if it's the floor then it's a thud
            audioSource2 = audioSources[1];

            ChangeMaterial(gameObject, 0);
            ChangeMaterial(gameObject.transform.GetChild(0).gameObject, 0);
        }

        void OnTriggerEnter(Collider other)
        {
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
            bool hitFloor = false;

            // Get suitable resources list from report object
            // First get report index of report currently associated with hit box

            switch (gameObject.name)
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
                case "Floor":
                    hitFloor = true;
                    break;
            }
            
            BoxMessage1Text.text = "";
            BoxMessage2Text.text = "";
            BoxMessage3Text.text = "";
            BoxMessage4Text.text = "";

            // Then get collection of resource IDs from indexed report
            if (!hitFloor)
            {
                var resourceManager = other.gameObject.GetComponent<ResourceManager>();
                var myResourceId = resourceManager.myResourceId;

                var selectedIsRequiredResource = ReportsManager.instance.SelectedResourceIsRequired(reportId, myResourceId);

                switch (selectedIsRequiredResource)
                {
                    // Resource not required
                    case TripleState.One:
                        infoMessage =
                            $"{Regex.Replace(((Resource) myResourceId).ToString(), "(\\B[A-Z])", " $1")} not required";
                        audioSource2.Play();
                        break;

                    // Resource already collected
                    case TripleState.Two:
                        infoMessage =
                            $"You have already collected {Regex.Replace(((Resource) myResourceId).ToString(), "(\\B[A-Z])", " $1")}";
                        audioSource2.Play();
                        break;

                    // Resource required
                    case TripleState.Three:
                        ReportsManager.instance.CollectResource(reportId, myResourceId);
                        ReportsManager.instance.AssignReportsToMonitors();

                        infoMessage =
                            $"Thanks for the {Regex.Replace(((Resource) myResourceId).ToString(), "(\\B[A-Z])", " $1")}";

                        // Have all resources been collected?
                        var required = ReportsManager.instance.RequiredResources(reportId).Length;
                        var collected = ReportsManager.instance.CollectedResources(reportId).Length;


                        //grandScoreText.text = $"{required == collected} - {required} : {collected}";

                        if (required == collected)
                        {
                            ChangeMaterial(gameObject, 1);
                            ChangeMaterial(gameObject.transform.GetChild(0).gameObject, 1);

                            GameManager.instance.UpdateDeploymentStatus(1);

                            grandScoreText.text = "Well done! Now collect your personal items.";

                            if (GameManager.instance.GetDeploymentStatus() != DeploymentStatus.Green)
                            {
                                audioSource1.Play();
                            }
                            
                            // TODO Destroy all existing resource objects and stop producing new ones
                        }
                        else
                        {
                            //grandScoreText.text = "floor";
                            audioSource1.Play(); // In this instance this is audio source component of the current Box GameObject
                        }

                        break;
                }
                
                //grandScoreText.text = $"TripleState: {selectedIsRequiredResource.ToString()}";

                switch (gameObject.name)
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
                audioSource1.Play(); // In this instance this is audio source component of the Floor GameObject
            }

            //grandScoreText.text = $"Score: {grandScore.ToString("0")}";

            Destroy(other.gameObject);

            ResourceInstantiator.instance.CreateResourceObject();
        }

        private void ChangeMaterial(GameObject gameObjectToAffect, int matIndex)
        {
            var mats = gameObjectToAffect.GetComponent<Renderer>().materials;
            var newMat = mats[matIndex];

            mats[0] = newMat;
            gameObjectToAffect.GetComponent<Renderer>().materials = mats;
        }

        //private void ChangeMaterial(int matIndex)
        //{
        //    // Apply ShelterBox green-blue material to box and lid

        //    var box = gameObject;
        //    var boxLid = box.transform.GetChild(0).gameObject;

        //    var boxMaterials = box.GetComponent<Renderer>().materials;
        //    var mat = boxMaterials[matIndex];

        //    boxMaterials[0] = mat;
        //    box.GetComponent<Renderer>().materials = boxMaterials;

        //    var boxLidMats = boxLid.GetComponent<Renderer>().materials;
        //    boxLidMats[0] = mat;
        //    boxLid.GetComponent<Renderer>().materials = boxLidMats;
        //}
    }
}
