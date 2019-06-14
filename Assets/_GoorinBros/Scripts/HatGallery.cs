using Shopify.Examples.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
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

        public Text GetNameHat()
        {
            return nameHat;
        }

        public void SetNameHat(Text value)
        {
            nameHat = value;
        }

        public Image GetImageHat()
        {
            return imageHat;
        }

        public void SetImageHat(Image value)
        {
            imageHat = value;
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

            nameHat.text = product.title();

            var variants = (List<Shopify.Unity.ProductVariant>)product.variants();
            priceHat.text = variants.First().price().ToString("C");

            var images = (List<Shopify.Unity.Image>)product.images();
            if (images.Count > 0)
            {
                _imageSrc = images.First().transformedSrc("compact");
            }

            hatButton.onClick.AddListener(() => OnClick.Invoke());
        }
    }
}