using UnityEngine;

namespace Assets.Scripts
{
    public class CassetteInstantiator : MonoBehaviour
    {
        // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
        public GameObject myPrefab;

        public static CassetteInstantiator instance;

        void Awake()
        {
            // Check that it exists
            if (instance == null)
            {
                //assign it to the current object
                instance = this;
            }

            // make sure that it is equal to the current object
            //else if (instance != this)
            //{
            //    // Destroy current game object - we only need one and we already have it
            //    Destroy(gameObject);
            //}

            // don't destroy the object when changing scenes!
            //DontDestroyOnLoad(gameObject);
        }

        // This script will simply instantiate the Prefab when the game starts.
        void Start()
        {
            CreateCassette();
        }

        public void CreateCassette()
        {
            Instantiate(myPrefab, new Vector3(0.08f, 3.5f, 2.3f), Quaternion.identity);
        }
    }
}