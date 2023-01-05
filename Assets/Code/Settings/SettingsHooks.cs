using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.Settings
{
    public class SettingsHooks : MonoBehaviour
    {

        public void OnBackPressed()
        {
            SceneManager.LoadScene(0);
        }

        public void OnLowSelected()
        {
            QualitySettings.SetQualityLevel(0);
        }

        public void OnMediumSelected()
        {
            QualitySettings.SetQualityLevel(1);
        }

        public void OnHighSelected()
        {
            QualitySettings.SetQualityLevel(2);
        }
    }
}
