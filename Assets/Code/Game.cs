using TMPro;
using UnityEngine;

namespace Assets.Code.Game
{
    public class Game
    {
        public static void OnScore(bool win)
        {
            ShowStatus(win);
        }

        private static void ShowStatus(bool win)
        {
            ShowLightAgain();
            SoundManager.StopPlayingEngine();
            SoundManager.StopPlayingTire();
            SoundManager.StopPlayingHorns();
            SoundManager.StopPlayingBuild();
            GameObject.Find("Canvas").transform.Find("GameControls").gameObject.SetActive(true);
        }

        public static bool IsDark()
        {
            return RenderSettings.fogEndDistance < 70.0f;
        }

        public static void GoPitchBlack()
        {
            RenderSettings.fogEndDistance = 50f;

            CarRoadController[] cars = GameObject.FindObjectsOfType<CarRoadController>();
            foreach (CarRoadController car in cars)
            {
                car.TurnOffLights();
            }

            /*
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                Transform black = canvas.transform.Find("BlackBackground");
                if (black != null)
                {
                    black.gameObject.SetActive(true);
                }
            }
            */
        }

        public static void ShowLightAgain()
        {
            RenderSettings.fogEndDistance = 220.0f;

            CarRoadController[] cars = GameObject.FindObjectsOfType<CarRoadController>();
            foreach (CarRoadController car in cars)
            {
                car.TurnOnLights();
            }

            /*
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                Transform black = canvas.transform.Find("BlackBackground");
                if (black != null)
                {
                    black.gameObject.SetActive(false);
                }
            }
            */
        }
    }

}

