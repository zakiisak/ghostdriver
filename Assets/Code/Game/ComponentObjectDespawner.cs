using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentObjectDespawner : MonoBehaviour
    {
        public float zDistanceBehindPlayer = 30.0f;

        public void Update()
        {

            UpdateDespawning();
        }

        private void UpdateDespawning()
        {
            if(CarRoadController.LocalInstance != null)
            {
                if (transform.position.z < CarRoadController.LocalInstance.transform.position.z - zDistanceBehindPlayer)
                    Destroy(gameObject);
            }
        }

    }
}
