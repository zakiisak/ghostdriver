using TMPro;
using UnityEngine;

namespace Assets2.Code.Net
{
    public class ComponentIpDisplay : MonoBehaviour
    {
        public TMP_Text text;
        public string Prefix;

        public void Start()
        {
            text.text = Prefix + NetworkUtils.GetLocalIPv4();
        }

    }
}
