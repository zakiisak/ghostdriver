namespace Assets.Code.Game
{
    public struct ReplayMove
    {
        public float z;
        public byte index;

        public ReplayMove(int index, float z)
        {
            this.z = z;
            this.index = (byte)index;
        }
    }
}
