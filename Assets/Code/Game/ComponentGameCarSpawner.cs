using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentGameCarSpawner : MonoBehaviour
    {
        public void Start()
        {
            int count = Random.Range(3, 5);
            for (int i = 0; i < count; i++)
                DetermineNextObject(true);
        }

        public void Update()
        {
            if (CarRoadController.LocalInstance != null)
            {
                float progressMultiplier = 1.0f + CarRoadController.LocalInstance.transform.position.z / 400.0f;

                int count = (int)(2 + 4.0f * progressMultiplier);

                if (transform.childCount < count)
                {
                    DetermineNextObject(false);
                }
            }
        }

        private float GetProgressMultiplier()
        {
            float progressMultiplier = 1.0f + CarRoadController.LocalInstance.transform.position.z / 400.0f;
            return progressMultiplier;
        }

        private void DetermineNextObject(bool start)
        {
            float rng = Random.Range(0, 1.0f);
            if (rng <= 0.65f)
                DetermineNextCar(false);
            else if (rng <= 0.95f)
                SpawnWall(false);
            else SpawnRamp(false);
        }

        private const float _border = 300.0f;

        private void DetermineNextCar(bool start)
        {
            float progressMultiplier = CarRoadController.LocalInstance.transform.position.z / 400.0f;

            Vector3 position = GenerateLanePosition(start);
            position.y = 1.65f;

            SpawnCar(position, Random.Range(0, 1.0f) >= 0.5f, progressMultiplier);
        }

        private Vector3 GenerateLanePosition(bool start)
        {
            float border;
            if (start)
                border = 100f;
            else border = _border;

            int laneIndex = Random.Range(0, CarRoadController.maxLaneIndex + 1);

            float x = CarRoadController.leftMostLaneX + laneIndex * CarRoadController.laneGap;

            Vector3 position = new Vector3(x, 0, CarRoadController.LocalInstance.transform.position.z + border + Random.Range(0, 300.0f * GetProgressMultiplier()));
            return position;
        }

        private void SpawnWall(bool start)
        {
            const float y = 2.8f;
            Vector3 position = GenerateLanePosition(start);
            position.y = y;

            Instantiate(PrefabManager.Instance.Wall, transform).transform.position = position;
        }

        private void SpawnRamp(bool start)
        {
            const float y = 0.0f;
            Vector3 position = GenerateLanePosition(start);
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

            ParticleSystem system = car.transform.GetComponentInChildren<ParticleSystem>();
            ParticleSystem.EmissionModule emission = system.emission;
            emission.rateOverTimeMultiplier = 0;
            emission.rateOverDistanceMultiplier = 0;


            //AudioSource source = car.AddComponent<AudioSource>();
            //source.clip = Resources.Load<AudioClip>("Sounds/engine_1");
            //source.loop = true;
            //source.spatialBlend = 1.0f;
            //source.spatialize = true;
            //source.volume = 1.0f;
            //source.maxDistance = 50.0f;
            //source.Play();
        }
    }
}
