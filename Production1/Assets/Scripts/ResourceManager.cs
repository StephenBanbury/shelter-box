using System;
using Com.MachineApps.PrepareAndDeploy.Enums;
using UnityEngine;
using Random = System.Random;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class ResourceManager : MonoBehaviour
    {
        public int myResourceId;

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
            else if (gameObject.CompareTag("Water"))
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

        private Resource RandomResource()
        {
            Array values = Enum.GetValues(typeof(Resource));
            Random random = new Random();
            Resource randomValue = (Resource)values.GetValue(random.Next(1, values.Length - 1));
            return randomValue;
        }

    }

}