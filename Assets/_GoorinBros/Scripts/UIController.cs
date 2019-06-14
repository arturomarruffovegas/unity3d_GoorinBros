using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace goorinAR
{

    public class UIController : MonoBehaviour
    {
        [Header("Gallery UI")]
        [SerializeField]
        private Transform galleryContent;
        [SerializeField]
        private GameObject hatPrefab;
        [SerializeField]
        private int Amount = 10;

        [Header("Panels")]
        [SerializeField]
        private Transform galleryPanel;
        [SerializeField]
        private Transform infoPanel;
        [SerializeField]
        private Transform cartPanel;
        [SerializeField]
        private Transform basicPanel;
        [SerializeField]
        private Transform morePanel;

        [Header("Buttons")]
        [SerializeField]
        private Button moreInfoButton;
        [SerializeField]
        private Button backButton;

        [SerializeField]
        private Button cartButton;
        [SerializeField]
        private Button backCButton;


        private void Start()
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    InstantiateHats(hatPrefab, galleryContent);
            //}

            //  moreInfoButton.onClick.AddListener(OnMoreInfo);

            //backButton.onClick.AddListener(delegate { OnActivePanels(647, infoPanel); });

            //cartButton.onClick.AddListener(delegate { OnActivePanels(0, cartPanel); });
            //backCButton.onClick.AddListener(delegate { OnActivePanels(-647, cartPanel); });
        }

        //private void OnMoreInfo()
        //{
        //    basicPanel.gameObject.SetActive(false);
        //    morePanel.gameObject.SetActive(true);
        //    morePanel.DOLocalMoveY(0, 0.5f);

        //    backButton.onClick.RemoveAllListeners();
        //    backButton.onClick.AddListener(OnBackInfo);
        //}

        //private void OnBackInfo()
        //{
        //    morePanel.DOLocalMoveY(-250, 0.5f).OnComplete(()=> 
        //    {
        //        morePanel.gameObject.SetActive(false);
        //        basicPanel.gameObject.SetActive(true);
        //    });


        //    backButton.onClick.RemoveAllListeners();
        //    backButton.onClick.AddListener(delegate { OnActivePanels(647, infoPanel); });
        //}

        //private void InstantiateHats(GameObject prefab, Transform parent)
        //{
        //    var hat = Instantiate(prefab, parent);
        //    hat.SetActive(true);
        //    hat.GetComponent<HatGallery>().GetHatButton().onClick.AddListener(delegate { OnActivePanels(0f,infoPanel); });
        //}

        //private void OnActivePanels(float posX ,Transform obj)
        //{
        //    obj.DOLocalMoveX(posX, 0.2f);
        //}

    }
}
