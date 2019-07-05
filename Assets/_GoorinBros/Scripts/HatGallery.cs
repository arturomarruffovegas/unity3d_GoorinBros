using Shopify.Examples.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace goorinAR
{
    public class HatGallery : MonoBehaviour
    {
        private string _imageSrc;
        public int MaxDescriptionCharacters = 150;
        public bool IsLoaded { get; private set; }
        public UnityEvent OnClick;

        [Header("info")]
        [SerializeField]
        private string sku;
        [SerializeField]
        private string colorDefault;
        [SerializeField]
        private string URLImage;


        [SerializeField]
        private Text nameHat;
        [SerializeField]
        private Image imageHat;
        [SerializeField]
        private Text priceHat;
        [SerializeField]
        private Button hatButton;

        public string GetNameHat()
        {
            return nameHat.text;
        }

        public void SetNameHat(string value)
        {
            nameHat.text = value;
        }

        public Image GetImageHat()
        {
            return imageHat;
        }

        public void SetImageHat(Sprite value)
        {
            imageHat.sprite = value;
        }

        public Text GetPriceHat()
        {
            return priceHat;
        }

        public void SetPriceHat(Text value)
        {
            priceHat = value;
        }

        public Button GetHatButton()
        {
            return hatButton;
        }

        public void SetHatButton(Button value)
        {
            hatButton = value;
        }

        public void Load()
        {
            IsLoaded = true;
            if (!string.IsNullOrEmpty(_imageSrc))
            {
                StartCoroutine(
                    ImageHelper.AssignImage(
                        _imageSrc,
                        imageHat
                    )
                );
            }
        }

        public void SetCurrentProduct(Shopify.Unity.Product product, int index)
        {
            gameObject.SetActive(true);

            SetNameHat(product.title());

            var variants = (List<Shopify.Unity.ProductVariant>)product.variants();

            var List_color_code_map = product.tags();
            
            colorDefault = Utils.GetColorDefault(List_color_code_map);
            sku = Utils.GetSKU(List_color_code_map);

            var img = product.images();

            foreach (Shopify.Unity.ImageEdge item in img.edges())
            {
                string URLglobal = item.node().transformedSrc("large");

                if (colorDefault != "" && sku != "")
                {
                    if (URLglobal.Contains(sku + "-" + colorDefault + "-F01"))
                    {
                        string _URLImage = URLglobal;
                        URLImage = URLglobal;
                        Debug.Log("<color=blue> " + "hat: "+ product.title() + " URL" +   _URLImage + "</color>");
                        StartCoroutine(Utils.OnDownloadImage(_URLImage, (spri) =>
                        {
                            SetImageHat(spri);
                        }));
                    }

                }
            }

            string price = variants.First().price().ToString();
            priceHat.text = "$ " + price;
            
            hatButton.onClick.AddListener(() => OnClick.Invoke());
        }

        //private IEnumerator OnDownloadImage(string myURL, UnityAction<Sprite> image)
        //{
        //    UnityWebRequest www = UnityWebRequestTexture.GetTexture(myURL);
        //    yield return www.SendWebRequest();

        //    Texture2D tex = DownloadHandlerTexture.GetContent(www);
        //    Sprite spri = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        //    image?.Invoke(spri);
        //}
    }
}