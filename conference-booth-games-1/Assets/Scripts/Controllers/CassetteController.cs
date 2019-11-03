using System;
using System.Text.RegularExpressions;
using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Assets.Scripts.Controllers
{

    public class CassetteController : MonoBehaviour
    {
        public Text cassetteText;
        public int myResourceId;

        // Start is called before the first frame update
        void Start()
        {
            var myResource = RandomResource();
            myResourceId = (int) myResource;

            if (gameObject.CompareTag("Cassette"))
            {
                var displayText = Regex.Replace(myResource.ToString(), "(\\B[A-Z])", " $1");
                cassetteText.text = displayText;
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