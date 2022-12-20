using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentGrassAllignToPlayer : MonoBehaviour
    {

        public void Update()
        {
            if(CarRoadController.LocalInstance != null)
                transform.position = new Vector3(transform.position.x, transform.position.y, CarRoadController.LocalInstance.transform.position.z);
        }

    }
}
