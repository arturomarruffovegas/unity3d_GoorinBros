using Shopify.Examples.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace goorinAR
{
    public class AppController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField]
        private GalleryPanel GalleryPanel;
        [SerializeField]
        private InformationPanel InformationPanel;
        [SerializeField]
        private HatCartPanel HatCartPanel;

        //eliminar este boton
        public Button backButtonAR;

        [Header("Shopify")]
        public string AccessToken;
        public string ShopDomain;


        public void Start()
        {
            Tags.useTag = true;

            ShopifyHelper.Init(AccessToken, ShopDomain);

            GalleryPanel.Init();

            GalleryPanel.OnShowProduct.AddListener(product =>
            {
                InformationPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(0, 0.2f);
                // ShowPanel(ProductPanel.gameObject);
                InformationPanel.SetCurrentProduct(product);
            });

            InformationPanel.OnReturnToProducts.AddListener(() =>
            {
                InformationPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(719, 0.2f);
            });

            InformationPanel.OnViewCart.AddListener(() => 
            {
                HatCartPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(0, 0.2f);
            });

            InformationPanel.OnTryProduct.AddListener(() =>
            {
                InformationPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(719, 0.2f);
                GalleryPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(719, 0.2f).OnComplete(()=> 
                {
                    HatSlidingContentAR.LoadContentGG();
                 });
            });

            HatCartPanel.OnReturnToProducts.AddListener(() => 
            {
                HatCartPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(-719, 0.2f);
            });

            InformationPanel.OnAddProductToCart.AddListener(HatCartPanel.AddToCart);
            //  GalleryPanel.OnNetworkError.AddListener(() => RenderError("Could not find products."));




            //eliminar metodo
            if (backButtonAR != null)
            {
                backButtonAR.onClick.AddListener(() =>
                {
                    InformationPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(0, 0.2f);
                    GalleryPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(0, 0.2f);
                });
            }
        }
    }
}
