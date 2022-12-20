using UnityEngine;

namespace Assets.Code.Titlescreen
{
    public class ComponentCarTitlescreenController : MonoBehaviour
    {
        public bool goingForward;

        private Vector3 direction;
        private const float speed = 32.0f;

        public void Start()
        {
            direction = goingForward ? new Vector3(0, 0, 1) : new Vector3(0, 0, -1);
        }

        public void Update()
        {
            Move();
            UpdateDespawnBounds();
        }

        private void Move()
        {
            this.transform.position += direction * (speed * Time.deltaTime);
        }

        private void UpdateDespawnBounds()
        {
            if(transform.position.z < -100 || transform.position.z > 100)
            {
                Destroy(gameObject);
            }
        }

        public void OnDestroy()
        {
            ComponentTitlescreenCarSpawner.CarCount--;
        }


    }
}
