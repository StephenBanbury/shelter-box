using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.Controllers;
using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts
{
    public class Goal : MonoBehaviour
    {
        public Text score1Text;
        public Text score2Text;
        public Text score3Text;
        public Text score4Text;
        public Text grandScoreText;
        public Text infoText;

        private AudioSource audioSource1;
        private AudioSource audioSource2;

        static int grandScore;

        void Start()
        {
            var audioSources = GetComponents<AudioSource>();
            audioSource1 = audioSources[0]; // Audio source = if this is a box then it's is a beep; if it's the floor then it's a thud
            audioSource2 = audioSources[1];
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Cassette") || other.CompareTag("Tent") || other.CompareTag("Bottle"))
            {
                DetectHitOrMiss(other);
            }
        }

        void DetectHitOrMiss(Collider other)
        {
            string infoMessage = "";

            int reportIndex = 0;
            bool hitFloor = false;

            // Get suitable resources list from report object
            // First get report index of report currently associated with hit box

            switch (gameObject.name)
            {
                case "Box1":
                    reportIndex = Reports.instance.reportIndex0;
                    break;
                case "Box2":
                    reportIndex = Reports.instance.reportIndex1;
                    break;
                case "Box3":
                    reportIndex = Reports.instance.reportIndex2;
                    break;
                case "Box4":
                    reportIndex = Reports.instance.reportIndex3;
                    break;
                case "Floor":
                    hitFloor = true;
                    break;
            }

            infoText.text = $"Report Index: {reportIndex.ToString()}";

            score1Text.text = "";
            score2Text.text = "";
            score3Text.text = "";
            score4Text.text = "";

            // Then get collection of resource IDs from indexed report
            if (!hitFloor)
            {
                var cassetteController = other.gameObject.GetComponent<CassetteController>();
                var myResourceId = cassetteController.myResourceId;
                var resourcesRequiredForDisaster = Reports.instance.reports[reportIndex].RequiredResources;
                var selectedIsRequiredResource = resourcesRequiredForDisaster.Contains(myResourceId);

                infoMessage = "Box hit";

                if (selectedIsRequiredResource)
                {
                    infoMessage =
                        $"Thanks for the {Regex.Replace(((Resource)myResourceId).ToString(), "(\\B[A-Z])", " $1")}";
                    audioSource1.Play();
                    grandScore++;

                    switch (gameObject.name)
                    {
                        case "Box1":
                            score1Text.text = "Thanks!";
                            break;
                        case "Box2":
                            score2Text.text = "Thanks!";
                            break;
                        case "Box3":
                            score3Text.text = "Thanks!";
                            break;
                        case "Box4":
                            score4Text.text = "Thanks!";
                            break;
                    }
                }
                else
                {
                    infoMessage =
                        $"{Regex.Replace(((Resource)myResourceId).ToString(), "(\\B[A-Z])", " $1")} not required";
                    audioSource2.Play();
                    grandScore--;
                }
            }
            else
            {
                grandScore--;
                infoMessage = "Floor hit";
                audioSource1.Play();
            }

            infoText.text = infoMessage;
            grandScoreText.text = $"Score: {grandScore.ToString("0")}";

            Destroy(other.gameObject);

            CassetteInstantiator.instance.CreateCassette();
        }
    }
}
