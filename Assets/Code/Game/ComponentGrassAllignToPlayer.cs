using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentGrassAllignToPlayer : MonoBehaviour
    {

        public void Update()
        {
            if(PlayerController.LocalInstance != null)
                transform.position = new Vector3(transform.position.x, transform.position.y, PlayerController.LocalInstance.transform.position.z);
        }

    }
}
