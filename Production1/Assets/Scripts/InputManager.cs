using UnityEngine;
using UnityEngine.UI;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class InputManager : MonoBehaviour
    {

        [SerializeField] private InputField playerNameInputField;

        public void OnKeyEnter()
        {
            var playerName = playerNameInputField.text;

            if (playerName != string.Empty)
            {
                var kb = GameObject.Find("FormKeyboard-L1");
                Destroy(kb);

                GameManager.instance.PersonalMessage(playerName);

            }
        }
    }
}
