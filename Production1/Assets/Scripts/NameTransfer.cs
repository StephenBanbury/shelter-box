using UnityEngine;
using UnityEngine.UI;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class NameTransfer : MonoBehaviour
    {
        public string name;
        public GameObject inputField;
        public GameObject textDisplay;

        public void StoreName()
        {
            name = inputField.GetComponent<Text>().text;
            textDisplay.GetComponent<Text>().text = $"Welcome {name} to the game";
        }
    }
}