using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.Game
{
    public class ComponentEndControls : MonoBehaviour
    {
        public void Replay()
        {
            if(Game.ShouldPlayerBeInReplayMode)
                PredictableRandom.SetSeed(PredictableRandom.GetSeed());
            SceneManager.LoadScene(1);
        }

        public void OnExit()
        {
            SceneManager.LoadScene(0);
        }

        public void CopyReplayCode()
        {
            Debug.Log("Replay Code: " + Game.ReplayCode);
            GUIUtility.systemCopyBuffer = Game.ReplayCode;
        }

    }
}
