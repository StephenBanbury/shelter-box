using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    public class ResourceObjectInstantiator : MonoBehaviour
    {
        public GameObject myPrefab1;
        public GameObject myPrefab2;
        public GameObject myPrefab3;

        private GameObject[] myPrefabs;

        public int numberOfResourceObjects = 5;

        public static ResourceObjectInstantiator instance;

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
            myPrefabs = new GameObject[] {myPrefab1, myPrefab2, myPrefab3};

            //myPrefabs[0] = myPrefab1;
            //myPrefabs[1] = myPrefab2;
            //myPrefabs[2] = myPrefab3;

            for (int i = 1; i <= numberOfResourceObjects; i++)
            {
                CreateResourceObject();
            }
        }

        public void CreateResourceObject()
        {
            Random random = new Random();
            int randomValue = random.Next(0, myPrefabs.Length);

            var myPrefab = myPrefabs[randomValue];
            Instantiate(myPrefab, new Vector3(0.08f, 3.5f, 2.3f), Quaternion.identity);
        }
    }
}