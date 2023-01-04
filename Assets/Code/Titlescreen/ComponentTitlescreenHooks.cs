using Assets.Code.Game;
using Assets2.Code;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.Titlescreen
{
    public class ComponentTitlescreenHooks : MonoBehaviour
    {

        public TMP_InputField ReplayCodeInputField;
        public TMP_Text ReplayCodeInvalidText;

        public void Start()
        {

        }

        public void OnStartPressed()
        {
            Game.Game.ShouldPlayerBeInReplayMode = false;
            PredictableRandom.SetRandomSeed();
            SceneManager.LoadScene(1);
        }

        public void OnViewRelay()
        {
            Game.Game.ReplayCode = ReplayCodeInputField.text;
            Game.Game.ShouldPlayerBeInReplayMode = true;

            if(Game.Game.ParseReplayMove(Game.Game.ReplayCode))
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                ReplayCodeInvalidText.enabled = true;
                StartCoroutine(InvalidCodeDelayRemove());
            }

        }

        private IEnumerator InvalidCodeDelayRemove()
        {
            float duration = 3f;
            while (duration > 0f)
            {
                duration -= Time.deltaTime;
                yield return null;
            }
            if (SceneManager.GetActiveScene().name.ToLower() == "titlescreen")
            {
                ReplayCodeInvalidText.enabled = false;
            }
        }

        public void SwitchGame()
        {
            SceneManager.LoadScene(Game2.TitlescreenBuildIndex);
        }
    }
}
