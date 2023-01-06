using UnityEngine;

namespace Assets.Code.Game
{
    public class RedBoxController : MonoBehaviour
    {

        public void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.GetComponent<ComponentTruck>() != null || collision.gameObject.tag == "truck")
            {
                Instantiate(PrefabManager.Instance.Explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
