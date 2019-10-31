using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.Controllers;
using Assets.Scripts.Enums;
using UnityEditor;
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
        //public Text collectedItemText;

        private AudioSource audioSource1;
        private AudioSource audioSource2;

        private bool floorHit;
        static int grandScore;

        void Start()
        {
            var audioSources = GetComponents<AudioSource>();
            audioSource1 = audioSources[0]; // Audio source = if this is a box then it's is a beep; if it's the floor then it's a thud
            audioSource2 = audioSources[1];
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Cassette"))
            {
                DetectHitOrMiss(other);
            }
        }

        void DetectHitOrMiss(Collider other)
        {
            int reportIndex = 0;

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
                    floorHit = true;
                    break;
            }
            
            //string mainMessage;

            score1Text.text = "";
            score2Text.text = "";
            score3Text.text = "";
            score4Text.text = "";

            // Then get collection of resource IDs from indexed report
            if (!floorHit)
            {
                var cassetteController = other.gameObject.GetComponent<CassetteController>();
                var myResourceId = cassetteController.myResourceId;
                var resourcesRequiredForDisaster = Reports.instance.reports[reportIndex].RequiredResources;
                var selectedIsRequiredResource = resourcesRequiredForDisaster.Contains(myResourceId);

                if (selectedIsRequiredResource)
                {
                    //mainMessage =
                    //    $"Thanks for the {Regex.Replace(((Resource) myResourceId).ToString(), "(\\B[A-Z])", " $1")}";
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
                    //mainMessage =
                    //    $"{Regex.Replace(((Resource) myResourceId).ToString(), "(\\B[A-Z])", " $1")} not required";
                    audioSource2.Play();
                    grandScore--;
                }
            }
            else
            {
                grandScore--;
                //mainMessage = "";
                audioSource1.Play();
            }

            //collectedItemText.text = mainMessage;
            grandScoreText.text = $"Total: {grandScore.ToString("0")}";

            Destroy(other.gameObject);

            CassetteInstantiator.instance.CreateCassette();
        }
    }
}
