using UnityEngine;

namespace Assets.Code.Game
{
    public class Settings
    {
        public static bool ShadowsEnabled = true;
        public static bool TreesEnabled { get { return PlayerPrefs.GetInt("TreesEnabled", 0) == 1; } }


        public static bool CrazyMode = false;

    }
}
