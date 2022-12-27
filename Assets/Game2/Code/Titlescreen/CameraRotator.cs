using UnityEngine;

namespace Assets2.Code.Titlescreen
{
    public class CameraRotator : MonoBehaviour
    {
        public Camera camera;
        public Transform pivot;
        public float RotationSpeed = 30f;

        private float degrees = 0;
        private float distance;

        public void Start()
        {
            distance = (pivot.position - transform.position).magnitude;    
        }


        public void Update()
        {
            degrees += Time.deltaTime * RotationSpeed;

            Vector2 circle = GetCircle(degrees) * distance;
            transform.position = new Vector3(pivot.position.x + circle.x, transform.position.y, pivot.position.z + circle.y);
            transform.LookAt(pivot);
            
        }

        public static Vector2 GetCircle(float degrees)
        {
            return new Vector2(Mathf.Cos(degrees * Mathf.Deg2Rad), Mathf.Sin(degrees * Mathf.Deg2Rad));
        }

    }
}
