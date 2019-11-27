using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CheckIn : MonoBehaviour
    {
        public Text checkInText;

        private AudioSource audioSource1;
        private AudioSource audioSource2;

        void Start()
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSource1 = audioSources[0]; 
            audioSource2 = audioSources[1];
        }
        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Hand"))
            {
                GameObject.Find("CheckIn").GetComponent<Renderer>().material.color = new Color(0, 196, 255, 255);

                var audioSources = gameObject.GetComponents<AudioSource>();
                audioSource1.Play();
                audioSource2.Play();
                checkInText.text = "Enjoy your flight!";

                FindObjectOfType<AeroplaneController>().startTaxi = true;
            }
        }
    }
}