using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentStar : MonoBehaviour
    {

        public void OnTriggerEnter(Collider other)
        {
            //It's the player colliding
            if(other.gameObject.GetComponent<PlayerController>() != null)
            {
                Destroy(gameObject);
                Instantiate(PrefabManager.Instance.StarPickup, transform.position + new Vector3(0, 5, 0), Quaternion.identity);

                /*
                Collider[] objects = Physics.OverlapSphere(transform.position, 500);
                Debug.Log("Found " + objects.Length + " objectrs");
                foreach(Collider obj in objects)
                {
                    if (obj.gameObject.GetComponent<PlayerController>() != null)
                        continue;

                    Rigidbody rb = obj.GetComponent<Rigidbody>();
                    if(rb != null)
                    {
                        if (rb.isKinematic)
                            rb.isKinematic = false;

                        Vector3 normalizedDiff = (obj.transform.position - transform.position).normalized;
                        rb.velocity += normalizedDiff * 1000f;
                        rb.angularVelocity += normalizedDiff * 15f;
                    }
                }*/
                ComponentTruck existingTruck = other.gameObject.GetComponent<ComponentTruck>();
                if (existingTruck != null)
                {
                    existingTruck.Upgrade();
                }
                else 
                    other.gameObject.AddComponent<ComponentTruck>();

                //Make an effect somehow
            }
        }
    }
}
