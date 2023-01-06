using UnityEngine;

namespace Assets.Code.Game
{
    public class Settings
    {
        public static bool ShadowsEnabled = true;
        public static bool TreesEnabled { get { return PlayerPrefs.GetInt("TreesDisabled", 0) == 0; } }


        public static bool CrazyMode = false;

    }
}
