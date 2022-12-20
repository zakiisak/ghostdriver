using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.Titlescreen
{
    public class ComponentStartGame : MonoBehaviour
    {

        public void Start()
        {

        }

        public void OnStartPressed()
        {
            SceneManager.LoadScene(1);
        }
    }
}
