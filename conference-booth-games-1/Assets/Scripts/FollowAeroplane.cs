using UnityEngine;

namespace Assets.Scripts
{
    public class FollowAeroplane : MonoBehaviour
    {
        public Transform aeroplane;
        public Vector3 offset;
        public bool startFollowing;

        void Update()
        {
            if (startFollowing)
            {
                transform.position = aeroplane.position + offset;
            }
        }

        public void RotateCamera()
        {
                transform.eulerAngles =
                    Vector3.Lerp(transform.eulerAngles, new Vector3(0, -90, 0), Time.deltaTime * 0.5f);
        }
    }
}