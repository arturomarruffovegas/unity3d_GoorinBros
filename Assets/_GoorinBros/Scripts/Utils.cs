using Shopify.Unity;
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

        public static string GetSKU(List<string> List_color_code_map)
        {
            string SKU = "";
            foreach (var item in List_color_code_map)
            {
                var def = item.Split(new char[] { ':', ';' });
                if (def[0] == "product_sku")
                {
                    if (def.Length > 1)
                    {
                        char[] charArr = def[1].ToCharArray();

                        if (charArr.Length == 9)
                        {
                            string temp = def[1].Remove(8);
                            SKU = temp;
                        }
                        else
                            SKU = def[1];
                    }
                }
            }
            return SKU;
        }
        
        public static string GetColorDefault(List<string> List_color_code_map)
        {
            string colorDefault = "";
            foreach (var item in List_color_code_map)
            {
                var def = item.Split(new char[] { ':', ';' });

                if (def.Length > 0)
                {
                    if (def[0] == "default_color_code")
                    {
                        if (def.Length > 1)
                        {
                            colorDefault = def[1];
                        }
                    }
                }
            }
            return colorDefault;
        }

        public static string GetHatShape(Product product)
        {
            string colorDefault = "";
            var tagShape = product.tags();

            foreach (var item in tagShape)
            {
                var def = item.Split(new char[] { ':', ';' });

                if (def.Length > 0)
                {
                    if (def[0] == "shape")
                    {
                        if (def.Length > 1)
                        {
                            colorDefault = def[1];
                        }
                    }
                }
            }
            return colorDefault;

        }


        public static string GetColorCodeMap(List<string> List_color_code_map, string tagColor)
        {
            string Code = "";
            foreach (var item in List_color_code_map)
            {
                var def = item.Split(new char[] { ':', ';' });

                if (def.Length > 1)
                {
                    if (def[0] +  ":" + def[1] == tagColor)
                    {
                        if (def.Length > 1)
                        {
                            Code = def[2];
                        }
                    }
                }
            }
            return Code;
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
        public string sku;
        public string color_code_map;
        public string URLImage;
        public Sprite HatImage;
        public string URLExperience;
        public Sprite HatExperience;
        public List<string> sizes = new List<string>();
    }
}
