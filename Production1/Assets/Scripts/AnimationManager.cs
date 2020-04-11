using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class AnimationManager : MonoBehaviour
    {
        [SerializeField] private Animator monitor1;
        [SerializeField] private Animator monitor2;
        [SerializeField] private Animator monitor3;
        [SerializeField] private Animator monitor4;

        [SerializeField] private Animator monitorTextFade;

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

        public void RaiseCentrePartition(string action)
        {
            Debug.Log("RaiseCentrePartition");
            centrePerimeter.SetBool(action, true);
        }

        public void ActivateMonitor(string monitor, string action)
        {
            Debug.Log($"Activate {monitor}: {action}");

            if (action == "open")
            {
                switch (monitor)
                {
                    case "monitor1":
                        //monitor1.SetBool("closeMonitor", false);
                        monitor1.SetBool("openMonitor", true);
                        //monitorTextFade.SetBool("text1FadeOut", false);
                        monitorTextFade.SetBool("text1FadeIn", true);
                        break;
                    case "monitor2":
                        monitor2.SetBool("closeMonitor", false);
                        monitor2.SetBool("openMonitor", true);
                        monitorTextFade.SetBool("text2FadeOut", false);
                        monitorTextFade.SetBool("text2FadeIn", true);
                        break;
                    case "monitor3":
                        monitor3.SetBool("closeMonitor", false);
                        monitor3.SetBool("openMonitor", true);
                        monitorTextFade.SetBool("text3FadeOut", false);
                        monitorTextFade.SetBool("text3FadeIn", true);
                        break;
                    case "monitor4":
                        monitor4.SetBool("closeMonitor", false);
                        monitor4.SetBool("openMonitor", true);
                        monitorTextFade.SetBool("text4FadeOut", false);
                        monitorTextFade.SetBool("text4FadeIn", true);
                        break;
                }
            }
            else if (action == "close")
            {
                switch (monitor)
                {
                    case "monitor1":
                        monitorTextFade.SetBool("text1FadeIn", false);
                        //monitor1.SetBool("openMonitor", false);
                        monitor1.SetBool("closeMonitor", true);
                        //monitorTextFade.SetBool("text1FadeOut", true);
                        break;
                    case "monitor2":
                        monitor2.SetBool("openMonitor", false);
                        monitor2.SetBool("closeMonitor", true);
                        monitorTextFade.SetBool("text2FadeIn", false);
                        monitorTextFade.SetBool("text2FadeOut", true);
                        break;
                    case "monitor3":
                        monitor3.SetBool("openMonitor", false);
                        monitor3.SetBool("closeMonitor", true);
                        monitorTextFade.SetBool("text3FadeIn", false);
                        monitorTextFade.SetBool("text3FadeOut", true);
                        break;
                    case "monitor4":
                        monitor4.SetBool("openMonitor", false);
                        monitor4.SetBool("closeMonitor", true);
                        monitorTextFade.SetBool("text4FadeIn", false);
                        monitorTextFade.SetBool("text4FadeOut", true);
                        break;
                }
            }
        }

        public void FadeMonitorText(bool fadeIn)
        {
            Debug.Log("FadeMonitorText");
            monitorTextFade.SetBool("text4FadeIn", fadeIn);
            monitorTextFade.SetBool("text4FadeOut", !fadeIn);
        }
    }
}