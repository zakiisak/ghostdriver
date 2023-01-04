using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentGameCarSpawner : MonoBehaviour
    {
        public void Start()
        {
            int count = PredictableRandom.Range(3, 5);
            for (int i = 0; i < count; i++)
                DetermineNextObject(true);
        }

        public void FixedUpdate()
        {
            if (CarRoadController.LocalInstance != null)
            {
                float progressMultiplier = 1.0f + Mathf.Floor(CarRoadController.LocalInstance.transform.position.z) / 400.0f;
                if (progressMultiplier > 5f)
                    progressMultiplier = 5f;

                int count = (int)(2 + 5.0f * progressMultiplier);

                if (transform.childCount < count)
                {
                    DetermineNextObject(false);
                }
            }
        }

        private float GetProgressMultiplier()
        {
            float progressMultiplier = 1.0f + Mathf.Floor(CarRoadController.LocalInstance.transform.position.z) / 600.0f;
            if (progressMultiplier > 4.0f)
                progressMultiplier = 4.0f;
            return progressMultiplier;
        }

        private void DetermineNextObject(bool start)
        {
            float rng = PredictableRandom.Range(0, 1.0f);
            if (rng <= 0.65f)
                DetermineNextCar(false);
            else if (rng <= 0.95f)
                SpawnWall(false);
            else SpawnRamp(false);
        }

        private const float _border = 400.0f;

        private void DetermineNextCar(bool start)
        {
            float progressMultiplier = Mathf.Floor(CarRoadController.LocalInstance.transform.position.z) / 600.0f;

            Vector3 position = GenerateLanePosition(start, start ? 100 : _border);
            position.y = 1.65f;

            SpawnCar(position, PredictableRandom.Range(0, 1.0f) >= 0.5f, progressMultiplier);
        }

        private Vector3 GenerateLanePosition(bool start, float border)
        {
            int laneIndex = PredictableRandom.Range(0, CarRoadController.maxLaneIndex + 1);

            float x = CarRoadController.leftMostLaneX + laneIndex * CarRoadController.laneGap;

            Vector3 position = new Vector3(x, 0, Mathf.Floor(CarRoadController.LocalInstance.transform.position.z) + border + PredictableRandom.Range(0, 300.0f * GetProgressMultiplier()));
            return position;
        }

        private void SpawnWall(bool start)
        {
            const float y = 2.8f;
            Vector3 position = GenerateLanePosition(start, start ? 20f : _border);
            position.y = y;

            Collider[] hitColliders = Physics.OverlapSphere(position, 2);
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
