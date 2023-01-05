using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Settings
{
    public class ComponentTreesEnabled : MonoBehaviour
    {
        public Toggle toggle;

        public void Start()
        {
            toggle = GetComponent<Toggle>();

            toggle.isOn = PlayerPrefs.GetInt("TreesEnabled", 0) == 1;
        }

        public void OnToggled()
        {
            int val = toggle.isOn ? 1 : 0;
            PlayerPrefs.SetInt("TreesEnabled", val);
            PlayerPrefs.Save();
            Debug.Log("Is on: " + val);
        }
    }
}
