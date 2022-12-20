using Unity.Netcode;
using UnityEngine;

namespace Assets.Code
{
    public class ComponentCameraFollower : MonoBehaviour
    {
        private float xAngle = 30;
        public float Zoom = 50f;

        public void Start()
        {
        }

        public void LateUpdate()
        {
            Vector3 direction = Camera.main.transform.forward;
            Camera.main.transform.rotation = Quaternion.Euler(xAngle, 0, 0);
            Camera.main.transform.position = transform.position - direction * Zoom;
        }

    }
}
