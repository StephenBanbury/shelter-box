﻿using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class AeroplaneController : MonoBehaviour
    {
        public Rigidbody rb;

        private float forwardForce = 0f;
        private float upwardForce = 0f;

        public bool startTaxi;

        void Start()
        {
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (startTaxi)
            {
                // Add a forward force
                rb.AddForce(- forwardForce * Time.deltaTime, upwardForce * Time.deltaTime, 0);
                
                forwardForce += 0.2f;

                if (forwardForce >= 130f)
                {
                    upwardForce += 0.2f;
                }

                //if (Input.GetKey("d"))
                //{
                //    rb.AddForce(0, 0, sidewaysForce * Time.deltaTime, ForceMode.VelocityChange);
                //}

                //if (Input.GetKey("a"))
                //{
                //    rb.AddForce( 0, 0, -sidewaysForce * Time.deltaTime, ForceMode.VelocityChange);
                //}
            }
        }

        public void StartTaxi()
        {
            startTaxi = false;

            var audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.Play();

            StartCoroutine(DelayTakeoff(5));
        }

        IEnumerator DelayTakeoff(int seconds)
        {
            yield return new WaitForSeconds(seconds);
            startTaxi = true;
        } 
    }
}