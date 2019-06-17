using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Shopify.Examples.Helpers;
using UnityEngine.Events;
using System.Linq;

namespace goorinAR
{
    public class AddProductToCartEvent : UnityEvent<Shopify.Unity.Product, Shopify.Unity.ProductVariant>
    {

    }

    public class InformationPanel : MonoBehaviour
    {
        public AddProductToCartEvent OnAddProductToCart = new AddProductToCartEvent();
        public UnityEvent OnReturnToProducts;
        public UnityEvent OnTryProduct;
        public UnityEvent OnViewCart;

        [Header("Animations")]
        [SerializeField]
        private Button hatButton;
        [SerializeField]
        private GameObject empty;
        [SerializeField]
        private Transform info;
        [SerializeField]
        private Transform portada;
        [SerializeField]
        private Button backPanelButton;
        [SerializeField]
        private Button addToCartButton;
        [SerializeField]
        private Button tryButton;

        [Header("Buttons")]
        [SerializeField]
        private Button colorButton;
        [SerializeField]
        private Button SizeButton;

        [SerializeField]
        private List<ColorsAndSizes> m_ColorsAndSizes = new List<ColorsAndSizes>();
        [SerializeField]
        private List<GameObject> colors = new List<GameObject>();
        [SerializeField]
        private List<GameObject> sizes = new List<GameObject>();

        private bool isEnabled;

        public Shopify.Unity.Product CurrentProduct;
        public Shopify.Unity.ProductVariant CurrentVariant;


        [Header("Informations")]
        [SerializeField]
        private Text productTitle;
        [SerializeField]
        private Image productImage;


        [Header("Data")]
        [SerializeField]
        private Texture2D Image;
        [SerializeField]
        private Image icon;


        private void Start()
        {
            // Utils.CutImage(Image, (text) => { icon.sprite = text; });

            hatButton.onClick.AddListener(EnableInfo);

            backPanelButton.onClick.AddListener(() =>
            {
                OnReturnToProducts.Invoke();
                ResetInfoAnimation();
                ResetValue();
            });

            addToCartButton.onClick.AddListener(() => {
                OnAddProductToCart.Invoke(CurrentProduct, CurrentVariant);
                OnViewCart.Invoke();
            });

            tryButton.onClick.AddListener(()=>OnTryProduct.Invoke());
        }

        private void ResetInfoAnimation()
        {
            if (isEnabled)
            {
                info.DOLocalMoveY(0, 0.2f);
                portada.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.2f);
                empty.SetActive(true);
                isEnabled = false;
            }
        }

