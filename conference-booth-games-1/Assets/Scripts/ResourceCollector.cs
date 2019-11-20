using System.Linq;
using System.Text;
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
        //public Text infoText;

        private AudioSource audioSource1;
        private AudioSource audioSource2;

        public int grandScore;

        void Start()
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSource1 = audioSources[0]; // Audio source = if this is a box then it's is a beep; if it's the floor then it's a thud
            audioSource2 = audioSources[1];

            ChangeMaterial(0);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Cassette") || 
                other.CompareTag("Tent") || 
                other.CompareTag("Food") ||
                other.CompareTag("Bottle") ||
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

            //infoText.text = $"Report Index: {reportIndex.ToString()}";

            BoxMessage1Text.text = "";
            BoxMessage2Text.text = "";
            BoxMessage3Text.text = "";
            BoxMessage4Text.text = "";

            // Then get collection of resource IDs from indexed report
            if (!hitFloor)
            {
                var resourceManager = other.gameObject.GetComponent<ResourceManager>();
                var myResourceId = resourceManager.myResourceId;
                var resourcesRequiredForDisaster = ReportsManager.instance.RequiredResources(reportId);
                var selectedIsRequiredResource = resourcesRequiredForDisaster.Contains(myResourceId);

                if (selectedIsRequiredResource)
                {
                    ReportsManager.instance.CollectResource(reportId, myResourceId);
                    ReportsManager.instance.AssignReportsToMonitors();
                    
                    infoMessage =
                        $"Thanks for the {Regex.Replace(((Resource)myResourceId).ToString(), "(\\B[A-Z])", " $1")}";

                    audioSource1.Play();
                    //grandScore++;

                    if (ReportsManager.instance.AllResourcesCollected(reportId))
                    {
                        ChangeMaterial(1);
                        GameManager.instance.UpdateDeploymentStatus(1);
                    }
                }
                else
                {
                    infoMessage =
                        $"{Regex.Replace(((Resource)myResourceId).ToString(), "(\\B[A-Z])", " $1")} not required";
                    audioSource2.Play();
                    //grandScore--;
                }

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
                //grandScore--;
                audioSource1.Play();
            }

            //infoText.text = infoMessage;
            //grandScoreText.text = $"Score: {grandScore.ToString("0")}";

            Destroy(other.gameObject);

            ResourceInstantiator.instance.CreateResourceObject();
        }

        private void ChangeMaterial(int matIndex)
        {
            var mats = gameObject.GetComponent<Renderer>().materials;
            var mat = mats[matIndex];
            mats[0] = mat;
            gameObject.GetComponent<Renderer>().materials = mats;
        }

    }
}
