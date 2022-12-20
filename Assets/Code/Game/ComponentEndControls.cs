using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.Game
{
    public class ComponentEndControls : MonoBehaviour
    {
        public void Replay()
        {
            SceneManager.LoadScene(1);
        }

    }
}
