using Assets.Scripts.Controllers;
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

        static int hits1;
        static int hits2;
        static int hits3;
        static int hits4;
        static int grandScore;


        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Cassette"))
            {
                DetectHitOrMiss(other);
            }
        }

        void DetectHitOrMiss(Collider other)
        {
            switch (gameObject.name)
            {
                case "Box1":
                    hits1++;
                    break;
                case "Box2":
                    hits2++;
                    break;
                case "Box3":
                    hits3++;
                    break;
                case "Box4":
                    hits4++;
                    break;
            }

            grandScore = hits1 + hits2 + hits3 + hits4;

            score1Text.text = hits1.ToString("0");
            score2Text.text = hits2.ToString("0");
            score3Text.text = hits3.ToString("0");
            score4Text.text = hits4.ToString("0");

            grandScoreText.text = $"Total: {grandScore.ToString("0")}";

            collectedItemText.text = "hahaha";

            Destroy(other.gameObject);

            CassetteInstantiator.instance.CreateCassette();
        }
    }
}
