using Shopify.Examples.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

            HatCartPanel.OnReturnToProducts.AddListener(() => 
            {
                HatCartPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(-719, 0.2f);
            });

            InformationPanel.OnAddProductToCart.AddListener(HatCartPanel.AddToCart);
            //  GalleryPanel.OnNetworkError.AddListener(() => RenderError("Could not find products."));
        }
    }
}
