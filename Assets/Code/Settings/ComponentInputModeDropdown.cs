using TMPro;
using UnityEngine;

namespace Assets.Code.Settings
{
    public class ComponentInputModeDropdown : MonoBehaviour
    {

        public void Start()
        {
            int selectedIndex = PlayerPrefs.GetInt("InputMode", 0);
            TMP_Dropdown dropdown = GetComponent<TMP_Dropdown>();
            dropdown.onValueChanged.AddListener((val) =>
            {
                Debug.Log("Selected input mode : " + val);
                PlayerPrefs.SetInt("InputMode", val);
            });

            dropdown.value = selectedIndex;
        }
    }
}
