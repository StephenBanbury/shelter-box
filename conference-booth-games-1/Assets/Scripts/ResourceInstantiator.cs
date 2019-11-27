﻿using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    public class ResourceInstantiator : MonoBehaviour
    {
        public GameObject myPrefab1;
        public GameObject myPrefab2;
        public GameObject myPrefab3;
        public GameObject myPrefab4;
        public GameObject myPrefab5;
        public GameObject myPrefab6;
        //public GameObject myPrefab7;

        private GameObject[] myPrefabs;

        //public int numberOfResourceObjects = 7;

        public static ResourceInstantiator instance;

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
            myPrefabs = new GameObject[] { myPrefab1, myPrefab2, myPrefab3, myPrefab4, myPrefab5, myPrefab6};

            for (int i = 1; i <= myPrefabs.Length; i++)
            {
                CreateResourceObject();
            }
        }

        public void CreateResourceObject()
        {
            Random random = new Random();
            int randomPrefab = random.Next(0, myPrefabs.Length);
            float randomPosition = random.Next(-2, 2) * 0.1f;

            var myPrefab = myPrefabs[randomPrefab];

            Instantiate(myPrefab, new Vector3(0.08f + randomPosition, 3.5f, 2.3f + randomPosition), Quaternion.identity);
        }
    }
}