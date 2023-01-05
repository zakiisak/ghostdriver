using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentTreeSpawner : MonoBehaviour
    {
        public static int maxTreeAmount = 500; //150

        public static float xViewWidth;

        public void Start()
        {
            SetViewWidth();

            for (int i = 0; i < maxTreeAmount; i++)
                SpawnTree(true);

        }

        private void SetViewWidth()
        {
            float frustumHeight = 2.0f * 300f * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
            xViewWidth = frustumHeight * Camera.main.aspect;
            maxTreeAmount = (int) (xViewWidth * 2);
            Debug.Log("Frustom width: " + xViewWidth);
        }

        public void FixedUpdate()
        {
            SetViewWidth();

            if (CarRoadController.LocalInstance != null && transform.childCount < maxTreeAmount)
            {
                SpawnTree(false);
            }
        }

        private void SpawnTree(bool start)
        {
            const float xGapThreshold = 12.0f;
            const float maxForwardZ = 500f;

            float minZ;
            if (start)
                minZ = (int) CarRoadController.LocalInstance.transform.position.z - 10f;
            else minZ = (int) CarRoadController.LocalInstance.transform.position.z + maxForwardZ;
            float z = minZ + Random.Range(0, maxForwardZ);

            float x = Random.Range(0, xViewWidth) - xViewWidth / 2;
            x += Mathf.Sign(x) * xGapThreshold;

            Instantiate(PrefabManager.Instance.Tree, transform, true).transform.position = new Vector3(x, 0, z);
        }

    }
}
