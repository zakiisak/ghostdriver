using NatML.Recorders;
using NatML.Recorders.Clocks;
using NatML.Recorders.Inputs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentScreenRecorder : MonoBehaviour
    {
        public struct VideoPathEntry
        {
            public float time;
            public string path;

            public VideoPathEntry(float time, string path)
            {
                this.time = time;
                this.path = path;
            }
        }

        public static ComponentScreenRecorder Instance { get; private set; }

        private RealtimeClock clock;
        private MP4Recorder recorder;
        private CameraInput cameraInput;

        public string VideoPath { get; private set; }

        public bool Active { get; private set; }
        public bool Sharable { get { return VideoPath != null; } }

        public bool HasBeenShared { get; private set; }

        private bool deleteUponFinish = false;

        public void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            Camera camera = Camera.main;

            clock = new RealtimeClock();

            float height = 480f;

            float widthAspectRatio = (float) Screen.width / (float) Screen.height;
            int width = (int)(widthAspectRatio * height);
            if (width % 2 != 0)
                width++;

            recorder = new MP4Recorder(width, (int) height, 30);
            cameraInput = new CameraInput(recorder, clock, camera);
            Active = true;
        }

        public async Task<string> Finish()
        {
            Active = false;
            cameraInput.Dispose();

            string path = await recorder.FinishWriting();

            VideoPath = path;

            if (deleteUponFinish)
            {
                DeleteVideo(path);
            }

            return path;
        }

        private void SaveVideoPath(string path)
        {
            Replays replays = new Replays();
            if(PlayerPrefs.HasKey("Replays"))
                replays = JsonUtility.FromJson<Replays>(PlayerPrefs.GetString("Replays"));

            replays.Add(path);
            PlayerPrefs.SetString("Replays", JsonUtility.ToJson(replays));
            PlayerPrefs.Save();
        }

        private bool DeleteVideo(string path)
        {
            try
            {
                File.Delete(path);
                Debug.Log("Deleted video " + path);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            return false;
        }

        public void Share()
        {
            HasBeenShared = true;

            new NativeShare().AddFile(VideoPath)
                .SetSubject("Ghost Driver Replay - Score " + ComponentScoreController.Score)
                .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
                .Share();

            SaveVideoPath(VideoPath);
        }

        private float lastTimeRunDeleteCheck;

        /*
        public void FixedUpdate()
        {
            if(Time.time - lastTimeRunDeleteCheck > 0.5f)
            {
                for(int i = 0; i < VideoPaths.Count; i++)
                {
                    VideoPathEntry entry = VideoPaths[i];

                    if(Time.time - entry.time > 5.0f && entry.path != VideoPath) //Don't delete the current video recorded, when idling in the leaderboard screen.
                    {
                        if(DeleteVideo(entry.path))
                            VideoPaths.RemoveAt(i--);
                    }
                }
            }
        }
        */

        public void Stop()
        {
            if (HasBeenShared == false)
            {
                if(Active)
                {
                    deleteUponFinish = true;
                    Finish();
                }
                else
                {
                    DeleteVideo(VideoPath);
                }
            }
        }

        public void OnDestroy()
        {

        }

    }
}
