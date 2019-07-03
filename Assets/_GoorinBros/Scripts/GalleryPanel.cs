using Shopify.Examples.Helpers;
using Shopify.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

        public bool _hitEndCursor;

        public ScrollRect ScrollView;
        private bool _wasScrolledToBottom;
        private RectTransform _rectTransform;

        private readonly List<HatGallery> _lineItems = new List<HatGallery>();
        private List<string> _addedProductIds = new List<string>();

        private string _after;

        private UnityAction<Product, HatGallery> ActionProduct;

        private HatData hatData;

        public static UnityAction<Product> galleryAR;
        
        public void Init()
        {
            hatData = FindObjectOfType<HatData>();

            FetchProducts();

            _rectTransform = GetComponent<RectTransform>();
            ScrollView.onValueChanged.AddListener(OnScrollRectPositionChanged);

            ActionProduct += OnProduct;

        }

        private void OnDestroy()
        {
            ActionProduct -= OnProduct;

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



            Debug.Log(Utils.GetHatShape(product));

            _addedProductIds.Add(product.id());

            var instance = Instantiate(HatGallery);

            instance.transform.SetParent(galleryContent, false);

            instance.SetCurrentProduct(product, _lineItems.Count);
            instance.Load();

            instance.OnClick.AddListener(delegate { OnProduct(product, instance); });

            _lineItems.Add(instance);

            galleryAR(product);
        }

        private void OnProduct(Product product, HatGallery hatGallery)
        {
            hatData.SethatName(hatGallery.GetNameHat());
            hatData.SethatImage(hatGallery.GetImageHat().sprite);
            OnShowProduct.Invoke(product);
        }
        

        private void OnScrollRectPositionChanged(Vector2 scrollOffset)
        {
            var visibleProductViews = _lineItems.Where((x) => {
                if (x.IsLoaded) return false;

                var productViewLocalPosition = _rectTransform.InverseTransformPoint(x.transform.position);
                return productViewLocalPosition.y > _rectTransform.rect.yMin
                    && productViewLocalPosition.y < _rectTransform.rect.yMax;
            });

            foreach (var productView in visibleProductViews)
            {
                productView.Load();
            }

            bool isScrolledToBottom = scrollOffset.y < 0;

            if (!_wasScrolledToBottom && isScrolledToBottom)
            {
                if (!_hitEndCursor)
                {
                    FetchProducts();
                }
            }

            _wasScrolledToBottom = isScrolledToBottom;
        }



        private bool SearchName(List<ColorsAndSizes> list, string name)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].NameColor == name)
                    return true;
            }

            return false;
        }

        private void AddSizes(List<ColorsAndSizes> list, string name, string size)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].NameColor == name)
                    list[i].sizes.Add(size);
            }
        }


    }

   
}

public class ShowProductEvent : UnityEvent<Shopify.Unity.Product>
{
}
