using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class AnimationManager : MonoBehaviour
    {
        [SerializeField] private Animator monitor1;
        [SerializeField] private Animator monitor2;
        [SerializeField] private Animator monitor3;
        [SerializeField] private Animator monitor4;

        [SerializeField] private Animator monitorText;

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

        public void RaiseCentrePartition(string action)
        {
            Debug.Log("RaiseCentrePartition");
            centrePerimeter.SetBool(action, true);
        }

        public void ActivateMonitor(string monitor, string action)
        {
            Debug.Log("ActivateMonitor");
            if (action == "close")
            {
                switch (monitor)
                {
                    case "monitor1":
                        monitor1.SetBool("openMonitor", false);
                        monitor1.SetBool("closeMonitor", true);
                        monitorText.SetBool("fadeIn", false);
                        break;
                    case "monitor2":
                        monitor2.SetBool("openMonitor", false);
                        monitor2.SetBool("closeMonitor", true);
                        monitorText.SetBool("fadeIn", false);
                        break;
                    case "monitor3":
                        monitor3.SetBool("openMonitor", false);
                        monitor3.SetBool("closeMonitor", true);
                        monitorText.SetBool("fadeIn", false);
                        break;
                    case "monitor4":
                        monitor4.SetBool("openMonitor", false);
                        monitor4.SetBool("closeMonitor", true);
                        monitorText.SetBool("fadeIn", false);
                        break;
                }
            }

            if (action == "open")
            {
                switch (monitor)
                {
                    case "monitor1":
                        monitor1.SetBool("closeMonitor", false);
                        monitor1.SetBool("openMonitor", true);
                        monitorText.SetBool("fadeIn", true);
                        break;
                    case "monitor2":
                        monitor2.SetBool("closeMonitor", false);
                        monitor2.SetBool("openMonitor", true);
                        monitorText.SetBool("fadeIn", true);
                        break;
                    case "monitor3":
                        monitor3.SetBool("closeMonitor", false);
                        monitor3.SetBool("openMonitor", true);
                        monitorText.SetBool("fadeIn", true);
                        break;
                    case "monitor4":
                        monitor4.SetBool("closeMonitor", false);
                        monitor4.SetBool("openMonitor", true);
                        monitorText.SetBool("fadeIn", true);
                        break;
                }
            }
        }

        public void FadeMonitorText(bool fadeIn)
        {
            Debug.Log("FadeMonitorText");
            monitorText.SetBool("fadeIn", fadeIn);
        }
    }
}