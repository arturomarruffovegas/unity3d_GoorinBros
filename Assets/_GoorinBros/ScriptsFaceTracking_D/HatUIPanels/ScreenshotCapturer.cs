using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScreenshotCapturer {

    private static List<Camera> cameras = new List<Camera>();

    public static void SetCameras(params Camera[] camera) {
        cameras.Clear();
        for (int i = 0; i < camera.Length; i++) {
            cameras.Add(camera[i]);
        }
    }

    public static Texture2D Capture() {
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32) {
            useMipMap = false,
            antiAliasing = 4
        };

        foreach (var item in cameras) {
            var currentTT = item.targetTexture;
            item.targetTexture = rt;
            item.Render();
            item.targetTexture = currentTT;
        }

        var activeRT = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply(false, false);
        RenderTexture.active = activeRT;

        return texture;
    }

}