
using UnityEngine;

namespace Assets.Scripts
{
    public class AeroplaneController : MonoBehaviour
    {
        public Rigidbody rb;

        private float forwardForce = 0f;
        private float upwardForce = 0f;
        //private float sidewaysForce = 5f;

        public bool startTaxi;

        // Start is called before the first frame update
        void Start()
        {
            //startTaxi = true;
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
                    upwardForce += 0.1f;
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

        //public void StartTaxi()
        //{
        //    // Add a forward force
        //    forwardForce = 1f;
        //    upwardForce = 0f;
        //    startTaxi = true;
        //}
    }
}