using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class SoundManager : MonoBehaviour
    {
        private static AudioSource sourceBuild;
        private static AudioSource sourceCrash;
        private static AudioSource sourceEngine;
        private static AudioSource sourceTire;
        private static AudioSource sourceEngineStart;
        private static AudioSource sourceHorn;

        public static bool IsInitialized { get { return sourceCrash != null && PrefabManager.Instance != null; } }

        public void Awake()
        {
            sourceBuild = GameObject.Find("AudioBuild").GetComponent<AudioSource>();
            sourceCrash = GameObject.Find("AudioCrash").GetComponent<AudioSource>();
            sourceEngine = GameObject.Find("AudioEngine").GetComponent<AudioSource>();
            sourceEngineStart = GameObject.Find("AudioEngineStart").GetComponent<AudioSource>();
            sourceHorn = GameObject.Find("AudioHorn").GetComponent<AudioSource>();
            sourceTire = GameObject.Find("AudioTire").GetComponent<AudioSource>();
        }


        public static void PlayBuild()
        {
            if (IsInitialized == false)
                return;
            sourceBuild.clip = SelectRandom(PrefabManager.Instance.Sound_Build);
            sourceBuild.Play();
        }

        public static void StopPlayingBuild()
        {
            if (IsInitialized == false)
                return;
            sourceBuild.Stop();
        }

        public static void PlayCrash()
        {
            if (IsInitialized == false)
                return;
            sourceCrash.clip = SelectRandom(PrefabManager.Instance.Sound_Crash);
            sourceCrash.Play();
        }

        public static void PlayTire(float delay = 0f)
        {
            if (IsInitialized == false)
                return;
            sourceTire.clip = SelectRandom(PrefabManager.Instance.Sound_Tire);
            sourceTire.PlayDelayed(delay);
        }

        public static void PlayEngineStart(float delay = 0f)
        {
            if (IsInitialized == false)
                return;
            sourceEngineStart.clip = SelectRandom(PrefabManager.Instance.Sound_Start);
            sourceEngineStart.PlayDelayed(delay);
        }

        public static void PlayHorn(float delay = 0f)
        {
            if (IsInitialized == false)
                return;
            sourceHorn.clip = SelectRandom(PrefabManager.Instance.Sound_Horn);
            sourceHorn.PlayDelayed(delay);
        }

        public static void BeginPlayingEngine()
        {
            if (IsInitialized == false)
                return;
            sourceEngine.clip = SelectRandom(PrefabManager.Instance.Sound_Horn);
            sourceEngine.loop = true;
            sourceEngine.Play();
            sourceEngine.pitch = 0.5f;
            sourceEngine.volume = 0.5f;
        }

        public static void StopPlayingEngine()
        {
            if (IsInitialized == false)
                return;
            sourceEngine.Stop();
        }

        public static void StopPlayingHorns()
        {
            if (IsInitialized == false)
                return;
            sourceHorn.Stop();
        }

        public static void StopPlayingTire()
        {
            if (IsInitialized == false)
                return;
            sourceTire.Stop();
        }

        public static void UpdateEnginePitch(float volumePercentage, float pitchPercentage)
        {
            if (IsInitialized == false)
                return;
            sourceEngine.volume = 0.5f + 0.5f * volumePercentage;
            sourceEngine.pitch = 0.5f + 0.5f * pitchPercentage;
        }


        private static AudioClip SelectRandom(List<AudioClip> clips)
        {
            return clips[Random.Range(0, clips.Count)];
        }

    }
}
