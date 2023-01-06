using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.Game
{
    public class ComponentEndControls : MonoBehaviour
    {
        public void Replay()
        {
            ComponentScreenRecorder.Instance.Stop();
            PredictableRandom.SetRandomSeed();
            SceneManager.LoadScene(1);
        }

        public void OnExit()
        {
            ComponentScreenRecorder.Instance.Stop();
            SceneManager.LoadScene(0);
        }

        public void ShareReplay()
        {
            ComponentScreenRecorder.Instance.Share();

            /*
            if(SharePayload.Supported)
            {
                SharePayload share = new SharePayload();
                share.AddText("Ghost Driver Replay - Score " + ComponentScoreController.Score);
                share.AddMedia(path);
                string result = await share.Share();
                Debug.Log("Share payload share result: " + result);
            }
            else
            {
                Debug.Log("Sharing not supported on this platform. The media path: is " + path);
            }
            */

        }


    }
}
