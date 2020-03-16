using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class ResourceInstantiator : MonoBehaviour
    {
        public GameObject myPrefab1;
        public GameObject myPrefab2;
        public GameObject myPrefab3;
        public GameObject myPrefab4;
        public GameObject myPrefab5;
        public GameObject myPrefab6;

        public int resourcesPerBin = 1;

        private GameObject[] myPrefabs;

        public static ResourceInstantiator instance;
        static Random random = new Random();

        void Awake()
        {
            // Check that it exists
            if (instance == null)
            {
                //assign it to the current object
                instance = this;
            }
        }

        // Instantiate the Prefab when the game starts.
        void Start()
        {
            myPrefabs = new GameObject[] { myPrefab1, myPrefab2, myPrefab3, myPrefab4, myPrefab5, myPrefab6};

            for (int z = 1; z <= resourcesPerBin; z++)
            {
                for (int i = 1; i <= myPrefabs.Length; i++)
                {
                    InstantiateOneResource(i);
                }
            }
        }

        public void CreateResourceObject(string resourceObjectName)
        {
            Debug.Log($"Replace resourceObjectName: {resourceObjectName}");

            for (int i = 1; i <= myPrefabs.Length; i++)
            {
                Debug.Log($"myPrefabs name: {myPrefabs[i - 1].name}");

                if (myPrefabs[i-1].name == resourceObjectName)
                {
                    InstantiateOneResource(i);
                }
            }
        }

        public void CreateResourceObjectAtRandomPosition()
        {
            int randomPrefab = random.Next(0, myPrefabs.Length);
            float randomPosition = random.Next(-2, 2) * 0.1f;

            var myPrefab = myPrefabs[randomPrefab];
            Instantiate(myPrefab, new Vector3(0.08f + randomPosition, 3.5f, 1.4f + randomPosition), Quaternion.identity);
        }

        private void InstantiateOneResource(int i)
        {
            var boxName = $"ResourceBin{i}";
            var box = GameObject.Find(boxName);

            var xPos = box.transform.position.x;
            var yPos = box.transform.position.y + 0.5f;
            var zPos = box.transform.position.z;

            var myPrefab = myPrefabs[i - 1];

            Instantiate(myPrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity);
        }
    }
}