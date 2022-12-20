using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentEnemyCarController : MonoBehaviour
    {
        public Vector3 Direction;
        public float Speed = 1.0f;

        private Vector3 impulseAdd = Vector3.zero;

        public void OnCollisionEnter(Collision collision)
        {
            string tag = collision.gameObject.tag;
            if (tag == "destroyer" || tag == "ramp")
            {
                Explode();
            }

            //impulseAdd += collision.impulse;
        }

        public void Explode()
        {
            Destroy(gameObject);
            Instantiate(PrefabManager.Instance.Explosion, transform.position, Quaternion.identity);
            Instantiate(PrefabManager.Instance.FracturedBlueCar, transform.position, Quaternion.identity).AddComponent<ComponentDespawnDelay>().Delay = 5.0f;
        }


        public void Update()
        {
            transform.position += (transform.forward * Speed + impulseAdd) * Time.deltaTime;
        }
    }
}
