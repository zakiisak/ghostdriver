using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentTruck : MonoBehaviour
    {

        private float scalePercentage = 0;
        public float timeLeft = 5.0f;

        private GameObject truck;

        private float scaleModifier = 1.0f;

        public void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == "tree")
            {
                Instantiate(PrefabManager.Instance.Explosion, collision.gameObject.transform.position, Quaternion.identity);
                Destroy(collision.gameObject);
            }
        }


        private Rigidbody rb;

        public void Awake()
        {
            truck = Instantiate(PrefabManager.Instance.Truck, transform);
            truck.transform.localScale = Vector3.zero;

            PlayerController player = GetComponent<PlayerController>();
            if(player != null)
            {
                player.TurnOffLights();
            }

            rb = GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.freezeRotation = true;
            }
        }

        public void Upgrade()
        {
            timeLeft += 5.0f;
            scaleModifier += 0.5f;
        }

        public void Update()
        {
            if(timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                if (scalePercentage < 1f)
                {
                    scalePercentage += Time.deltaTime;
                    UpdateScale();
                }
            }
            else
            {
                if (scalePercentage > 0)
                {
                    scalePercentage -= Time.deltaTime;
                    UpdateScale();
                }
                else
                {
                    PlayerController player = GetComponent<PlayerController>();
                    if (player != null)
                    {
                        player.TurnOnLights();
                    }
                    Destroy(this);
                }
            }
        }

        public void OnDestroy()
        {
            rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.freezeRotation = false;
            }
            Destroy(truck);
        }

        private void UpdateScale()
        {
            float s = Mathf.Clamp(scalePercentage, 0, 1);
            s *= scaleModifier;
            truck.transform.localScale = new Vector3(s, s, s);
        }
    }
}