        private void EnableInfo()
        {
            if (!isEnabled)
            {
                info.DOLocalMoveY(280, 0.2f);
                portada.DOScale(Vector3.one, 0.2f);
                empty.SetActive(false);
                isEnabled = true;
            }
            else
            {
                info.DOLocalMoveY(0, 0.2f);
                portada.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.2f);
                empty.SetActive(true);
                isEnabled = false;
            }
        }

        private void ResetValue()
        {
            m_ColorsAndSizes.Clear();
            addToCartButton.interactable = true;

            //colors
            int amountColors = colorButton.gameObject.transform.parent.childCount;
            if (amountColors > 1)
            {
                for (int i = amountColors - 1; i > 0; i--)
                {
                    Destroy(colorButton.gameObject.transform.parent.GetChild(i).gameObject);
                }
            }
            colors.Clear();
            //sizes
            DeleteSizes();

        }

        public void SetCurrentProduct(Shopify.Unity.Product product)
        {
            productTitle.text = product.title();

            var images = (List<Shopify.Unity.Image>)product.images();
            StartCoroutine(ImageHelper.AssignImage(images[0].transformedSrc(), productImage));
            
            var options = new List<string>();
            var variants = (List<Shopify.Unity.ProductVariant>)product.variants();


            foreach (var variant in variants)
            {
                if (variant.availableForSale())
                {
                    string name = variant.title();
                    string[] names = name.Split('/');

                    var NameColor = names[0].Trim();
                    var NameSize = names[1].Trim();

                    if(m_ColorsAndSizes.Count > 0)
                    {
                        if (SearchName(m_ColorsAndSizes, NameColor) == false)
                        {
                            ColorsAndSizes d = new ColorsAndSizes
                            {
                                NameColor = NameColor
                            };
                            d.sizes.Add(NameSize);
                            m_ColorsAndSizes.Add(d);
                        }
                        else
                            AddSizes(m_ColorsAndSizes, NameColor, NameSize);
                    }
                    else
                    {
                        ColorsAndSizes d = new ColorsAndSizes
                        {
                            NameColor = NameColor
                        };
                        d.sizes.Add(NameSize);
                        m_ColorsAndSizes.Add(d);
                    }

                    options.Add(variant.title());
                }
                
            }

            //instantiate colors
            if (m_ColorsAndSizes.Count <= 0)
            {
                addToCartButton.interactable = false;
            }
            for (int i = 0; i < m_ColorsAndSizes.Count; i++)
            {
                var cl = Instantiate(colorButton.gameObject, colorButton.gameObject.transform.parent);
                cl.name = m_ColorsAndSizes[i].NameColor;
                colors.Add(cl);
                cl.transform.GetChild(0).GetComponent<Text>().text = m_ColorsAndSizes[i].NameColor;
                cl.GetComponent<Button>().onClick.AddListener(delegate { InstantiateSizes(cl.name);});
                cl.SetActive(true);
                //InstantiateSizes(m_ColorsAndSizes[0].NameColor);
            }

            CurrentProduct = product;
            CurrentVariant = variants[0];
        }

        private void InstantiateSizes(string name)
        {
            DeleteSizes();

            foreach (var item in colors)
                item.transform.GetChild(1).GetComponent<Image>().enabled = false;

            for (int i = 0; i < m_ColorsAndSizes.Count; i++)
            {
                if (name == m_ColorsAndSizes[i].NameColor)
                {
                    colors[i].gameObject.transform.GetChild(1).GetComponent<Image>().enabled = true;

                    for (int j  = 0; j < m_ColorsAndSizes[i].sizes.Count; j++)
                    {
                        var size = Instantiate(SizeButton.gameObject, SizeButton.gameObject.transform.parent);
                        size.name = m_ColorsAndSizes[i].sizes[j];
                        sizes.Add(size);

                        var nameSize = size.name;
                        char s = nameSize.FirstOrDefault();
                        string[] characters = nameSize.Split('-');
                        string theName = s.ToString();

                        if (characters.Length > 1)
                            theName = characters[0] + characters[1].FirstOrDefault().ToString();
                        
                        size.transform.GetChild(0).GetComponent<Text>().text = theName;

                        size.GetComponent<Button>().onClick.AddListener(delegate { GetColorAndSize(name, size.name); });
                        size.SetActive(true);
                        GetColorAndSize(name, m_ColorsAndSizes[i].sizes[0]);
                    }
                }
               
            }
        }

        private void GetColorAndSize(string color, string size)
        {
            string value = color + " / " + size;

            Debug.Log(value);

            var variants = (List<Shopify.Unity.ProductVariant>)CurrentProduct.variants();
            foreach (var item in variants)
            {
                if(item.title() == value)
                {
                    CurrentVariant = item;
                }
            }


            addToCartButton.interactable = true;

            foreach (var item in sizes)
                item.transform.GetChild(1).GetComponent<Image>().enabled = false;

            for (int i = 0; i < sizes.Count; i++)
            {
                if (size == sizes[i].name)
                {
                    sizes[i].gameObject.transform.GetChild(1).GetComponent<Image>().enabled = true;
                }
            }
        }

        private void DeleteSizes()
        {
            int amount = SizeButton.gameObject.transform.parent.childCount;
            if (amount > 1)
            {
                for (int j = amount - 1; j > 0; j--)
                {
                    Destroy(SizeButton.gameObject.transform.parent.GetChild(j).gameObject);
                }
            }
            sizes.Clear();
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

    [System.Serializable]
    public class ColorsAndSizes
    {
        public string NameColor;
        public List<string> sizes = new List<string>();
    }
}
