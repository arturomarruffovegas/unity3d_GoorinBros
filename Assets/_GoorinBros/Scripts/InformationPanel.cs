using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.Networking;

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

        [Header("Icons")]
        [SerializeField]
        private Sprite waitIcon;

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
        private Button sizingGuideButton;

        [Header("data")]
        [SerializeField]
        private string hatName;
        [SerializeField]
        private List<ColorsAndSizes> m_ColorsAndSizes = new List<ColorsAndSizes>();
        public bool finishDownloadImage;
        private List<GameObject> colors = new List<GameObject>();
        private List<GameObject> sizes = new List<GameObject>();
       // public List<Sprite> m_hatImages = new List<Sprite>();

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
        private Image icon;
        [SerializeField]
        private GameObject iconMask;
        [SerializeField]
        private Image onlyHat;

        private HatData hatData;

        private string nameLocal;

        private bool GetImage;

        private Shopify.Unity.Product product;

        private void Start()
        {
            // Utils.CutImage(Image, (text) => { icon.sprite = text; });
            hatData = FindObjectOfType<HatData>();

            hatButton.onClick.AddListener(EnableInfo);

            backPanelButton.onClick.AddListener(() =>
            {
               // ResetValue();
                OnReturnToProducts.Invoke();
                ResetInfoAnimation();
                iconMask.SetActive(false);
                onlyHat.gameObject.SetActive(false);
                productImage.sprite = waitIcon;
            });

            addToCartButton.onClick.AddListener(() => {
                OnAddProductToCart.Invoke(CurrentProduct, CurrentVariant);
                OnViewCart.Invoke();
            });

            tryButton.onClick.AddListener(OnActionTry);

            sizingGuideButton.onClick.AddListener(() => 
            {
                Application.OpenURL("https://firebasestorage.googleapis.com/v0/b/goorin-bros.appspot.com/o/Sizes_pdf.pdf?alt=media&token=30e99e0c-e101-4a3d-a9a7-181740ec84cd");
            });
        }

        public void OnActionTry()
        {
            OnTryProduct.Invoke();

            if (product != null)
            {
                HatSlidingContentAR.OnSearchHat(m_ColorsAndSizes, product, CurrentVariant);
            }
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
            if(hatData.GethatName() == hatName)
            {
                onlyHat.sprite = hatData.GethatImage();
                onlyHat.gameObject.SetActive(true);
                iconMask.SetActive(false);

                return;
            }
            else
            {
                ResetValue();
                finishDownloadImage = false;
                //productImage.sprite = waitIcon;
            }
            this.product = product;

            hatName = product.title();
            productTitle.text = hatName;

           // productImage.sprite = hatData.GethatImage();
            
            var options = new List<string>();
            var variants = (List<Shopify.Unity.ProductVariant>)product.variants();

            var List_color_code_map = product.tags();

            var img = product.images();

            foreach (var variant in variants)
            {
                if (variant.availableForSale())
                {

                //    var URLDownloadImage = variant.image().transformedSrc("large");
                    string name = variant.title();
                    string[] names = name.Split('/');

                    var NameColor = names[0].Trim();
                    var NameSize = names[1].Trim();

                    if(m_ColorsAndSizes.Count > 0)
                    {
                        
                        if (SearchName(m_ColorsAndSizes, NameColor) == false)
                        {
                            ColorsAndSizes d = new ColorsAndSizes();
                            d.NameColor = NameColor;
                            d.sku = Utils.GetSKU(List_color_code_map);
                            string tagColor = "color_code_map:" + NameColor;
                            d.color_code_map = Utils.GetColorCodeMap(List_color_code_map, tagColor);

                            string _URLImage = "";
                            string _URLExperienceImage = "";
                            foreach (Shopify.Unity.ImageEdge item in img.edges())
                            {
                                string URLglobal = item.node().transformedSrc("large");
                                string URLExperienceglobal = item.node().transformedSrc("grande");
                                if (d.sku != "" && d.color_code_map != "")
                                {
                                    string text = d.sku + "-" + d.color_code_map + "-F01";
                                    if (URLglobal.Contains(text))
                                    {
                                        _URLImage = URLglobal;
                                        Debug.Log("URL: " + _URLImage);
                                    }

                                    string v = d.sku + "-" + d.color_code_map;
                                    if (URLExperienceglobal.Contains(v + "-HF01"))
                                        _URLExperienceImage = URLExperienceglobal;
                                    else if(URLExperienceglobal.Contains(v + "-HM01"))
                                        _URLExperienceImage = URLExperienceglobal;
                                    else if(URLExperienceglobal.Contains(v + "-MF01"))
                                        _URLExperienceImage = URLExperienceglobal;
                                }



                            }

                            d.URLImage = _URLImage;
                            d.URLExperience = _URLExperienceImage;
                            d.sizes.Add(NameSize);
                            m_ColorsAndSizes.Add(d);
                            
                        }
                        else
                            AddSizes(m_ColorsAndSizes, NameColor, NameSize);

                        
                    }
                    else
                    {
                        ColorsAndSizes d = new ColorsAndSizes();
                        d.NameColor = NameColor;
                        d.sku = Utils.GetSKU(List_color_code_map);
                        string tagColor = "color_code_map:" + NameColor;
                        d.color_code_map = Utils.GetColorCodeMap(List_color_code_map, tagColor);

                        string _URLImage = "";
                        string _URLExperienceImage = "";
                        foreach (Shopify.Unity.ImageEdge item in img.edges())
                        {
                            string URLglobal = item.node().transformedSrc("large");
                            string URLExperienceglobal = item.node().transformedSrc("grande");

                            if (d.sku != "" && d.color_code_map != "")
                            {
                                string text = d.sku + "-" + d.color_code_map + "-F01";
                                if (URLglobal.Contains(text))
                                {
                                    _URLImage = URLglobal;
                                    Debug.Log("URL: " + _URLImage);
                                }

                                string v = d.sku + "-" + d.color_code_map;
                                if (URLExperienceglobal.Contains(v + "-HF01"))
                                    _URLExperienceImage = URLExperienceglobal;
                                else if (URLExperienceglobal.Contains(v + "-HM01"))
                                    _URLExperienceImage = URLExperienceglobal;
                                else if(URLExperienceglobal.Contains(v + "-MF01"))
                                    _URLExperienceImage = URLExperienceglobal;
                            }

                            

                        }

                        d.URLImage = _URLImage;
                        d.URLExperience = _URLExperienceImage;
                        d.sizes.Add(NameSize);
                        m_ColorsAndSizes.Add(d);

                        //StartCoroutine(OnDownloadImage(URLDownloadImage, (spri) =>
                        //{
                        //    m_ColorsAndSizes[0].HatImage = spri;
                        //}));
                    }

                    options.Add(variant.title());
                }
                
            }

             //GetImages
            OnGetHatImageColors();
            OnGetHatExperience();

            //instantiate colors
            if (m_ColorsAndSizes.Count <= 0)
            {
                addToCartButton.interactable = false;
            }
            for (int i = 0; i < m_ColorsAndSizes.Count; i++)
            {
                var cl = Instantiate(colorButton.gameObject, colorButton.gameObject.transform.parent);
                cl.name = m_ColorsAndSizes[i].NameColor;
                int indexHat = i;
                colors.Add(cl);
                cl.GetComponent<Button>().onClick.AddListener(delegate { InstantiateSizes(true,cl.name, indexHat);});
                cl.SetActive(true);
                nameLocal = m_ColorsAndSizes[0].NameColor;
            }

            CurrentProduct = product;
            CurrentVariant = variants[0];


            if (m_ColorsAndSizes.Count > 0)
            {
                InstantiateSizes(true, m_ColorsAndSizes[0].NameColor, 0);
            }
            else
            {
                onlyHat.sprite = hatData.GethatImage();
                productImage.sprite = hatData.GethatImage();
                onlyHat.gameObject.SetActive(true);
                iconMask.SetActive(false);
            }


        }

        private void OnGetHatImageColors()
        {
            if (m_ColorsAndSizes.Count > 0)
            {
                for (int i = 0; i < m_ColorsAndSizes.Count; i++)
                {
                    string s = m_ColorsAndSizes[i].URLImage;
                    StartCoroutine(OnDownloadImage(s,i,(spri,tex, index) =>
                    {
                        colors[index].gameObject.transform.GetChild(0).GetChild(0).transform.GetComponent<Image>().sprite = Utils.CutTexture(tex);
                        m_ColorsAndSizes[index].HatImage = spri;
                    }));
                }
            }

            StartCoroutine(OnCheckFinishImage(m_ColorsAndSizes));
        }

        private void OnGetHatExperience()
        {
            if (m_ColorsAndSizes.Count > 0)
            {
                for (int i = 0; i < m_ColorsAndSizes.Count; i++)
                {
                    string s = m_ColorsAndSizes[i].URLExperience;
                    if (!string.IsNullOrEmpty(s))
                    {
                        StartCoroutine(OnDownloadImage(s, i, (spri, tex, index) =>
                        {
                            m_ColorsAndSizes[index].HatExperience = spri;
                        }));
                    }

                }
            }
        }

        private IEnumerator OnCheckFinishImage(List<ColorsAndSizes> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].HatImage == null)
                    yield return new WaitUntil(() => list[i].HatImage != null);
            }
            finishDownloadImage = true;
        }

       

        private void InstantiateSizes(bool changeImage,string name , int indexHat)
        {

            Debug.Log("aqui tengo que hacer la verificacion");
            StartCoroutine( OnChangeImagePortada(name));

            DeleteSizes();
            StartCoroutine(OnChangeHat(indexHat));

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
        private IEnumerator OnChangeHat(int index)
        {
            yield return new WaitUntil(()=> m_ColorsAndSizes[index].HatImage != null);
            productImage.sprite = m_ColorsAndSizes[index].HatImage;
        }

        private IEnumerator OnChangeImagePortada(string color)
        {
            for (int i = 0; i < m_ColorsAndSizes.Count; i++)
            {
                if (m_ColorsAndSizes[i].NameColor == color)
                {
                    if (!string.IsNullOrEmpty(m_ColorsAndSizes[i].URLExperience))
                    {
                        yield return new WaitUntil(() => m_ColorsAndSizes[i].HatExperience != null);
                        icon.sprite = m_ColorsAndSizes[i].HatExperience;
                        iconMask.SetActive(true);
                        onlyHat.gameObject.SetActive(false);
                    }
                    else
                    {
                        yield return new WaitUntil(() => m_ColorsAndSizes[i].HatImage != null);
                        onlyHat.sprite = m_ColorsAndSizes[i].HatImage;
                        iconMask.SetActive(false);
                        onlyHat.gameObject.SetActive(true);
                    }
                }
            }
        }

        private IEnumerator OnDownloadImage(string myURL,int index, UnityAction<Sprite,Texture2D, int> image)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(myURL);
            yield return www.SendWebRequest();

            Texture2D tex = DownloadHandlerTexture.GetContent(www);
            Sprite spri = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            image?.Invoke(spri, tex, index);
        }

        private void GetColorAndSize(string color, string size)
        {
            string value = color + " / " + size;
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
    
}
