using UnityEngine;

namespace Assets.Scripts
{
    public class NavigateDisaster : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hand"))
            {
                GameManager.instance.LoadAppropriateScene(GameManager.instance.CurrentScene() == "HomeTown"
                    ? "Disaster"
                    : "HomeTown");
            }
        }
    }
}
