using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace Assets.Scripts
{
    public class Ball : MonoBehaviour
    {

        public float lifeTime = 10f;
        public bool inWindZone = false;
        public GameObject windZone;

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (lifeTime > 0)
            {
                lifeTime -= Time.deltaTime;
                if (lifeTime <= 0)
                {
                    Destruction();
                }
            }

            if (this.transform.position.y <= -20)
            {
                Destruction();
            }
        }

        void FixedUpdate()
        {
            if (inWindZone)
            {
                var windArea = windZone.GetComponent<WindArea>();
                rb.AddForce(windArea.direction * windArea.strength);
            }
        }

        void OnTriggerEnter(Collider coll)
        {
            if (coll.gameObject.tag == "WindArea")
            {
                windZone = coll.gameObject;
                inWindZone = true;
            }
        }

        void OnTriggerExit(Collider coll)
        {
            if (coll.gameObject.tag == "WindArea")
            {
                inWindZone = false;
            }
        }

        void OnCollisionEnter(Collision coll)
        {
            if (coll.gameObject.name == "destroyer")
            {
                Destruction();
            }
        }

        void Destruction()
        {
            Destroy(this.gameObject);
        }
    }
}