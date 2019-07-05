using FFmpeg.Demo.REC;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public FFmpegREC recLogic;
    public Button startBtn, stopBtn;
    public Text output;
    const int charsLimit = 2000;

    public VideoPlayer videoPlayer;

    string videoPath;

    //------------------------------

    void Awake()
    {
        recLogic.Init(Output, Finish);
     //   startBtn.onClick.AddListener(OnStart);
       // stopBtn.onClick.AddListener(OnStop);
    }

    //------------------------------

    public void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 100, 100), "Start"))
        {
            OnStart();
        }

        if (GUI.Button(new Rect(Screen.width / 2, (Screen.height / 2) + 100, 100, 100), "Stop"))
        {
            OnStop();
        }

    }

    public void OnStart()
    {
        //startBtn.interactable = false;
       // stopBtn.interactable = true;
        recLogic.StartREC();
        Debug.Log("start");
    }

    //------------------------------

    public void OnStop()
    {
       // stopBtn.interactable = false;
        recLogic.StopREC();
    }

    //------------------------------

    void Output(string msg)
    {
        //string newOutput = output.text + msg;
        //if (newOutput.Length > charsLimit)
        //    newOutput = newOutput.Remove(0, newOutput.Length - charsLimit);
        //output.text = newOutput;
    }
    
    public void Finish(string outputVideo)
    {
       // startBtn.interactable = true;
        Debug.Log("Video saved to: " + outputVideo);
        string localURL = "";

        localURL = "file://" + outputVideo;
        Debug.Log("all path: " + localURL);
        videoPath = localURL;

        //StartCoroutine(LoadInitVideo(videoPlayer, videoPath, null));

       StartCoroutine(DownloadVideo());

    }

    private IEnumerator DownloadVideo()
    {
        yield return new WaitForEndOfFrame();
        WWW www = new WWW(videoPath);
        yield return www;
        Debug.Log("saving");

        //guardar
        NativeGallery.SaveVideoToGallery(www.bytes, "Goorin Bros", "test.mp4");

        //compartir
        //string filePath = Path.Combine(Application.temporaryCachePath, "GG.mp4");
        //File.WriteAllBytes(filePath, www.bytes);
        //new NativeShare().AddFile(filePath).Share();
        
        Debug.Log("Saved");
    }


    public IEnumerator LoadInitVideo(VideoPlayer video, string path, MeshRenderer ob = null)
    {
        video.source = VideoSource.Url;
        video.url = path;

        video.Prepare();

        while (!video.isPrepared)
            yield return null;

        video.Play();

        while (video.isPlaying)
        {
            if (ob != null)
                ob.enabled = true;

            yield return null;
        }
    }
}
