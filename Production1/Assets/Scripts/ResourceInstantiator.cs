using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using System.Linq;
using Com.MachineApps.PrepareAndDeploy.Enums;
using Com.MachineApps.PrepareAndDeploy.Services;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class ResourceInstantiator : MonoBehaviour
    {
        [SerializeField] private GameObject myPrefab1;
        [SerializeField] private GameObject myPrefab2;
        [SerializeField] private GameObject myPrefab3;
        [SerializeField] private GameObject myPrefab4;
        [SerializeField] private GameObject myPrefab5;
        [SerializeField] private GameObject myPrefab6;

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
            myPrefabs = new GameObject[] { myPrefab1, myPrefab2, myPrefab3, myPrefab4, myPrefab5, myPrefab6 };

            for (int z = 1; z <= resourcesPerBin; z++)
            {
                for (int i = 1; i <= myPrefabs.Length; i++)
                {
                    InstantiateOneResource(i, true);
                }
            }
        }

        public void CreateResourceObject(string resourceObjectName, bool dropFromHeight)
        {
            //Debug.Log($"Creating resource {resourceObjectName}");

            for (int i = 1; i <= myPrefabs.Length; i++)
            {
                if (myPrefabs[i - 1].name == resourceObjectName)
                {
                    InstantiateOneResource(i, dropFromHeight);
                }
            }
        }

        private void InstantiateOneResource(int i, bool dropFromHeight)
        {
            var boxName = $"ResourceBin{i}";
            var box = GameObject.Find(boxName);

            var xPos = box.transform.position.x;
            var yPos = box.transform.position.y + (dropFromHeight ? 0.5f : 0);
            var zPos = box.transform.position.z;

            var myPrefab = myPrefabs[i - 1];

            //Debug.Log($"Instantiating {myPrefab.name} into {boxName}");

            var newGameObject= Instantiate(myPrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity);
        }
    }
}