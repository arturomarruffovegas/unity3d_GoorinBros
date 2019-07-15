﻿using Shopify.Examples.Helpers;
using Shopify.Unity;
using Shopify.Unity.SDK;
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

        [Header("Categories")]
        public Transform contentCategoryButtons;
        public GameObject categoryButton;
        public List<GameObject> listCategoryButtons = new List<GameObject>();


        [Header("Content")]
        [SerializeField]
        private GameObject ScrollView;
        [SerializeField]
        private Transform contentHatPanel;
        [SerializeField]
        List<GameObject> scrollViews = new List<GameObject>();
       
        public bool _hitEndCursor;

        public GameObject laodingPanel;
        private bool _wasScrolledToBottom;
        private RectTransform _rectTransform;

        private readonly List<HatGallery> _lineItems = new List<HatGallery>();
        private List<string> _addedProductIds = new List<string>();

        private string _after;

        private UnityAction<Product, HatGallery> ActionProduct;

        private HatData hatData;

        public static UnityAction<Product> galleryAR;

        [Header("colors")]
        public Color textColor;
        public Color baseColor;

        //[Header("Buttons")]
        //[SerializeField]
        //private Button feminine;
        //[SerializeField]
        //private Button fedora;
        //[SerializeField]
        //private Button flatcap;


        public IEnumerator OnCreateCategoryButtons()
        {
            var categories = FindObjectOfType<InitialApp>().shapes;
            var PP = InitialApp.m_product;

            for (int i = 0; i < categories.Count; i++)
            {
                yield return new WaitForEndOfFrame();
                var cat = Instantiate(categoryButton, contentCategoryButtons);
                cat.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = categories[i];
                cat.name = categories[i];
                cat.GetComponent<Button>().onClick.AddListener(delegate { OnDisableScrolls(cat.name); });
                cat.SetActive(true);
                listCategoryButtons.Add(cat);

                var scroll = Instantiate(ScrollView, contentHatPanel);
                scroll.name = scroll.name + " " + categories[i];
                scroll.SetActive(true);
                scrollViews.Add(scroll);
            }

            yield return new WaitForEndOfFrame();

            StartCoroutine( OnInstantiateHats());
        }

        private IEnumerator OnInstantiateHats()
        {
            var PP = InitialApp.m_product;
            var categories = FindObjectOfType<InitialApp>().shapes;

            for (int i = 0; i < PP.Count; i++)
            {
                var tags = PP[i].tags();

                foreach (var item in tags)
                {
                    for (int j = 0; j < categories.Count; j++)
                    {
                   
                        if (item == "shape:" + categories[j])
                        {
                            StartCoroutine(AddProduct(PP[i], scrollViews[j]));
                        }
                    }
                }
            }

            yield return new WaitForSeconds(1f);
            OnDisableScrolls(categories[0]);
        }

        private void OnDisableScrolls(string name)
        {
            for (int i = 0; i < scrollViews.Count; i++)
            {
                if (scrollViews[i].name.Contains(name))
                {
                    listCategoryButtons[i].GetComponent<UnityEngine.UI.Image>().color = Color.black;
                    scrollViews[i].SetActive(true);

                }
                else
                {
                    listCategoryButtons[i].GetComponent<UnityEngine.UI.Image>().color = Color.white;
                    scrollViews[i].SetActive(false);
                }
            }

        }

        public void Start()
        {

            StartCoroutine(OnCreateCategoryButtons());
           // DefaultQueries.MaxProductPageSize = 10;

            //feminine.onClick.AddListener(() => 
            //{
            //    Tags.SetTag(true, "Feminine");
            //    OnChangeScroll(ScrollViewFeminine.name);
            //    galleryContent = galleryContentFeminine;
            //    feminine.GetComponent<UnityEngine.UI.Image>().color = Color.black;
            //    fedora.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            //    flatcap.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            //    FetchProducts();
            //});

            //fedora.onClick.AddListener(() =>
            //{
            //    Tags.SetTag(true, "Fedora");
            //    OnChangeScroll(ScrollViewFedora.name);
            //    galleryContent = galleryContentFedora;
            //    feminine.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            //    fedora.GetComponent<UnityEngine.UI.Image>().color = Color.black;
            //    flatcap.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            //    FetchProducts();
            //});

            //flatcap.onClick.AddListener(() =>
            //{
            //    Tags.SetTag(true, "Flatcap");
            //    OnChangeScroll(ScrollViewFlatcap.name);
            //    galleryContent = galleryContentFlatcap;
            //    feminine.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            //    fedora.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            //    flatcap.GetComponent<UnityEngine.UI.Image>().color = Color.black;
            //    FetchProducts();
            //});
        }

        public void OnChangeScroll(string name)
        {
            //ScrollViewFeminine.gameObject.SetActive(ScrollViewFeminine.name.Equals(name));
            //ScrollViewFedora.gameObject.SetActive(ScrollViewFedora.name.Equals(name));
            //ScrollViewFlatcap.gameObject.SetActive(ScrollViewFlatcap.name.Equals(name));
        }


        public void Init()
        {
          // galleryContent = galleryContentFedora;

            hatData = FindObjectOfType<HatData>();

          //  FetchProducts();

            _rectTransform = GetComponent<RectTransform>();
           // ScrollViewFeminine.onValueChanged.AddListener(OnScrollRectPositionChanged);
           // ScrollViewFedora.onValueChanged.AddListener(OnScrollRectPositionChanged);
            //ScrollViewFlatcap.onValueChanged.AddListener(OnScrollRectPositionChanged);

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

                        //AddProduct(product);
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

        public IEnumerator AddProduct(Product product, GameObject obj)
        {
            if (_addedProductIds.Contains(product.id()))
            {
                yield return null;
            }

            laodingPanel.SetActive(false);

            Debug.Log(Utils.GetHatShape(product));

            _addedProductIds.Add(product.id());

            HatGallery hatGallery = obj.GetComponent<ScrollGallery>().hatGallery;
            Transform trans = obj.GetComponent<ScrollGallery>().content.transform;

           var instance = Instantiate(hatGallery);

            instance.transform.SetParent(trans, false);
            yield return new WaitForEndOfFrame();
           instance.SetCurrentProduct(product, _lineItems.Count);
            yield return new WaitForEndOfFrame();
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
