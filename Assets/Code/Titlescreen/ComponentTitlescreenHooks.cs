using Assets.Code.Game;
using Assets2.Code;
using TMPro;
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
            if(PlayerPrefs.HasKey("Quality"))
            {
                int qualityLevel = PlayerPrefs.GetInt("Quality");
                QualitySettings.SetQualityLevel(qualityLevel);
            }
            Game.Game.DeleteAllSavedReplays();
        }

        public void OnStartPressed()
        {
            PredictableRandom.SetRandomSeed();
            SceneManager.LoadScene(1);
        }

        public void OnSettingsPressed()
        {
            SceneManager.LoadScene(5);
        }


        public void SwitchGame()
        {
            SceneManager.LoadScene(Game2.TitlescreenBuildIndex);
        }
    }
}
