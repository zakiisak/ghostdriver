using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets2.Code.Net
{
    public class ComponentServerControls : MonoBehaviour
    {

        public void OnRestart()
        {
            Unity.Netcode.NetworkManager.Singleton.SceneManager.LoadScene("LoadingScreen", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        public void OnQuit()
        {
            Unity.Netcode.NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene(Game2.TitlescreenBuildIndex);
        }
    }
}
