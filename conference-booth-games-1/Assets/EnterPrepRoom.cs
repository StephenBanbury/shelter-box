using UnityEngine;
using UnityEngine.UI;

public class EnterPrepRoom : MonoBehaviour
{
    public Text DoorMessage;
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
            DoorMessage.text = "Welcome!";
            GameManager.instance.LoadAppropriateScene("PreperationCentre");
        } 
    }
}
