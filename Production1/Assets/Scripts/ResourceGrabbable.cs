using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class ResourceGrabbable : OVRGrabbable
    {
        /// <summary>
        /// Notifies the object that it has been grabbed.
        /// Overrides OVRGrabbable so as to apply event.
        /// </summary>
        public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
        {
            m_grabbedBy = hand;
            m_grabbedCollider = grabPoint;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;

            GrabEvent(hand, grabPoint);
        }

        void GrabEvent(OVRGrabber hand, Collider grabPoint)
        {
            //Debug.Log($"GrabEvent: {gameObject.name} grabbed by {hand.name}");
            var resourceManager = grabPoint.gameObject.GetComponent<ResourceManager>();
            resourceManager.Grabbed();
        }
    }
}
