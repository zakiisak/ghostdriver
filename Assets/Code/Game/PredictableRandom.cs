using System;

namespace Assets.Code.Game
{
    public static class PredictableRandom
    {
        private static int seed;
        private static Random random;

        private static Random seedRng = new Random();

        public static void SetSeed(int seed)
        {
            PredictableRandom.seed = seed;
            random = new Random(seed);
            UnityEngine.Debug.Log("Set seed to " + seed);
        }

        public static void SetRandomSeed()
        {
            SetSeed(seedRng.Next());
        }

        public static int GetSeed()
        {
            return seed;
        }

        public static int Range(int minInclusive, int maxExclusive)
        {
            int result = 0;

            if (maxExclusive == minInclusive || maxExclusive < minInclusive)
                result = minInclusive;
            else 
                result = random.Next(maxExclusive - minInclusive) + minInclusive;

            UnityEngine.Debug.Log("int Range: " + result);
            return result;
        }
        public static float Range(float minInclusive, float maxInclusive)
        {
            float diff = maxInclusive - minInclusive;
            float result = minInclusive + (float)(random.NextDouble() * diff);
            UnityEngine.Debug.Log("float Range: " + result);
            return result;
        }

        public static Random Instance()
        {
            return random;
        }


    }
}
