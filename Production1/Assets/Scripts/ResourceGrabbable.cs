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
            if (GameManager.instance.BudgetRemaining <= 0)
            {
                GameManager.instance.HudMessage("You do not have any funds left!", 3);
                OVRInput.SetControllerVibration(0.5f, 0.5f, OVRInput.Controller.RTouch);
            }
            else
            {
                m_grabbedBy = hand;
                m_grabbedCollider = grabPoint;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;

                //Debug.Log($"GrabEvent: {gameObject.name} grabbed by {hand.name}");

                var resourceManager = gameObject.GetComponent<ResourceManager>();
                resourceManager.Grabbed(true);

                VibrationManager.instance.TriggerVibration(40, 2, 255, OVRInput.Controller.RTouch);

            }
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
        }
}
