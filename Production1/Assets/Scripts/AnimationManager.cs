using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class AnimationManager : MonoBehaviour
    {
        public static AnimationManager instance;

        [SerializeField] private Animator monitor1;
        [SerializeField] private Animator monitor2;
        [SerializeField] private Animator monitor3;
        [SerializeField] private Animator monitor4;
        [SerializeField] private Animator centrePerimeter;
        [SerializeField] private Animator startButton;
        [SerializeField] private Animator inputKeyboard;
        [SerializeField] private Animator fadeFireCurtain;
        [SerializeField] private Animator fadeOutPlayButton;
        [SerializeField] private Animator fadeOutHighScoresPanel;
        [SerializeField] private Animator boxesThruFloor;

        [SerializeField] private GameObject highscoresTable;
        [SerializeField] private CanvasGroup highscoresCanvasGroup;

        private bool animateHighscoreTable;
        public float speed = 1.0f;
        private Vector3 highScoreTableTarget = new Vector3(0, 2f, 2.5f);


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

        private void FixedUpdate()
        {
            if (animateHighscoreTable)
            {
                StartCoroutine(MoveHighscoresTable());
            }
        }

        private IEnumerator MoveHighscoresTable()
        {
            if (highscoresTable.transform.position == highScoreTableTarget)
            {
                Debug.Log("Highscore reached target");
                animateHighscoreTable = false;
            }

            float step = speed * Time.deltaTime;
            highscoresTable.transform.position =
                Vector3.MoveTowards(highscoresTable.transform.position, highScoreTableTarget, step);

            yield return new WaitForSeconds(0.1f);
        }

        public void AnimateHighScoresPanel()
        {
            animateHighscoreTable = true;
        }

        public void BoxesThruFloor(bool up)
        {
            Debug.Log($"BoxesThruFloor: {up}");
            boxesThruFloor.SetBool("up", up);
        }

        public void FadeOutPlayButton(bool fadeOut)
        {
            Debug.Log($"FadeOutPlayButton: {fadeOut}");
            fadeOutPlayButton.SetBool("fadeOut", fadeOut);
        }

        public void FadeOutHighScoresPanel(bool fadeOut)
        {
            Debug.Log($"FadeOutHighScoresPanel: {fadeOut}");
            fadeOutHighScoresPanel.SetBool("fadeOut", fadeOut);
        }

        public void FadeFireCurtain(bool fadeOut)
        {
            Debug.Log($"FadeFireCurtain: {fadeOut}");
            fadeFireCurtain.SetBool("fadeOut", fadeOut);
        }

        public void RaiseCentrePartition(bool raise)
        {
            Debug.Log($"RaiseCentrePartition: {raise}");
            centrePerimeter.SetBool("raise", raise);
        }

        public void LowerStartButton(bool lower)
        {
            Debug.Log($"LowerStartButton: {lower}");
            startButton.SetBool("lower", lower);
        }

        public void OpenInputKeyboard(bool open)
        {
            Debug.Log($"OpenInputKeyboard: {open}");
            inputKeyboard.SetBool("open", open);
        }

        public void ActivateMonitor(string monitor, bool activate)
        {
            Debug.Log($"Activate {monitor}: {activate}");
            
            switch (monitor)
            {
                case "Monitor1":
                    if (monitor1.GetBool("openMonitor") != activate)
                    {
                        monitor1.SetBool("openMonitor", activate);
                    }
                    break;
                case "Monitor2":
                    if (monitor2.GetBool("openMonitor") != activate)
                    {
                        monitor2.SetBool("openMonitor", activate);
                    }
                    break;
                case "Monitor3":
                    if (monitor3.GetBool("openMonitor") != activate)
                    {
                        monitor3.SetBool("openMonitor", activate);
                    }
                    break;
                case "Monitor4":
                    if (monitor4.GetBool("openMonitor") != activate)
                    {
                        monitor4.SetBool("openMonitor", activate);
                    }
                    break;
            }

            //Debug.Log($"Monitor1 activated: {monitor1.GetBool("openMonitor")}");
            //Debug.Log($"Monitor2 activated: {monitor1.GetBool("openMonitor")}");
            //Debug.Log($"Monitor3 activated: {monitor1.GetBool("openMonitor")}");
            //Debug.Log($"Monitor4 activated: {monitor1.GetBool("openMonitor")}");
        }
    }
}