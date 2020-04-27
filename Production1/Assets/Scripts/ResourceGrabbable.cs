using Com.MachineApps.PrepareAndDeploy.Enums;
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
            Debug.Log($"GrabEvent: {gameObject.name} grabbed by {hand.name}");

            var currentPlayer = PlayerManager.instance.GetCurrentPlayer();
            var resourceManager = gameObject.GetComponent<ResourceManager>();
            var myResourceId = resourceManager.MyResourceId;
            var resourceCost = GameManager.instance.GetResourceCost((Resource)myResourceId);
            var budgetRemaining = GameManager.instance.BudgetRemaining();

            if (budgetRemaining - resourceCost < 0)
            {
                GameManager.instance.HudMessage($"I'm sorry, {currentPlayer.PlayerName}, there is not enough money left! Why not stage a fund-raising event?", 5);
                GameManager.instance.PlayAudio("notEnoughMoneyLeft");
                OVRInput.SetControllerVibration(0.5f, 0.5f, OVRInput.Controller.RTouch);
            }
            else if (budgetRemaining <= 0) // Shouldn't get to this point
            {
                GameManager.instance.HudMessage($"I'm sorry, {currentPlayer.PlayerName}, there is not enough money left!", 5);
                OVRInput.SetControllerVibration(0.5f, 0.5f, OVRInput.Controller.RTouch);
            }
            else
            {
                m_grabbedBy = hand;
                m_grabbedCollider = grabPoint;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;

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
