using System;
using Assets.Scripts.Enums;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{

    public class ResourceManager : MonoBehaviour
    {
        //public Text cassetteText;
        public int myResourceId;

        // Start is called before the first frame update
        void Start()
        {
            //if (gameObject.CompareTag("Cassette"))
            //{
            //    var randomResource = RandomResource();
            //    myResourceId = (int) randomResource;

            //    var displayText = Regex.Replace(randomResource.ToString(), "(\\B[A-Z])", " $1");
            //    cassetteText.text = displayText;
            //}
            //else 
            
            if (gameObject.CompareTag("Tent"))
            {
                myResourceId = (int)Resource.Tents;
            }
            else if (gameObject.CompareTag("Bottle"))
            {
                myResourceId = (int)Resource.Water;
            }
            else if (gameObject.CompareTag("Food"))
            {
                myResourceId = (int)Resource.Food;
            }
            else if (gameObject.CompareTag("FirstAidKit"))
            {
                myResourceId = (int)Resource.FirstAidKits;
            }
            else if (gameObject.CompareTag("Boat"))
            {
                myResourceId = (int)Resource.Boats;
            }
            else if (gameObject.CompareTag("Toy"))
            {
                myResourceId = (int)Resource.Toys;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private Resource RandomResource()
        {
            Array values = Enum.GetValues(typeof(Resource));
            Random random = new Random();
            Resource randomValue = (Resource)values.GetValue(random.Next(1, values.Length - 1));
            return randomValue;
        }

    }

}