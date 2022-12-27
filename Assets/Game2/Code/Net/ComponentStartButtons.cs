using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets2.Code.Net
{
    public class ComponentStartButtons : MonoBehaviour
    {
        public TMP_InputField JoinField;
        public Button HostButton;
        public Button JoinButton;
        public TMP_Text TimeoutText;

        public void HostGame()
        {
            DisableEverything();
            string listenAddress = NetworkUtils.GetLocalIPv4();
            Debug.Log("Listen address " + listenAddress);
            Unity.Netcode.NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.ServerListenAddress = listenAddress;
            if(Unity.Netcode.NetworkManager.Singleton.StartServer())
            {
                //Navigate to new scene
                Unity.Netcode.NetworkManager.Singleton.SceneManager.LoadScene("CrashGame", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            else
            {
                EnableEverything();
            }
        }

        private void DisableEverything()
        {
            JoinField.interactable = false;
            HostButton.interactable = false;
            JoinButton.interactable = false;
        }

        private void EnableEverything()
        {
            JoinField.interactable = true;
            HostButton.interactable = true;
            JoinButton.interactable = true;
        }

        private bool connected = false;

        public void JoinGame()
        {
            string ipToConnectTo = JoinField.text.Substring(0, JoinField.text.Length);
            int ipLength = ipToConnectTo.Length;
            Debug.Log("Connecting to " + ipToConnectTo);
            DisableEverything();
            try
            {
                Unity.Netcode.NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = ipToConnectTo;
                StartCoroutine(ConnectTimeout());
                if (Unity.Netcode.NetworkManager.Singleton.StartClient() == false)
                {
                    EnableEverything();
                    TimeoutText.gameObject.SetActive(true);
                    Debug.LogError("Failed to connect to host");
                    StartCoroutine(Countdown5Seconds());
                }
                else Debug.Log("Client started!");
            }
            catch(Exception e)
            {
                TimeoutText.gameObject.SetActive(true);
                StartCoroutine(Countdown5Seconds());
            }
        }

        public void SwitchGame()
        {
            SceneManager.LoadScene(0);
        }

        private IEnumerator ConnectTimeout()
        {
            float duration = 5f;
            while(duration > 0f)
            {
                duration -= Time.deltaTime;
                yield return null;
            }    
            if(SceneManager.GetActiveScene().name.ToLower() == "crashtitlescreen" && NetworkManager.Singleton != null)
            {
                EnableEverything();
                TimeoutText.gameObject.SetActive(true);
                StartCoroutine(Countdown5Seconds());
                NetworkManager.Singleton.Shutdown();
            }
        }

        private IEnumerator Countdown5Seconds()
        {
            float duration = 4f;
                                 //to whatever you want
            float time = 0;
            while (time < duration)
            {
                time += Time.deltaTime;
                yield return null;
            }
            TimeoutText.gameObject.SetActive(false);
        }

    }
}
