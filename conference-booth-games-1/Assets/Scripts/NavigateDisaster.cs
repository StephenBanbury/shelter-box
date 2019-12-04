using UnityEngine;

namespace Assets.Scripts
{
    public class NavigateDisaster : MonoBehaviour
    {
        private AudioSource audioSource1;

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

                GameManager.instance.LoadAppropriateScene(GameManager.instance.CurrentScene() == "HomeTown"
                    ? "Disaster"
                    : "HomeTown");
            }
        }
    }
}
