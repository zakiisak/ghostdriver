using System;
using System.Collections.Generic;

namespace Assets.Code.Game
{
    [Serializable]
    public class Replays
    {
        public List<string> VideoPaths;

        public void Add(string path)
        {
            VideoPaths.Add(path);
        }

        public void Clear()
        {
            VideoPaths.Clear();
        }
    }
}
