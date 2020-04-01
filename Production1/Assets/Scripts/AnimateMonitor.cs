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

        public void CloseMonitor(string monitor, bool close)
        {
            switch (monitor)
            {
                case "monitor1":
                    monitor1.SetBool("closeMonitor", close);
                    break;
                case "monitor2":
                    monitor2.SetBool("closeMonitor", close);
                    break;
                case "monitor3":
                    monitor3.SetBool("closeMonitor", close);
                    break;
                case "monitor4":
                    monitor4.SetBool("closeMonitor", close);
                    break;
            }
        }
    }
}