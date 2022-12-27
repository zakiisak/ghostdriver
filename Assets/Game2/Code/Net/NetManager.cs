using Assets2.Code;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Netcode.NetworkManager;

namespace Assets2.Code.Net
{
    public class NetManager : NetworkBehaviour
    {
        public NetworkVariable<float> CollisionPercentage = new NetworkVariable<float>(0);
        public NetworkVariable<bool> Started = new NetworkVariable<bool>(false);
        public static NetManager Instance;
        public const float CarExtent = 300f;

        private float startTimer = 2f;

        private const float maxMovementSpeed = 0.4f;
        private float movementSpeed = 0;
        private float accelerationSpeed = 0.04f;


        public void Awake()
        {
            Instance = this;
            CollisionPercentage.OnValueChanged = OnCollisionPercentageChanged;
            Started.OnValueChanged = (b, a) =>
            {
                if (a)
                {
                    Assets.Code.SoundManager.BeginPlayingEngine();
                }
            };
        }

        public void SetGameEnded()
        {
            gameEnded = true;
        }

        public void Start()
        {
            if(Unity.Netcode.NetworkManager.Singleton.IsServer)
            {
                Transform waitingText = GameObject.Find("Canvas").transform.Find("WaitingText");

                if (waitingText != null)
                    waitingText.gameObject.SetActive(true);


                Unity.Netcode.NetworkManager.Singleton.ConnectionApprovalCallback = OnClientConnecting;

                Unity.Netcode.NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadCompleted;
            }

            Unity.Netcode.NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
            Unity.Netcode.NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadComplete;
        }

        private bool hasSpawnedClient = false;

        private void OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
        {
            if (sceneName == "CrashGame" || sceneName.ToLower() == "crashgame")
            {
                if (clientId != Unity.Netcode.NetworkManager.Singleton.LocalClientId)
                {
                    if(!hasSpawnedClient)
                    {
                        OnClientConnected(clientId);
                        hasSpawnedClient = true;

                    }
                }
            }
        }

        private void OnLoadCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {

        }

        private void OnClientDisconnected(ulong obj)
        {
            try
            {
                if(Unity.Netcode.NetworkManager.Singleton.ShutdownInProgress == false)
                {
                    Unity.Netcode.NetworkManager.Singleton.Shutdown();
                }
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
            SceneManager.LoadScene(Game2.TitlescreenBuildIndex);

        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
        }

        private void OnCollisionPercentageChanged(float before, float now)
        {
            CarRoadController[] controllers = GameObject.FindObjectsOfType<CarRoadController>();
            foreach (CarRoadController controller in controllers)
            {
                controller.RepositionCar(now);
            }
        }

        private void OnClientConnecting(ConnectionApprovalRequest request, ConnectionApprovalResponse response)
        {
            //We can only have 2 clients playing at a time
            if (Unity.Netcode.NetworkManager.Singleton.ConnectedClients.Count >= 2)
            {
                response.Approved = false;
            }
            else response.Approved = true;
        }

        private float lastTimeCheckedForClients;

        private bool gameEnded = false;
        private void LeaveGame()
        {
            NetworkManager.Shutdown();
            SceneManager.LoadScene(Game2.TitlescreenBuildIndex);
        }


        public void Update()
        {
            if(IsServer && !gameEnded)
            {
                UpdateCars();
            }


            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LeaveGame();
            }
        }

        private bool hasSentWontNotification;

        private void UpdateCars()
        {
            if (Started.Value)
            {
                if (startTimer <= 0f)
                {
                    if (CollisionPercentage.Value < 1.1f)
                    {
                        //Accelerate
                        movementSpeed += accelerationSpeed * Time.deltaTime;
                        if (movementSpeed > maxMovementSpeed)
                            movementSpeed = maxMovementSpeed;

                        CollisionPercentage.Value += movementSpeed * Time.deltaTime;
                    }
                    else if (movementSpeed > 0)
                    {
                        CollisionPercentage.Value += movementSpeed * Time.deltaTime;

                        //Deccelerate
                        movementSpeed -= accelerationSpeed * 3f * Time.deltaTime;
                    }
                    else if (hasSentWontNotification == false)
                    {
                        //Finish the game and the host has won
                        NotifyClientLossClientRpc();
                        Game2.OnServerScore(true);
                        gameEnded = true;
                        hasSentWontNotification = true;
                    }
                }
                else
                    startTimer -= Time.deltaTime;
            }
        }

        [ClientRpc]
        private void NotifyClientLossClientRpc()
        {
            if (!IsHost)
                Game2.OnClientScore(false);
            //Stop all car noise
            Assets.Code.SoundManager.StopPlayingEngine();
        }

        private void OnClientConnected(ulong clientId)
        {
            GameObject client = Instantiate(PrefabManager2.Instance.RedCar, new Vector3(3, 2, CarExtent), Quaternion.Euler(0, 180, 0));
            client.GetComponent<NetworkObject>().SpawnWithOwnership(clientId, true);

            GameObject waitingText = GameObject.Find("WaitingText");

            if(waitingText != null)
                waitingText.SetActive(false);

            SpawnServerCar();
            Started.Value = true;

        }

        private void SpawnServerCar()
        {
            GameObject client = Instantiate(PrefabManager2.Instance.BlueCar, new Vector3(-2.38f, 2, -CarExtent), Quaternion.identity);
            client.GetComponent<NetworkObject>().Spawn(true);
        }

    }
}
