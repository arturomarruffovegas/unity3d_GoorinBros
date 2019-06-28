using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace FFmpeg.Demo.REC
{
    public class RECView : MonoBehaviour
    {
        public FFmpegREC recLogic;
        public Button startBtn, stopBtn;
        public Text output;
        const int charsLimit = 2000;

        public VideoPlayer video;

        //------------------------------

        void Awake()
        {
            recLogic.Init(Output, Finish);
            startBtn.onClick.AddListener(OnStart);
            stopBtn.onClick.AddListener(OnStop);
        }

        //------------------------------

        public void OnStart()
        {
            startBtn.interactable = false;
            stopBtn.interactable = true;
            recLogic.StartREC();
            Debug.Log("ssss");
        }

        //------------------------------

        public void OnStop()
        {
            stopBtn.interactable = false;
            recLogic.StopREC();
        }

        //------------------------------

        void Output(string msg)
        {
            string newOutput = output.text + msg;
            if (newOutput.Length > charsLimit)
                newOutput = newOutput.Remove(0, newOutput.Length - charsLimit);
            output.text = newOutput;
        }

        public void Finish(string outputVideo)
        {
            startBtn.interactable = true;
            Debug.Log("Video saved to: " + outputVideo);
            string localURL = "file://" + outputVideo;

            //video.source = VideoSource.Url;
            //video.url = localURL;

            //video.Play();


#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
            Handheld.PlayFullScreenMovie(
                localURL,
                Color.black,
                FullScreenMovieControlMode.Full,
                FullScreenMovieScalingMode.AspectFit);
#else
            //  Application.OpenURL(localURL);
#endif
        }

        //------------------------------

        void OnDestroy()
        {
            startBtn.onClick.RemoveListener(OnStart);
            stopBtn.onClick.RemoveListener(OnStop);
        }
    }
}