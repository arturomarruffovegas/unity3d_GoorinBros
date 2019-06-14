using Shopify.Examples.Helpers;
using Shopify.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace goorinAR
{
    public class GalleryPanel : MonoBehaviour
    {
        public ShowProductEvent OnShowProduct = new ShowProductEvent();
        public UnityEvent OnNetworkError;

        [SerializeField]
        private HatGallery HatGallery;
        [SerializeField]
        private Transform galleryContent;

        private readonly List<HatGallery> _lineItems = new List<HatGallery>();
        private List<string> _addedProductIds = new List<string>();

        private string _after;
        public bool _hitEndCursor;

        public void Init()
        {
            FetchProducts();

        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                FetchProducts();
            }


        }

        private void FetchProducts()
        {
            ShopifyHelper.FetchProducts(
                delegate (List<Product> products, string cursor)
                {

                    foreach (var product in products)
                    {

                        AddProduct(product);
                    }


                    _after = cursor;
                    _hitEndCursor = _after == null;
                },
                delegate
                {
                    OnNetworkError.Invoke();
                },
                _after
            );


        }

        public void AddProduct(Product product)
        {
            if (_addedProductIds.Contains(product.id()))
            {
                return;
            }

            _addedProductIds.Add(product.id());

            var instance = Instantiate(HatGallery);

            instance.transform.SetParent(galleryContent, false);

            instance.SetCurrentProduct(product, _lineItems.Count);
            instance.Load();

            instance.OnClick.AddListener(() => OnShowProduct.Invoke(product));

            _lineItems.Add(instance);



        }


    }
}

public class ShowProductEvent : UnityEvent<Shopify.Unity.Product>
{
}
