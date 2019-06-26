﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class PhotoController : MonoBehaviour
{
    public Texture2D tex;
    
    private void OnGUI()
    {
        if(GUI.Button(new Rect(0,0,100,100),"Take photo"))
        {
            StartCoroutine(TakePhoto((img) =>
            {
                tex = img;
            }));
        }

        if (GUI.Button(new Rect(0, 200, 100, 100), "save photo"))
        {
            SavePhoto(tex);
        }

        if (GUI.Button(new Rect(0, 300, 100, 100), "share photo"))
        {
            SharePhoto(tex);
        }
    }

    private void SavePhoto(Texture2D img)
    {
        if (img != null)
        {
            NativeGallery.SaveImageToGallery(img, "GalleryTest", "My img {0}.png");
        }
    }

    private void SharePhoto(Texture2D img)
    {
        if (img != null)
        {
            string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
            File.WriteAllBytes(filePath, img.EncodeToPNG());
           // Destroy(img);

            new NativeShare().AddFile(filePath).SetSubject("Goorin Bros").SetText("O SI!").Share();
        }
    }

    private IEnumerator TakePhoto(UnityAction<Texture2D> img)
    {
        Texture2D s = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        yield return new WaitForEndOfFrame();
        s.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        s.Apply();
        img(s);
    }


}