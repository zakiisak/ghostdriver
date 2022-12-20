using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentTreeSpawner : MonoBehaviour
    {
        private int maxTreeAmount = 150;

        public void Start()
        {
            for (int i = 0; i < maxTreeAmount; i++)
                SpawnTree(true);
        }

        public void Update()
        {
            if(CarRoadController.LocalInstance != null && transform.childCount < maxTreeAmount)
            {
                SpawnTree(false);
            }
        }

        private void SpawnTree(bool start)
        {
            const float xGapThreshold = 12.0f;
            const float xViewWidth = 100.0f;
            const float maxForwardZ = 500f;

            float minZ;
            if (start)
                minZ = CarRoadController.LocalInstance.transform.position.z - 10f;
            else minZ = CarRoadController.LocalInstance.transform.position.z + maxForwardZ;
            float z = minZ + Random.Range(0, maxForwardZ);

            float x = Random.Range(0, xViewWidth) - xViewWidth / 2;
            x += Mathf.Sign(x) * xGapThreshold;

            Instantiate(PrefabManager.Instance.Tree, transform, true).transform.position = new Vector3(x, 0, z);
        }

    }
}
