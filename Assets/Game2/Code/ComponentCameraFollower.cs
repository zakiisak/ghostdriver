using Unity.Netcode;
using UnityEngine;

namespace Assets2.Code
{
    public class ComponentCameraFollower : NetworkBehaviour
    {
        private float xAngle = 30;
        public float Zoom = 50f;

        public void Start()
        {
        }

        public void LateUpdate()
        {
            if(IsOwner)
            {
                Vector3 direction = Camera.main.transform.forward;
                Camera.main.transform.rotation = Quaternion.Euler(xAngle, IsServer ? 0 : 180, 0);
                Camera.main.transform.position = transform.position - direction * Zoom;
            }
        }

    }
}
