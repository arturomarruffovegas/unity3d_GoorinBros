using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace goorinAR
{

    public class Utils 
    {
        public static Sprite CutTexture(Texture2D tex)
        {
            Sprite text = Sprite.Create(tex, new Rect(tex.width / 2, tex.height / 2, tex.width * 0.1f, tex.height * 0.1f), Vector2.zero);
            return text;
        }

        public static IEnumerator OnDownloadImage(string myURL, UnityAction<Sprite> image)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(myURL);
            yield return www.SendWebRequest();

            Texture2D tex = DownloadHandlerTexture.GetContent(www);
            Sprite spri = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            image?.Invoke(spri);
        }

        public static IEnumerator TakePhoto(UnityAction<Texture2D> img)
        {
            Texture2D s = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            yield return new WaitForEndOfFrame();
            s.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            s.Apply();
            img(s);
        }


    }



    [System.Serializable]
    public class DataProducts
    {
        public string Name;
        public string price;
        public Sprite Icon;
        public List<ColorsAndSizes> colorsAndSizes = new List<ColorsAndSizes>();
    }

    [System.Serializable]
    public class ColorsAndSizes
    {
        public string NameColor;
        public string URLImage;
        public Sprite HatImage;
        public List<string> sizes = new List<string>();
    }
}
