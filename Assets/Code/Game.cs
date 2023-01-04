using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace Assets.Code.Game
{
    public class Game
    {
        //A global flag that gets set when we press the buttons - Start or watch replay
        public static bool ShouldPlayerBeInReplayMode;
        public static string ReplayCode;
        public static List<ReplayMove> ReplayMoves = new List<ReplayMove>();

        public static bool ParseReplayMove(string replayCode)
        {
            try
            {
                ReplayMoves.Clear();
                string[] parts = replayCode.Split("_");
                if (parts.Length > 3)
                {
                    int seed = int.Parse(parts[0], CultureInfo.InvariantCulture);
                    int maxTreeAmount = int.Parse(parts[1], CultureInfo.InvariantCulture);
                    float maxTreeViewWidth = float.Parse(parts[2], CultureInfo.InvariantCulture);

                    for (int i = 3; i < parts.Length; i++)
                    {
                        string[] replayParts = parts[i].Split(';');
                        float z = float.Parse(replayParts[0], CultureInfo.InvariantCulture);
                        int index = int.Parse(replayParts[1], CultureInfo.InvariantCulture);
                        ReplayMove move = new ReplayMove(index, z);
                        ReplayMoves.Add(move);
                    }

                    ComponentTreeSpawner.maxTreeAmount = maxTreeAmount;
                    PredictableRandom.SetSeed(seed);

                    return true;
                }
                else return false;
            }
            catch(Exception e)
            {
                Debug.LogError(e.Message);
                return false;
            }
        }

        public static string GenerateReplayCode(List<ReplayMove> replayMoves)
        {
            string code = "";
            code += PredictableRandom.GetSeed() + "_";
            code += ComponentTreeSpawner.maxTreeAmount.ToString(CultureInfo.InvariantCulture) + "_";
            code += ComponentTreeSpawner.xViewWidth.ToString(CultureInfo.InvariantCulture) + "_";
            for (int i = 0; i < replayMoves.Count; i++)
            {
                ReplayMove move = replayMoves[i];
                code += move.z.ToString(CultureInfo.InvariantCulture) + ";" + move.index.ToString(CultureInfo.InvariantCulture);
                if (i < replayMoves.Count - 1)
                    code += "_";
            }
            return code;
        }

        public static void OnScore()
        {
            if(ShouldPlayerBeInReplayMode == false)
            {
                int score = ComponentScoreController.Score;

                if(score > PlayerPrefs.GetInt("MaxScore", 0))
                {
                    PlayerPrefs.SetInt("MaxScore", score);
                    Leaderboard.ShareScore(score, (b) => { });
                }
            }

            ShowStatus();
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

