using Shopify.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace goorinAR
{

    public class Utils 
    {
        public static Sprite CutTexture(Texture2D tex)
        {
            Sprite text = Sprite.Create(tex, new Rect(tex.width / 2, tex.height / 2, tex.width * 0.1f, tex.height * 0.1f), Vector2.zero);
            return text;
        }

        public static Sprite CutTextureIcon(Texture2D tex)
        {
            Sprite text = Sprite.Create(tex, new Rect(0, 0, tex.width * 0.8f, tex.height), Vector2.zero);
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

        public static void GetBrimGruop(Product product,Text brimGruop, Text brimGruopTitle)
        {
            var tagShape = product.tags();
            foreach (var item in tagShape)
            {
                var def = item.Split(new char[] { ':', ';' });
                if (def.Length > 0)
                {
                    if(def[0]== "brim_category")
                    {
                        if (def.Length > 1)
                        {
                            Debug.Log("Brim Gruop");
                            brimGruop.text = def[1];
                            brimGruopTitle.text = "Brim Gruop";
                            return;
                        }
                    }
                    else if (def[0] == "subshape")
                    {
                        if (def.Length > 1)
                        {
                            Debug.Log("Style");
                            brimGruop.text = def[1];
                            brimGruopTitle.text = "Style";
                            return;
                        }
                    }
                    else
                    {
                        brimGruop.text = "";
                    }
                }
            }
            return;
        }

        public static void GetCrownShape(Product product, Text crown, Text crownTitle)
        {
            var tagShape = product.tags();
            foreach (var item in tagShape)
            {
                var def = item.Split(new char[] { ':', ';' });
                if (def.Length > 0)
                {
                    if (def[0] == "crown_type")
                    {
                        if (def.Length > 1)
                        {
                            Debug.Log("Crown Shape");
                            crown.text = def[1];
                            crownTitle.text = "Crown Shape";
                            return;
                        }
                    }
                    else if (def[0] == "made_in")
                    {
                        if (def.Length > 1)
                        {
                            Debug.Log("Fabrication");
                            crown.text = "Made in The " + def[1];
                            crownTitle.text = "Fabrication";
                            return;
                        }
                    }
                    else
                    {
                        crown.text = "";
                    }
                }
            }
            return;
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

        public static List<string> OrdenarList(List<string> sizes)
        {
            List<string> newSize = new List<string>();
            string size = "";
            List<int> amount = new List<int>();
            int index = -1;
            foreach (var item in sizes)
            {
                switch (item)
                {
                        //Kid
                    case "One Size":
                        index = 0;
                        break;
                        //adult
                    case "Small":
                        index = 1;
                        break;
                    case "Medium":
                        index = 2;
                        break;
                    case "Large":
                        index = 3;
                        break;
                    case "X-Large":
                        index = 4;
                        break;
                    case "XX-Large":
                        index = 5;
                        break;
                    default:
                        index = 6;
                        break;
                }

                amount.Add(index);
            }

            int[] arr = amount.ToArray();
            Array.Sort(arr);
            Array.Reverse(arr);
            foreach (var item in arr)
            {
                switch (item)
                {
                    //Kid
                    case 0:
                        size = "One Size";
                        break;
                    //adult
                    case 1:
                        size = "Small";
                        break;
                    case 2:
                        size = "Medium";
                        break;
                    case 3:
                        size = "Large";
                        break;
                    case 4:
                        size = "X-Large";
                        break;
                    case 5:
                        size = "XX-Large";
                        break;
                    default:
                        size = "Other";
                        break;
                }

                newSize.Add(size);
            }
            
            return newSize;
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
        public List<ReferienceHats> ReferenceHats = new List<ReferienceHats>();
        public string URLExperience;
        public Sprite HatExperience;
        public List<string> sizes = new List<string>();
    }
    [System.Serializable]
    public class ReferienceHats
    {
        public string name;
        public string URL;
        public Sprite Image;
    }

    
    [System.Serializable]
    public class SummaryBox
    {
        public string title;
        public string text;
    }
    [System.Serializable]
    public class SpecsVariant
    {
        public string title;
        public string text;
    }
    [System.Serializable]
    public class Materials
    {
        public string title;
        public string text;
    }

    
    [System.Serializable]
    public class Info_JSON
    {
        
        public List<SummaryBox> summary_box = new List<SummaryBox>();
        public List<SpecsVariant> specs_variant = new List<SpecsVariant>();
        public List<Materials> materials = new List<Materials>();
   
    }










}
