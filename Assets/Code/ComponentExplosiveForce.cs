using UnityEngine;

namespace Assets.Code
{
    public class ComponentExplosiveForce : MonoBehaviour
    {
        public Vector3 center;
        public float force = 1f;

        public void Start()
        {
            Rigidbody[] rb = transform.GetComponentsInChildren<Rigidbody>();
            foreach(Rigidbody r in rb)
            {
                r.velocity = r.transform.localPosition * force;
                Debug.Log("Fractured part velocity : " + r.velocity);
            }
        }

    }
}
