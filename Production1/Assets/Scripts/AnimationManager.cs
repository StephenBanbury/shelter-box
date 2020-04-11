using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class AnimationManager : MonoBehaviour
    {
        [SerializeField] private Animator monitor1;
        [SerializeField] private Animator monitor2;
        [SerializeField] private Animator monitor3;
        [SerializeField] private Animator monitor4;

        //[SerializeField] private Animation monitor1Text;

        [SerializeField] private Animator entranceDoor;

        [SerializeField] private Animator centrePerimeter;

        public static AnimationManager instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        public void OpenEntranceDoor(string action)
        {
                entranceDoor.SetBool(action, true);
        }

        public void RaiseCentrePartition(bool raise)
        {
            Debug.Log("RaiseCentrePartition");
            centrePerimeter.SetBool("raise", raise);
        }

        public void ActivateMonitor(string monitor, bool activate)
        {
            Debug.Log($"Activate {monitor}: {activate}");
            
            switch (monitor)
            {
                case "monitor1":
                    monitor1.SetBool("openMonitor", activate);
                    break;
                case "monitor2":
                    monitor2.SetBool("openMonitor", activate);
                    break;
                case "monitor3":
                    monitor3.SetBool("openMonitor", activate);
                    break;
                case "monitor4":
                    monitor4.SetBool("openMonitor", activate);
                    break;
            }
        }
    }
}