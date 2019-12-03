using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class AffectedObject : MonoBehaviour
    {
        //public Text debugText;
        //public float lifeTime = 10f;
        public bool inWindZone = false;
        public GameObject windZone;

        private Rigidbody rb;
        private Vector3 trans;
        private GameObject go;

        private void Start()
        {
            windZone = GameObject.Find("Floor");
            go = gameObject;
            trans = gameObject.transform.position;
            rb = GetComponent<Rigidbody>();

            //print($"rb: {rb.name}");
        }

        void Update()
        {
            //if (lifeTime > 0)
            //{
            //    lifeTime -= Time.deltaTime;
            //    if (lifeTime <= 0)
            //    {
            //        Destruction();
            //    }
            //}

            //debugText.text = $"x: {this.transform.position.x.ToString()}, y: {this.transform.position.y.ToString()}, z{this.transform.position.z.ToString()}";

            if (this.transform.position.y <= -5)
            {
                //Destruction();

                //Instantiate(go, new Vector3(0.08f + randomPosition, 3.5f, 1.4f + randomPosition), Quaternion.identity);
                //Instantiate(go, new Vector3(trans.x, trans.y, trans.z), Quaternion.identity);

                this.transform.position = new Vector3(trans.x, trans.y, trans.z);
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.Sleep();
            }
        }

        void FixedUpdate()
        {
            //print($"inWindZone: {inWindZone.ToString()}");

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

        //void OnCollisionEnter(Collision coll)
        //{
        //    if (coll.gameObject.name == "destroyer")
        //    {
        //        Destruction();
        //    }
        //}

        void Destruction()
        {
            Destroy(this.gameObject);
        }
    }
}