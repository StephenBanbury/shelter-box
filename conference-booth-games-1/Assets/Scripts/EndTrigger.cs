using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class EndTrigger : MonoBehaviour
    {
        void OnTriggerEnter()
        {
            GameManager.instance.GameOver();
        }

    }
}