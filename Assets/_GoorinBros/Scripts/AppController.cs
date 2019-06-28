using Shopify.Examples.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;

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

        public HatSlidingContentAR HatSlidingContentAR;

        public static UnityAction OnShoopingCartPanelAR;
        public static UnityAction OnBackAR;

        [SerializeField]
        [Range(0,1f)]
        private float speedMovementPanel;
        private bool pluginActivated;

        [Header("Shopify")]
        public string AccessToken;
        public string ShopDomain;


        public void Start()
        {
            Tags.useTag = true;

            OnShoopingCartPanelAR += ShoopingCartPanelAR;
            OnBackAR += BackAR;

            ShopifyHelper.Init(AccessToken, ShopDomain);

            GalleryPanel.Init();

            Application.targetFrameRate = 60;

            GalleryPanel.OnShowProduct.AddListener(product =>
            {
                InformationPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(0, speedMovementPanel);
                // ShowPanel(ProductPanel.gameObject);
                InformationPanel.SetCurrentProduct(product);
            });

            HatSlidingContentAR.OnShowProduct.AddListener(p => 
            {
                InformationPanel.SetCurrentProduct(p);
            });

            InformationPanel.OnReturnToProducts.AddListener(() =>
            {
                InformationPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(719, speedMovementPanel);
            });

            InformationPanel.OnViewCart.AddListener(() => 
            {
                HatCartPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(0, speedMovementPanel);
            });

            InformationPanel.OnTryProduct.AddListener(() =>
            {
                InformationPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(719, speedMovementPanel);
                GalleryPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(719, speedMovementPanel).OnComplete(()=> 
                {
                    if (pluginActivated == false)
                    {
                        FaceTrackerController.InitialPlugin();
                        pluginActivated = true;
                    }
                   /// fade.DOFade(0, 2f);
                  //  HatSlidingContentAR.LoadContentGG();
                 });
            });

            HatCartPanel.OnReturnToProducts.AddListener(() => 
            {
                HatCartPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(-719, speedMovementPanel);
            });

            InformationPanel.OnAddProductToCart.AddListener(HatCartPanel.AddToCart);
            //  GalleryPanel.OnNetworkError.AddListener(() => RenderError("Could not find products."));




            //eliminar metodo
            //if (backButtonAR != null)
            //{
            //    backButtonAR.onClick.AddListener(() =>
            //    {
            //        InformationPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(0, speedMovementPanel);
            //        GalleryPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(0, speedMovementPanel);
            //        Object3D.StopPlugin();
            //       // fade.DOFade(1,0.1f);
            //    });
            //}
        }
        private void OnDestroy()
        {
            OnShoopingCartPanelAR -= ShoopingCartPanelAR;
            OnBackAR -= BackAR;
        }

        private void ShoopingCartPanelAR()
        {
            HatCartPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(0, speedMovementPanel);
        }

        private void BackAR()
        {
            InformationPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(0, speedMovementPanel);
            GalleryPanel.gameObject.GetComponent<RectTransform>().DOLocalMoveX(0, speedMovementPanel);
         //   FaceTrackerController.StopPlugin();
          //  pluginActivated = false;
        }
    }
}
