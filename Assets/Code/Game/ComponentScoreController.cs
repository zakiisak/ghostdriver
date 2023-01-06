using System;
using TMPro;
using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentScoreController : MonoBehaviour
    {
        public static int Score;

        public TMP_Text text;

        private int lastProgress = 0;

        public void Update()
        {
            if(PlayerController.LocalInstance != null)
            {
                int progress = (int) Math.Max(PlayerController.LocalInstance.transform.position.z / 50.0f, 1);
                Score = progress;

                if(progress != lastProgress)
                {
                    text.text = progress.ToString();
                }
                lastProgress = progress;
            }
        }
    }
}
