using UnityEngine;

namespace Assets.Code
{
    public class ComponentExplosiveForce : MonoBehaviour
    {
        public Vector3 center;
        public float force = 10f;

        public void Start()
        {
            Rigidbody[] rb = transform.GetComponentsInChildren<Rigidbody>();
            foreach(Rigidbody r in rb)
            {
                r.AddExplosionForce(force, center, 10.0f);
            }
        }

    }
}
