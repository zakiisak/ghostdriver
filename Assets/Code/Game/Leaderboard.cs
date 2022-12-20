using GlobalstatsIO;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Game
{
    public class Leaderboard : MonoBehaviour
    {
        private const string ClientID = "LSkBnPUrHdAK1ZDJYKldtbQI8ek70XoLPJmdyhFg";
        private const string ClientSecret = "f1weFfdpBqTMhBVvV2vSrhIaDWJVB0pQ2W5Hci6N";

        private static Leaderboard Instance;
        private static GlobalstatsIOClient client;

        public void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            Load();
        }


        public static void Load()
        {
            client = new GlobalstatsIOClient(ClientID, ClientSecret);
        }

        public static void ShareScore(int score, Action<bool> callback)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("score", score.ToString());

            string name = SystemInfo.deviceName;
            Instance.StartCoroutine(client.Share(values, "", name, callback));
        }

        public static void GetScores(Action<GlobalstatsIO.Leaderboard> callback)
        {
            Instance.StartCoroutine(client.GetLeaderboard("score", 10, callback));
        }

        public static void ShowLeaderboard()
        {
            //Instance.GetComponent<Image>().color = new Color(0, 0, 0, 0.6f);
            GetScores((leaderboard) =>
            {
                DisplayTitle();
                for (int i = 0; i < leaderboard.data.Length; i++)
                {
                    LeaderboardValue val = leaderboard.data[i];
                    SetTextOnScore(i, val.name, val.value);
                }
            });
        }

        private static void DisplayTitle()
        {
            for (int i = 0; i < Instance.transform.childCount; i++)
            {
                Transform child = Instance.transform.GetChild(i);
                if (child.name == "Title")
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        private static void SetTextOnScore(int scoreIndex, string name, string score)
        {
            string textScoreName = (scoreIndex + 1).ToString();

            for (int i = 0; i < Instance.transform.childCount; i++)
            {
                Transform child = Instance.transform.GetChild(i);
                if (child.name == textScoreName)
                {
                    TMP_Text text = child.GetComponent<TMP_Text>();
                    text.text = "#" + textScoreName + " - " + name + ": " + score;
                    text.enabled = true;
                    break;
                }
            }
        }
    }
}
