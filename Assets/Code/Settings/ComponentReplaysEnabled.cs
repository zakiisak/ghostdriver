using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Settings
{
    public class ComponentReplaysEnabled : MonoBehaviour
    {
        public Toggle toggle;

        public void Start()
        {
            toggle = GetComponent<Toggle>();

            toggle.isOn = PlayerPrefs.GetInt("ReplaysEnabled", 1) == 1;
        }

        public void OnToggled()
        {
            int val = toggle.isOn ? 1: 0;
            PlayerPrefs.SetInt("ReplaysEnabled", val);
            PlayerPrefs.Save();
        }
    }
}
