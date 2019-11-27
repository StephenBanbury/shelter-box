using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CheckIn : MonoBehaviour
    {
        public Text checkInText;

        private AudioSource audioSource;

        void Start()
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSource = audioSources[0]; 
        }
        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Hand"))
            {
                GameObject.Find("CheckIn").GetComponent<Renderer>().material.color = new Color(0, 196, 255, 255);

                var audioSources = gameObject.GetComponents<AudioSource>();
                audioSource.Play();

                checkInText.text = "Enjoy your flight!";

                FindObjectOfType<AeroplaneController>().StartTaxi();
            }
        }
    }
}