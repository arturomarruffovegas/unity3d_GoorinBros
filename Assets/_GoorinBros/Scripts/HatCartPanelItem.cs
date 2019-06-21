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

            StartCoroutine(
                ImageHelper.AssignImage(
                    _imageSrc,
                    ProductImage
                )
            );
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
                _imageSrc = variant.image().transformedSrc();
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
