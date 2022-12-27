using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentDieWhenChildrenGone : MonoBehaviour
    {
        public void Update()
        {
            if (transform.childCount <= 0)
                Destroy(gameObject);
        }
    }
}
