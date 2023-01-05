using NatML.Recorders;
using NatML.Recorders.Clocks;
using NatML.Recorders.Inputs;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Game
{
    public class ComponentScreenRecorder : MonoBehaviour
    {
        public static ComponentScreenRecorder Instance { get; private set; }

        private RealtimeClock clock;
        private MP4Recorder recorder;
        private CameraInput cameraInput;

        public string VideoPath { get; private set; }

        public bool Active { get; private set; }
        public bool Sharable { get { return VideoPath != null; } }

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

            return path;
        }

        public void OnDestroy()
        {
            if (Active)
                Finish();
        }

    }
}
