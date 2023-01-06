using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Assets.Code.Game
{
    public class ComponentLowSettingChecker : MonoBehaviour
    {

        public void Awake()
        {
            if (QualitySettings.GetQualityLevel() == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
