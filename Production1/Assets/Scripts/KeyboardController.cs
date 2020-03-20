using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class KeyboardController : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hand"))
            {
                Debug.Log("Keyboard press");

                var audio = GetComponent<AudioSource>();
                audio.Play();
                
                GameManager.instance.BudgetRemaining = 1000;
                GameManager.instance.UpdateBudgetDisplay();
            }
        }
    }
}