using System.Collections;
using UnityEditor;
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
        [SerializeField] private Animator fadeOutHighScoresTable1;
        [SerializeField] private Animator fadeOutHighScoresTable2;
        [SerializeField] private Animator boxesThruFloor;
        [SerializeField] private Animator dropDownPlayAgainButton;

        [SerializeField] private GameObject playerController;
        [SerializeField] private GameObject highscoresTable1;
        [SerializeField] private GameObject fireCurtain;


        private bool animateHighscoreTable;
        private bool animatePlayerController;
        public float speed = 1.5f;
        private Vector3 highScoreTableTarget = new Vector3(0, 2f, 2.5f);
        private Vector3 playerControllerTarget = new Vector3(0, 1f, -6f);


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
            //if (animateHighscoreTable)
            //{
            //    StartCoroutine(MoveHighscoresTable());
            //}

            if (animatePlayerController)
            {
                StartCoroutine(ReturnPlayerToPosition());
            }
        }

        //private IEnumerator MoveHighscoresTable()
        //{
        //    if (highscoresTable1.transform.position == highScoreTableTarget)
        //    {
        //        //Debug.Log("Highscore reached target");
        //        animateHighscoreTable = false;
        //    }

        //    float step = speed * Time.deltaTime;
        //    highscoresTable1.transform.position =
        //        Vector3.MoveTowards(highscoresTable1.transform.position, highScoreTableTarget, step);

        //    yield return new WaitForSeconds(0.1f);
        //}

        private IEnumerator ReturnPlayerToPosition()
        {
            if (playerController.transform.position == playerControllerTarget)
            {
                Debug.Log("playerController reached target");

                FadeFireCurtain(false);
                FadeOutPlayButton(false);
                FadeOutHighScoresTable(1, false);
                LowerStartButton(false);

                GameManager.instance.PlayAgain();

                animatePlayerController = false;
            }

            float step = speed * Time.deltaTime;
            playerController.transform.position =
                Vector3.MoveTowards(playerController.transform.position, playerControllerTarget, step);

            yield return new WaitForSeconds(0.1f);
        }

        //public void DropDownPlayAgainButton()
        //{
        //    Debug.Log($"DropDownPlayAgainButton");
        //    dropDownPlayAgainButton.SetBool("drop", true);
        //}

        //public void AnimateHighScoresTable()
        //{
        //    animateHighscoreTable = true;
        //}

        public void AnimatePlayerController()
        {
            ActivateMonitor("Monitor1", false);
            ActivateMonitor("Monitor2", false);
            ActivateMonitor("Monitor3", false);
            ActivateMonitor("Monitor4", false);

            animatePlayerController = true;
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

        public void FadeOutHighScoresTable(int table, bool fadeOut)
        {
            Debug.Log($"FadeOutHighScoresTable{table}: {fadeOut}");
            switch (table)
            {
                case 1:
                    fadeOutHighScoresTable1.SetBool("fadeOut", fadeOut);
                    StartCoroutine(fadeOut ? ActivateHighscoresTable(false, 6) : ActivateHighscoresTable(true, 0));

                    break;
                case 2:
                    fadeOutHighScoresTable2.SetBool("fadeOut", fadeOut);
                    break;
            }
        }

        private IEnumerator ActivateHighscoresTable(bool active, int secsDelay)
        {
            yield return new WaitForSeconds(secsDelay);
            highscoresTable1.SetActive(active);
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