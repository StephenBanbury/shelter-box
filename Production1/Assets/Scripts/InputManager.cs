﻿using Oculus.Platform.Samples.VrHoops;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class InputManager : MonoBehaviour
    {

        [SerializeField] private InputField playerNameInputField;

        public void OnStart()
        {
            Debug.Log("Start game");
        }

        public void OnKeyEnter()
        {
            var playerName = playerNameInputField.text;

            if (playerName != string.Empty)
            {
                var kb = GameObject.Find("InputKeyboard");
                var slate = GameObject.Find("Slate");
                Destroy(slate);
                Destroy(kb);

                var newPlayer = PlayerManager.instance.NewPlayer(playerName);

                Debug.Log($"Player: {newPlayer.PlayerName}, ID: {newPlayer.PlayerId}, Score: {newPlayer.CurrentScore}, Hi Score: {newPlayer.HighScore}.");

                GameManager.instance.PersonalMessage(playerName);

                //AnimationManager.instance.OpenEntranceDoor("open");
                AnimationManager.instance.RaiseCentrePartition(true);

                GameManager.instance.PlayAudio("missionStatementPart2");
            }
        }
    }
}
