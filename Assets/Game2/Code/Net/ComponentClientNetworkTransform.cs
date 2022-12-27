using Unity.Netcode.Components;

namespace Assets2.Code.Net
{
    public class ComponentClientNetworkTransform : NetworkTransform
    {

        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
