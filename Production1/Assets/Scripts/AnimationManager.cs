﻿using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class AnimationManager : MonoBehaviour
    {
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

        //[SerializeField] private Animation monitor1Text;

        //[SerializeField] private Animator entranceDoor;

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
                    monitor1.SetBool("openMonitor", activate);
                    break;
                case "Monitor2":
                    monitor2.SetBool("openMonitor", activate);
                    break;
                case "Monitor3":
                    monitor3.SetBool("openMonitor", activate);
                    break;
                case "Monitor4":
                    monitor4.SetBool("openMonitor", activate);
                    break;
            }
        }
    }
}