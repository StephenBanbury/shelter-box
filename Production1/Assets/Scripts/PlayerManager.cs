using System;
using System.Collections;
using System.Collections.Generic;
using Com.MachineApps.PrepareAndDeploy.Models;
using UnityEditor;
using UnityEngine;


namespace Com.MachineApps.PrepareAndDeploy
{
    public class PlayerManager : MonoBehaviour
    {
        // PlayerManger should eventually use a service to save, retrieve and update players, scores, leaderboard etc within a realtime database, such as Firebase.

        public static PlayerManager instance;

        private Player currentPlayer;

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

        public string Player
        {
            get => currentPlayer.PlayerName;
            set => NewPlayer(value);
        }

        public Player NewPlayer(string playerName)
        {
            var guidHelper = new GuidHelper();

            var id = GuidHelper.GetUniqueID();

            var newPlayer = new Player
            {
                PlayerId = id,
                PlayerName = playerName,
                CurrentScore = 0
            };

            // TODO look in PlayerService for high / previous scores

            currentPlayer = newPlayer;

            return currentPlayer;
        }

        public Player GetCurrentPlayer()
        {
            return currentPlayer;
        }
    }
}