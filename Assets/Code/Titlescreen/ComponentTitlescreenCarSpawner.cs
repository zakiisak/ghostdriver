using Unity.Netcode;
using UnityEngine;

namespace Assets.Code.Titlescreen
{
    public class ComponentTitlescreenCarSpawner : MonoBehaviour
    {
        public static int CarCount;

        public void Awake()
        {
            CarCount = 0;
        }

        public void Update()
        {
            if(CarCount <= 0f)
            {
                DetermineNextCars();
            }
        }


        private void DetermineNextCars()
        {
            int count = Random.Range(1, 3);
            for(int i = 0; i < count; i++)
            {
                bool goingForward = Random.Range(0, 2) == 0;

                float direction = goingForward ? -1 : 1;

                float border = 50.0f;

                float x = goingForward ? (PlayerController.leftMostLaneXTitlescreen + PlayerController.laneGap) : PlayerController.leftMostLaneXTitlescreen;

                Vector3 position = new Vector3(x, 1.65f, border * direction + Random.Range(0, 40.0f) * direction);

                SpawnCar(position, goingForward);

            }
        }

        private void SpawnCar(Vector3 position, bool goingForward)
        {
            GameObject prefab;
            if (goingForward)
                prefab = PrefabManager.Instance.RedCar;
            else
                prefab = PrefabManager.Instance.BlueCar;

            GameObject car = Instantiate(prefab, position, Quaternion.Euler(0, goingForward ? 0 : 180, 0));
            Destroy(car.GetComponent<ComponentCameraFollower>());
            Destroy(car.GetComponent<PlayerController>());
            Destroy(car.GetComponent<NetworkObject>());
            car.AddComponent<ComponentCarTitlescreenController>().goingForward = goingForward;

            CarCount++;
        }
    }
}
