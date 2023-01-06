using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentGameObstacleSpawner : MonoBehaviour
    {
        public void Start()
        {
            int count = Random.Range(3, 5);
            for (int i = 0; i < count; i++)
                DetermineNextObject(true);
        }

        public void FixedUpdate()
        {
            if (PlayerController.LocalInstance != null)
            {
                float progressMultiplier = 1.0f + Mathf.Floor(PlayerController.LocalInstance.transform.position.z) / 400.0f;
                if (progressMultiplier > 5f)
                    progressMultiplier = 5f;

                int count = (int)(2 + 5.0f * progressMultiplier);

                Debug.Log("child count: " + transform.childCount + " / " + count);

                if (transform.childCount < count)
                {
                    DetermineNextObject(false);
                }
            }
        }

        private float GetProgressMultiplier()
        {
            float progressMultiplier = 1.0f + Mathf.Floor(PlayerController.LocalInstance.transform.position.z) / 600.0f;
            if (progressMultiplier > 4.0f)
                progressMultiplier = 4.0f;
            return progressMultiplier;
        }

        private int lastHundredthSpawnedStar = 0;

        private void DetermineNextObject(bool start)
        {
            float rng = Random.Range(0, 1.0f);
            if (rng <= 0.65f)
                DetermineNextCar(false);
            else if (rng <= 0.95f)
                SpawnWall(false);
            else
            {
                if (Random.Range(0, 2) == 0)
                    SpawnRamp(false);
                else SpawnStub(false);
            }

            int hundredth = ComponentScoreController.Score / 100;
            if(hundredth != lastHundredthSpawnedStar)
            {
                if(SpawnStar(false))
                    lastHundredthSpawnedStar = hundredth;
            }
        }

        private const float _border = 600.0f;

        private void DetermineNextCar(bool start)
        {
            float progressMultiplier = Mathf.Floor(PlayerController.LocalInstance.transform.position.z) / 600.0f;

            Vector3 position = GenerateLanePosition(start, start ? 100 : _border);
            position.y = 1.65f;

            SpawnCar(position, Random.Range(0, 1.0f) >= 0.5f, progressMultiplier);
        }

        private void SpawnStub(bool start)
        {
            Vector3 position = GenerateLanePosition(start, start ? 20f : _border);
            GameObject stub = new GameObject();
            stub.transform.parent = transform;
            stub.AddComponent<ComponentObjectDespawner>();
        }

        private Vector3 GenerateLanePosition(bool start, float border)
        {
            int laneIndex = Random.Range(0, PlayerController.maxLaneIndex + 1);

            float x = PlayerController.leftMostLaneX + laneIndex * PlayerController.laneGap;

            Vector3 position = new Vector3(x, 0, Mathf.Floor(PlayerController.LocalInstance.transform.position.z) + border + Random.Range(0, 250.0f * GetProgressMultiplier()));
            return position;
        }

        private bool SpawnStar(bool start)
        {
            const float y = 2.8f;
            Vector3 position = GenerateLanePosition(start, start ? 20f : _border);
            position.y = y;

            Collider[] hitColliders = Physics.OverlapSphere(position, 10);
            foreach (Collider collider in hitColliders)
            {
                if (collider.gameObject.tag == "ramp" || collider.gameObject.tag == "destroyer")
                    return false;
            }

            Instantiate(PrefabManager.Instance.Star, transform).transform.position = position + new Vector3(0, 3.2f, 0);
            return true;
        }

        private void SpawnWall(bool start)
        {
            const float y = 2.8f;
            Vector3 position = GenerateLanePosition(start, start ? 20f : _border);
            position.y = y;

            Collider[] hitColliders = Physics.OverlapSphere(position, 10);
            foreach(Collider collider in hitColliders)
            {
                if (collider.gameObject.tag == "ramp")
                    return;
            }

            Instantiate(PrefabManager.Instance.Wall, transform).transform.position = position;
        }

        private void SpawnRamp(bool start)
        {
            const float y = 0.0f;
            Vector3 position = GenerateLanePosition(start, start ? 20f : _border);
            position.y = y;

            Collider[] hitColliders = Physics.OverlapSphere(position, 10);
            foreach (Collider collider in hitColliders)
            {
                if (collider.gameObject.tag == "destroyer")
                    return;
            }

            Instantiate(PrefabManager.Instance.Ramp, transform).transform.position = position;
        }

        private void SpawnCar(Vector3 position, bool red, float progressMultiplier)
        {
            GameObject prefab;
            if (red)
                prefab = PrefabManager.Instance.RedCar;
            else
                prefab = PrefabManager.Instance.BlueCar;

            float speed = Mathf.Max(Mathf.Min(16.0f * progressMultiplier, 64.0f), 64.0f);

            GameObject car = Instantiate(prefab, position, Quaternion.Euler(0, 180, 0));
            car.transform.SetParent(transform, true);
            car.AddComponent<ComponentEnemyCarController>().Speed = speed;
            car.AddComponent<ComponentObjectDespawner>();
        }
    }
}
