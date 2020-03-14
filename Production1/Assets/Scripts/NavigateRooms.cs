using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class NavigateRooms : MonoBehaviour
    {
        private AudioSource audioSource1;
        //private AudioSource audioSource2;

        void Start()
        {
            var audioSources = GetComponents<AudioSource>();
            audioSource1 = audioSources[0];
            //audioSource2 = audioSources[1];
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hand"))
            {
                audioSource1.Play();

                switch (GameManager.instance.CurrentScene())
                {
                    case "Welcome":
                        GameManager.instance.StartCountdown();
                        GameManager.instance.LoadAppropriateScene("Disaster");
                        break;

                    case "Disaster":
                        GameManager.instance.LoadAppropriateScene("PrepRoom");
                        break;

                    case "HomeTown":
                        GameManager.instance.LoadAppropriateScene("PrepRoom");
                        break;

                    case "PrepRoom":
                        GameManager.instance.LoadAppropriateScene("HomeTown");
                        break;
                }
            }
        }
    }
}
