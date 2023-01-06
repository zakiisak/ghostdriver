using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Code.Game
{
    public class Game
    {
        public static async void OnScore()
        {
            int score = ComponentScoreController.Score;

            if(score > PlayerPrefs.GetInt("MaxScore", 0))
            {
                PlayerPrefs.SetInt("MaxScore", score);
                Leaderboard.ShareScore(score, (b) => { });
            }

            ShowStatus();

            ComponentScreenRecorder.Instance.StartCoroutine(StopRecordingAfter3Seconds());
        }

        public static void DeleteAllSavedReplays()
        {
            if(PlayerPrefs.HasKey("Replays"))
            {
                string replaysString = PlayerPrefs.GetString("Replays");
                Replays replays = JsonUtility.FromJson<Replays>(replaysString);
                foreach(string replayPath in replays.VideoPaths)
                {
                    try
                    {
                        File.Delete(replayPath);
                    }
                    catch(Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                replays.Clear();
                PlayerPrefs.SetString("Replays", JsonUtility.ToJson(replays));
                PlayerPrefs.Save();
            }
        }

        private static IEnumerator StopRecordingAfter3Seconds()
        {
            yield return new WaitForSeconds(2);
            StopRecording();
        }

        private static async void StopRecording()
        {
            string path = await ComponentScreenRecorder.Instance.Finish();
            Switch3DScoreDisplayTo2D();
            if (string.IsNullOrEmpty(path) == false)
                GameObject.Find("Canvas").transform.Find("GameControls").transform.Find("ShareButton").gameObject.SetActive(true);
        }

        private static void Switch3DScoreDisplayTo2D()
        {
            Component3DScoreLocationController.Instance.gameObject.SetActive(false);

            ComponentScoreController controller = GameObject.Find("Canvas").transform.Find("Score").gameObject.GetComponent<ComponentScoreController>();
            controller.gameObject.SetActive(true);
            controller.text.text = ComponentScoreController.Score.ToString();
        }

        private static void ShowStatus()
        {
            Leaderboard.ShowLeaderboard();
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

            PlayerController[] cars = GameObject.FindObjectsOfType<PlayerController>();
            foreach (PlayerController car in cars)
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

            PlayerController[] cars = GameObject.FindObjectsOfType<PlayerController>();
            foreach (PlayerController car in cars)
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

