using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class AnimateMonitor : MonoBehaviour
    {
        [SerializeField] private Animator monitor1;
        [SerializeField] private Animator monitor2;
        [SerializeField] private Animator monitor3;
        [SerializeField] private Animator monitor4;

        public static AnimateMonitor instance;

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

        public void ActivateMonitor(string monitor, string action)
        {
            if (action == "close")
            {
                switch (monitor)
                {
                    case "monitor1":
                        monitor1.SetBool("closeMonitor", true);
                        break;
                    case "monitor2":
                        monitor2.SetBool("closeMonitor", true);
                        break;
                    case "monitor3":
                        monitor3.SetBool("closeMonitor", true);
                        break;
                    case "monitor4":
                        monitor4.SetBool("closeMonitor", true);
                        break;
                }
            }

            if (action == "open")
            {
                switch (monitor)
                {
                    case "monitor1":
                        monitor1.SetBool("openMonitor", true);
                        break;
                    case "monitor2":
                        monitor2.SetBool("openMonitor", true);
                        break;
                    case "monitor3":
                        monitor3.SetBool("openMonitor", true);
                        break;
                    case "monitor4":
                        monitor4.SetBool("openMonitor", true);
                        break;
                }
            }

        }
    }
}