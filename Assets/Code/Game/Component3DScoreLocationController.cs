using UnityEngine;

namespace Assets.Code.Game
{
    public class Component3DScoreLocationController : MonoBehaviour
    {
        public static Component3DScoreLocationController Instance { get; private set; }

        public float zDistance;
        public float yPercentDistance;

        public void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            RelocateAndResize();
        }

        private void RelocateAndResize()
        {
            Vector3[] corners = new Vector3[4];

            Camera.main.CalculateFrustumCorners(new Rect(0, 0, 1, 1), zDistance, Camera.MonoOrStereoscopicEye.Mono, corners);


            float width = corners[3].x - corners[0].x;
            float height = corners[2].x - corners[0].x;

            float x = corners[0].x + width / 2f;

            float y = corners[0].y + height * yPercentDistance;

            transform.localPosition = new Vector3(x, y, zDistance);
        }

    }
}
