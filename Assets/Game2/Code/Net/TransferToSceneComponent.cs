using Unity.Netcode;
using UnityEngine;

namespace Assets2.Code.Net
{
    public class TransferToSceneComponent : MonoBehaviour
    {
        public float delay = 0.5f;
        public string SceneName;

        public void Awake()
        {
            
        }

        private bool hasBegunNavigating = false;

        public void Update()
        {
            if (delay > 0f)
                delay -= Time.deltaTime;
            else if(hasBegunNavigating == false && NetworkManager.Singleton.IsServer)
            {
                NetworkManager.Singleton.SceneManager.LoadScene(SceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
                hasBegunNavigating = true;
            }
        }
    }
}
