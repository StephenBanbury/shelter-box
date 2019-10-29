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
        public Text collectedItemText;

        private AudioSource goalAudioSource;
        private bool floorHit;

        static int hits1;
        static int hits2;
        static int hits3;
        static int hits4;
        static int grandScore;

        void Start()
        {
            goalAudioSource = GetComponent<AudioSource>();
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
            //collectedItemText.text = cassetteController.cassetteText.text;

            //int[] resourcesRequired = null;
            int reportIndex = 0;

            // Get suitable resources list from report object
            // First get report index of report currently associated with hit box

            switch (gameObject.name)
            {
                case "Box1":
                    reportIndex = Reports.instance.reportIndex0;
                    hits1++;
                    break;
                case "Box2":
                    reportIndex = Reports.instance.reportIndex1;
                    hits2++;
                    break;
                case "Box3":
                    reportIndex = Reports.instance.reportIndex2;
                    hits3++;
                    break;
                case "Box4":
                    reportIndex = Reports.instance.reportIndex3;
                    hits4++;
                    break;
                case "Floor":
                    floorHit = true;
                    break;
            }
            
            string message;

            // Then get collection of resource IDs from indexed report
            if (!floorHit)
            {
                var cassetteController = other.gameObject.GetComponent<CassetteController>();

                var myResourceId = cassetteController.myResourceId;
                var myResourceName = cassetteController.cassetteText;

                var resourcesRequired = Reports.instance.reports[reportIndex].RequiredResources;

                

                if (resourcesRequired.Contains(myResourceId))
                {
                    message =
                        $"Thanks for the {Regex.Replace(((Resource) myResourceId).ToString(), "(\\B[A-Z])", " $1")}";
                    goalAudioSource.Play();
                    grandScore++;
                }
                else
                {
                    message =
                        $"{Regex.Replace(((Resource) myResourceId).ToString(), "(\\B[A-Z])", " $1")} is not required";
                    grandScore--;
                }

                //collectedItemText.text = resourcesRequired[0].ToString();

            }
            else
            {
                grandScore--;
                message = "Floor!";
            }

            collectedItemText.text = message;

                //grandScore = hits1 + hits2 + hits3 + hits4;

                //score1Text.text = hits1.ToString("0");
                //score2Text.text = hits2.ToString("0");
                //score3Text.text = hits3.ToString("0");
                //score4Text.text = hits4.ToString("0");

                grandScoreText.text = $"Total: {grandScore.ToString("0")}";


            Destroy(other.gameObject);

            CassetteInstantiator.instance.CreateCassette();
        }
    }
}
