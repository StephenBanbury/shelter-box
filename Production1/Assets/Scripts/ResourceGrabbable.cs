using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class ResourceGrabbable : OVRGrabbable
    {
        private GameObject grabbedObject;

        /// <summary>
        /// Notifies the object that it has been grabbed.
        /// Overrides OVRGrabbable so as to apply event.
        /// </summary>
        public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
        {
            m_grabbedBy = hand;
            m_grabbedCollider = grabPoint;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;

            //Debug.Log($"GrabEvent: {gameObject.name} grabbed by {hand.name}");
            //grabbedObject = grabPoint.gameObject;

            var resourceManager = gameObject.GetComponent<ResourceManager>();
            resourceManager.Grabbed(true);
        }

        /// <summary>
        /// Notifies the object that it has been released.
        /// </summary>
        public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = m_grabbedKinematic;
            rb.velocity = linearVelocity;
            rb.angularVelocity = angularVelocity;
            m_grabbedBy = null;
            m_grabbedCollider = null;

            var resourceManager = gameObject.GetComponent<ResourceManager>();
            resourceManager.Grabbed(false);
        }

        void GrabEvent(OVRGrabber hand, Collider grabPoint)
        {
        }


    }
}
