using UnityEngine;

namespace Assets.Code.HUD
{
    public class ComponentMakeHeightAccordingToScreenSize : MonoBehaviour
    {

        public float heightPercentage = 1.0f;

        public void Start()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            RectTransform canvasTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(canvasTransform.sizeDelta.x * 0.9f, canvasTransform.sizeDelta.y * heightPercentage); 
        }
    }
}
