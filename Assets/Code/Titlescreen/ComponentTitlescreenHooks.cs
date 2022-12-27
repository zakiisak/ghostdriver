using Assets2.Code;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.Titlescreen
{
    public class ComponentTitlescreenHooks : MonoBehaviour
    {

        public void Start()
        {

        }

        public void OnStartPressed()
        {
            SceneManager.LoadScene(1);
        }

        public void SwitchGame()
        {
            SceneManager.LoadScene(Game2.TitlescreenBuildIndex);
        }
    }
}
