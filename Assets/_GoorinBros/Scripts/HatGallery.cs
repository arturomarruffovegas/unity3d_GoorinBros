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

            string price = variants.First().price().ToString();
            priceHat.text = "$ " + price;

            //var images = (List<Shopify.Unity.Image>)product.images();
            //if (images.Count > 0)
            //{
            //    _imageSrc = images.First().transformedSrc("compact");
            //}


            if (variants[0].image() != null)
            {
                string _URLImage = variants[0].image().transformedSrc("large");

                StartCoroutine(Utils.OnDownloadImage(_URLImage, (spri) =>
                {
                    SetImageHat(spri);
                }));
            }

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