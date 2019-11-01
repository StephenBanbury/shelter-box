﻿using UnityEngine;

namespace Assets.Scripts
{
    public class CassetteInstantiator : MonoBehaviour
    {
        public GameObject myPrefab1;
        public GameObject myPrefab2;
        public GameObject myPrefab3;

        public int numberOfCassettes = 5;

        public static CassetteInstantiator instance;

        void Awake()
        {
            // Check that it exists
            if (instance == null)
            {
                //assign it to the current object
                instance = this;
            }
        }

        // This script will simply instantiate the Prefab when the game starts.
        void Start()
        {
            for (int i = 1; i <= numberOfCassettes; i++)
            {
                CreateCassette();
            }
        }

        public void CreateCassette()
        {
            var myPrefab = myPrefab1;
            Instantiate(myPrefab, new Vector3(0.08f, 3.5f, 2.3f), Quaternion.identity);
        }
    }
}