using Assets.Code;
using Assets2.Code.Net;
using Unity.Netcode;
using UnityEngine;

namespace Assets2.Code
{
    public class CarRoadController : NetworkBehaviour
    {
        public static CarRoadController LocalInstance { get; private set; }

        public NetworkVariable<bool> RightSide = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public bool Started { get; set; }

        public const float leftSideX = -2.38f;
        public const float rightSideX = 2.8f;

        private float collideCooldown;

        private bool controlsLocked;

        public void OnCollisionEnter(Collision collision)
        {
            if(IsServer)
            {
                CarRoadController otherCar = collision.gameObject.GetComponent<CarRoadController>();
                if (otherCar != null && Time.time - collideCooldown > 3.0f && otherCar.RightSide.Value == RightSide.Value)
                {
                    Destroy(gameObject);
                    Destroy(collision.gameObject);

                    //For clients
                    SpawnFracturedCarClientRpc(transform.position, false);
                    SpawnFracturedCarClientRpc(collision.gameObject.transform.position, true);

                    //for server
                    CrashCar();
                    SpawnFracturedCar(transform.position, false);
                    SpawnFracturedCar(collision.gameObject.transform.position, true);


                    NetManager.Instance.SetGameEnded();
                    //Client wins

                    NotifyClientWinsClientRpc();
                    Game2.OnServerScore(false);
                    //TODO Spawn explosion  
                }
            }
        }

        [ClientRpc]
        private void NotifyClientWinsClientRpc()
        {
            if (!IsHost)
                Game2.OnClientScore(true);
        }

        private void SpawnFracturedCar(Vector3 position, bool red)
        {
            Debug.Log("spawning fractured car!");
            GameObject prefab;
            if (red)
                prefab = PrefabManager2.Instance.FracturedRedCar;
            else
                prefab = PrefabManager2.Instance.FracturedBlueCar;
            Instantiate(prefab, position, transform.rotation);

            Instantiate(PrefabManager2.Instance.Explosion, position, Quaternion.identity);

        }

        [ClientRpc]
        private void SpawnFracturedCarClientRpc(Vector3 position, bool red)
        {
            SpawnFracturedCar(position, red);
            CrashCar();
        }

        private void CrashCar()
        {
            if (IsOwner)
            {
                SoundManager.PlayCrash();
                SoundManager.StopPlayingEngine();
            }
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                LocalInstance = this;
                StartEngine();
            }
        }

        public void Awake()
        {
            collideCooldown = Time.time;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }


        private bool hasPlayedHorns;
        private bool hasGoneBlack;
        private bool hasGoneLightAgain;

        private bool hasStoppedHorns;

        public void RepositionCar(float movementPercentage)
        {
            float direction;
            if(IsServer)
            {
                if (IsOwner)
                    direction = 1;
                else direction = -1;
            }
            else
            {
                if (IsOwner)
                    direction = -1;
                else direction = 1;
            }

            transform.position = new Vector3(transform.position.x, transform.position.y, NetManager.CarExtent * -direction + NetManager.CarExtent * direction * movementPercentage);

            if(IsOwner)
            {
                
                if(movementPercentage > 0.7f && !hasGoneBlack)
                {
                    Game2.GoPitchBlack();
                    hasGoneBlack = true;
                    SoundManager.PlayBuild();
                }
                /*
                if(movementPercentage > 0.7f && !hasGoneLightAgain)
                {
                    Game.ShowLightAgain();
                    hasGoneLightAgain = true;
                }
                */

                if (movementPercentage > 0.8f && hasPlayedHorns == false)
                {
                    PlayTireSounds();
                    hasPlayedHorns = true;
                }

                if(movementPercentage >= 0.88f && controlsLocked == false)
                {
                    //controlsLocked = true;
                }

                if (movementPercentage >= 1.1f && hasStoppedHorns == false)
                {
                    SoundManager.StopPlayingHorns();
                    SoundManager.StopPlayingTire();
                    SoundManager.StopPlayingBuild();
                    hasStoppedHorns = true;
                }

                float percentage = Mathf.Clamp(movementPercentage, 0, 1);

                float percentageSubtract = Mathf.Clamp(movementPercentage - 1.0f, 0, 1);
                percentage -= percentageSubtract;
                SoundManager.UpdateEnginePitch(percentage, percentage);
            }

        }

        private float timeToSwitchLightsOnOff;

        private void FlickerLights()
        {
            if(Time.time >= timeToSwitchLightsOnOff)
            {

                if (Game2.IsDark())
                    Game2.ShowLightAgain();
                else Game2.GoPitchBlack();

                timeToSwitchLightsOnOff = Time.time + Random.Range(0.1f, 0.3f);
            }
        }

        public void Update()
        {
            if(IsOwner)
            {
                if(Started)
                {
                    UpdateLaneShifts();
                }
            }
            UpdateLanePosition();
        }

        private void UpdateLaneShifts()
        {
            if (((Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)) && controlsLocked == false)
            {
                if (RightSide.Value)
                    GoToLeftSide();
                else GoToRightSide();
            }
        }

        private void UpdateLanePosition()
        {
            float destX = RightSide.Value ? rightSideX : leftSideX;

            transform.position = transform.position + new Vector3((destX - transform.position.x) * 7.0f * Time.deltaTime, 0, 0);
        }

        private void PlayTireSounds()
        {
            SoundManager.PlayTire();
            SoundManager.PlayHorn();
        }

        public void TurnOffLights()
        {
            Light[] lights = GetComponentsInChildren<Light>();
            foreach (Light light in lights)
                light.enabled = false;
        }

        public void TurnOnLights()
        {
            Light[] lights = GetComponentsInChildren<Light>();
            foreach (Light light in lights)
                light.enabled = false;
        }

        public void StartEngine()
        {
            Started = true;
            SoundManager.PlayEngineStart();
        }

        private void GoToRightSide()
        {
            RightSide.Value = true;
        }

        private void GoToLeftSide()
        {
            RightSide.Value = false;
        }

        [ServerRpc(RequireOwnership = false)]
        private void SendPositionServerRpc(Vector3 position)
        {

        }
    }
}
