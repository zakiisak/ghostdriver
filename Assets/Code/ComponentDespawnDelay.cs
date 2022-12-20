using UnityEngine;

namespace Assets.Code
{
    public class ComponentDespawnDelay : MonoBehaviour
    {
        public float Delay = 5.0f;

        public void Update()
        {
            Delay -= Time.deltaTime;

            if(Delay <= 0f)
            {
                Destroy(gameObject);
            }
        }

    }
}
