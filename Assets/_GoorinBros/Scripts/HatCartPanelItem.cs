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
            Quantity.text = quantity.ToString();
            Price.text = "$ " +  variant.price().ToString();

            var options = variant.selectedOptions();
            string sku = "";
            string code = "";
            if (options.Count > 0)
            {
                string tagColor = "color_code_map:" + options[0].value();
                code = Utils.GetColorCodeMap(product.tags(), tagColor);
                sku = Utils.GetSKU(product.tags());
            }
        
            try
            {
                foreach (Shopify.Unity.ImageEdge item in product.images().edges())
                {
                    string URLglobal = item.node().transformedSrc("large");
                    if (sku != "" && code != "")
                    {
                        string text = sku + "-" + code + "-F01";
                        if (URLglobal.Contains(text))
                        {
                            Debug.Log("<color=blue> " + URLglobal + "</color>");
                            StartCoroutine(Utils.OnDownloadImage(URLglobal, (img) =>
                            {
                                ProductImage.sprite = img;
                            }));
                        }
                    }
                }

            }
            catch (NullReferenceException)
            {
                var images = (List<Shopify.Unity.Image>)product.images();
                _imageSrc = images[0].transformedSrc();
                Debug.Log("GG");
            }

            _currentVariant = variant;
        }
    }

    

}
