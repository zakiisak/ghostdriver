using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentRoadSpawner : MonoBehaviour
    {

        private int maxRoadCount = 10;
        private const float roadPartLength = 100f;

        private float partZ = roadPartLength * 0.4f;

        public void Start()
        {
            for (int i = 0; i < maxRoadCount; i++)
                SpawnRoad();
        }

        public void Update()
        {
            if (transform.childCount < maxRoadCount)
                SpawnRoad();
        }

        private void SpawnRoad()
        {
            Vector3 position = new Vector3(0, 0.39f, partZ);
            Instantiate(PrefabManager.Instance.Road, transform, true).transform.position = position;
            partZ += roadPartLength;
        }
    }
}
