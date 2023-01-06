using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentRotating : MonoBehaviour
    {

        public Vector3 Rotation;
        public void Update()
        {
            transform.Rotate(Rotation * Time.deltaTime);
        }
    }
}
