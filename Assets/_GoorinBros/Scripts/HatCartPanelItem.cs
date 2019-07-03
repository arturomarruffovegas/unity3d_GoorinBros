using Shopify.Examples.Helpers;
using Shopify.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace goorinAR
{
    public class VariantLineItemQuantityAdjustmentEvent : UnityEvent<ProductVariant, int>
    {
    }

    public class HatCartPanelItem : MonoBehaviour
    {
        public VariantLineItemQuantityAdjustmentEvent OnVariantLineItemQuantityAdjustment = new VariantLineItemQuantityAdjustmentEvent();

        private string _imageSrc;
        private ProductVariant _currentVariant;

        public Text ProductTitle;
        public string VariantTitle;
        public Text Quantity;
        public Text Price;
        public UnityEngine.UI.Image ProductImage;
        public Button IncreaseQuantity;
        public Button DecreaseQuantity;

        public void Start()
        {
            IncreaseQuantity.onClick.AddListener(() => OnVariantLineItemQuantityAdjustment.Invoke(_currentVariant, 1));
            DecreaseQuantity.onClick.AddListener(() => OnVariantLineItemQuantityAdjustment.Invoke(_currentVariant, -1));
        }

        public void SetCurrentProduct(Product product, ProductVariant variant, int quantity)
        {
            gameObject.SetActive(true);

            ProductTitle.text = product.title();
           // VariantTitle.gameObject.SetActive(variant.title() != "Default Title");
           // VariantTitle.text = variant.title();
            Quantity.text = quantity.ToString();
            Price.text = "$ " +  variant.price().ToString();

            try
            {
                var List_color_code_map = product.tags();
                string colorDefault = "";
                string sku = "";
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
                            Debug.Log(_URLImage);
                            StartCoroutine(Utils.OnDownloadImage(_URLImage, (spri) =>
                            {
                                ProductImage.sprite = spri;
                            }));
                        }

                    }
                }
            }
            catch (NullReferenceException)
            {
                var images = (List<Shopify.Unity.Image>)product.images();
                _imageSrc = images[0].transformedSrc();
            }

            _currentVariant = variant;
        }
    }

    

}
