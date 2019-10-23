using System;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Assets.Scripts.Controllers
{

    public class CassetteController : MonoBehaviour
    {
        public Text text;
        public int myReliefResourceId;

        // Start is called before the first frame update
        void Start()
        {
            var myReliefResource = RandomReliefResource();
            myReliefResourceId = (int) myReliefResource;

            print("myReliefResourceId : " + myReliefResourceId);

            var displayText = Regex.Replace(myReliefResource.ToString(), "(\\B[A-Z])", " $1");
            text.text = displayText;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private ReliefResource RandomReliefResource()
        {
            Array values = Enum.GetValues(typeof(ReliefResource));
            Random random = new Random();
            ReliefResource randomValue = (ReliefResource)values.GetValue(random.Next(1, values.Length - 1));
            return randomValue;
        }

    }

}