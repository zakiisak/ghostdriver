using Assets2.Code;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets2.Code
{

    public class Game2
    {
        public static int TitlescreenBuildIndex = 4;
        public static void OnClientScore(bool win)
        {
            ShowWinLossText(win);
        }

        public static void OnServerScore(bool win)
        {
            ShowWinLossText(win);
            ShowServerControls();
        }

        private static void ShowWinLossText(bool win)
        {
            ShowLightAgain();
            Assets.Code.SoundManager.StopPlayingEngine();
            Assets.Code.SoundManager.StopPlayingTire();
            Assets.Code.SoundManager.StopPlayingHorns();
            Assets.Code.SoundManager.StopPlayingBuild();
            if (win)
            {
                GameObject.Find("Canvas").transform.Find("Win").GetComponent<TMP_Text>().enabled = true;
            }
            else
            {
                GameObject.Find("Canvas").transform.Find("Lose").GetComponent<TMP_Text>().enabled = true;
            }
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
            foreach(CarRoadController car in cars)
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

        private static void ShowServerControls()
        {
            GameObject.Find("Canvas").transform.Find("ServerControls").gameObject.SetActive(true);
        }
    }
}

