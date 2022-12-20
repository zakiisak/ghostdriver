using TMPro;
using UnityEngine;

namespace Assets.Code.Titlescreen
{
    public class ComponentDisplayMaxScore : MonoBehaviour
    {
        public TMP_Text text;

        public void Awake()
        {
            //TODO get max score from preferences
            if (PlayerPrefs.HasKey("MaxScore"))
            {
                text.text = "Your Max: " + PlayerPrefs.GetInt("MaxScore");
            }
            else text.text = "";

        }

    }
}
