using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class TestGrabbable : OVRGrabbable
    {
        /// <summary>
        /// Notifies the object that it has been grabbed.
        /// </summary>
        public void GrabBegin(OVRGrabber hand, Collider grabPoint)
        {
            m_grabbedBy = hand;
            m_grabbedCollider = grabPoint;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;

            testGrab(hand, grabPoint);
        }

        void testGrab(OVRGrabber hand, Collider grabPoint)
        {
            Debug.Log($"gameObject: {gameObject.name} grabbed by {hand.name}");



            var unitList = GameObject.FindGameObjectsWithTag(grabPoint.tag);

            var u = grabPoint.gameObject;

            var t = u.GetComponent("ReportManager");
        }
    }
}
