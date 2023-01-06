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
            PlayerPrefs.SetInt("Quality", 0);
            PlayerPrefs.Save();
        }

        public void OnMediumSelected()
        {
            PlayerPrefs.SetInt("Quality", 1);
            PlayerPrefs.Save();
        }

        public void OnHighSelected()
        {
            PlayerPrefs.SetInt("Quality", 2);
            PlayerPrefs.Save();
        }
    }
}
