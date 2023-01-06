using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentEnemyCarController : MonoBehaviour
    {
        public Vector3 Direction;
        public float Speed = 1.0f;

        private Vector3 impulseAdd = Vector3.zero;

        private float startY;

        public void Awake()
        {
            startY = transform.position.y;
        }

        public void OnCollisionEnter(Collision collision)
        {
            string tag = collision.gameObject.tag;
            if (tag == "destroyer" || tag == "ramp" || tag == "truck" || collision.gameObject.GetComponent<ComponentTruck>() != null)
            {
                Explode();
                if(collision.rigidbody != null && collision.gameObject.GetComponent<RedBoxController>() != null)
                    collision.rigidbody.velocity += transform.forward * Speed;
            }

            //impulseAdd += collision.impulse;
        }

        public void Explode()
        {
            Destroy(gameObject);
            Instantiate(PrefabManager.Instance.Explosion, transform.position, Quaternion.identity);
            Instantiate(PrefabManager.Instance.FracturedBlueCar, transform.position, Quaternion.identity).AddComponent<ComponentObjectDespawner>();
        }

        private float udpateRate = 0.02f;

        public void FixedUpdate()
        {
            transform.position += (transform.forward * Speed + impulseAdd) * Time.fixedDeltaTime;
            //PreventFromFallingThroughGround();
        }

        private void PreventFromFallingThroughGround()
        {
            if(transform.position.y < startY)
            {
                transform.position = new Vector3(transform.position.x, startY, transform.position.z);
            }
        }
    }
}
