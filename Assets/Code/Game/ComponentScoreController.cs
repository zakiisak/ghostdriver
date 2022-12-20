using System;
using TMPro;
using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentScoreController : MonoBehaviour
    {
        public TMP_Text text;

        private int lastProgress = 0;

        public void Update()
        {
            if(CarRoadController.LocalInstance != null)
            {
                int progress = (int) Math.Max(CarRoadController.LocalInstance.transform.position.z / 50.0f, 1);

                if(progress != lastProgress)
                {
                    text.text = progress.ToString();
                }
                lastProgress = progress;
            }
        }
    }
}
