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
        [SerializeField]
        private GameObject sizesPanel;

        [Header("Zoom hat")]
        [SerializeField]
        private Button hatButton;
        [SerializeField]
        private Button backZoomButton;
        [SerializeField]
        private GameObject hatZoomPanel;

        [Header("Animations")]
        [SerializeField]
        private Button ViewDetailButton;
        [SerializeField]
        private GameObject empty;
        [SerializeField]
        private ScrollRect scrollInfomation;
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
        private Button referienceButton;
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
        private List<GameObject> referiences = new List<GameObject>();
       // public List<Sprite> m_hatImages = new List<Sprite>();

        private bool isEnabled;

        public Shopify.Unity.Product CurrentProduct;
        public Shopify.Unity.ProductVariant CurrentVariant;


        [Header("Informations")]
        [SerializeField]
        private Text productTitle;
        [SerializeField]
        private Image productImage;
        //[SerializeField]
        //private Text brimGruop;
        //[SerializeField]
        //private Text brimGruopTitle;
        //[SerializeField]
        //private Text crown;
        //[SerializeField]
        //private Text crownTitle;




        [SerializeField]
        private Transform contentInfoBasic;
        [SerializeField]
        private GameObject infoBasicPanel;
        [SerializeField]
        private GameObject line;

        private List<GameObject> infoB = new List<GameObject>();
        private List<GameObject> lines = new List<GameObject>();

        [SerializeField]
        private Text specifications;
        [SerializeField]
        private Text materials;

        [SerializeField]
        private Info_JSON info_JSON; 




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

            ViewDetailButton.onClick.AddListener(EnableInfo);
            hatButton.onClick.AddListener(() => 
            {
                hatZoomPanel.SetActive(true);
                Sprite sp = hatButton.transform.GetChild(0).GetComponent<Image>().sprite;

                FindObjectOfType<ZoomAndRotateController>().SetTexture(sp);

            });

            backZoomButton.onClick.AddListener(() => 
            {
                FindObjectOfType<ZoomAndRotateController>().OnReset();
                hatZoomPanel.SetActive(false);
            });

            backPanelButton.onClick.AddListener(() =>
            {
               // ResetValue();
                OnReturnToProducts.Invoke();
                ResetInfoAnimation();

                
            });

            addToCartButton.onClick.AddListener(() => {
                OnAddProductToCart.Invoke(CurrentProduct, CurrentVariant);
                OnViewCart.Invoke();
            });

            tryButton.onClick.AddListener(OnActionTry);

            sizingGuideButton.onClick.AddListener(() => 
            {
                sizesPanel.SetActive(true);
                sizesPanel.transform.GetChild(0).GetComponent<Image>().DOFade(1, 2f);
            });
        }

        public void OnBackSizesPanel()
        {
            sizesPanel.SetActive(false);
            sizesPanel.transform.GetChild(0).GetComponent<Image>().DOFade(0,0.01f);
        }


        public void OnActionTry()
        {
            OnTryProduct.Invoke();

            if (product != null)
            {
                HatSlidingContentAR.OnSearchHat(m_ColorsAndSizes, product, CurrentVariant);
                FirebaseController.stopDownload = false;
            }
        }

        private void ResetInfoAnimation()
        {

            scrollInfomation.verticalNormalizedPosition = 1;

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
            DeleteReferience();

            iconMask.SetActive(false);
            onlyHat.gameObject.SetActive(false);
            productImage.sprite = waitIcon;

            specifications.text = "";
            materials.text = "";

            ResetInfoBasic();

        }

        private void ResetInfoBasic()
        {
            //int amountInfoBasic = infoBasicPanel.gameObject.transform.parent.childCount;
            //if (amountInfoBasic > 2)
            //{
            //    for (int i = amountInfoBasic - 2; i > 0; i--)
            //    {
            //        Destroy(infoBasicPanel.gameObject.transform.parent.GetChild(i).gameObject);
            //    }
            //}
            for (int i = 0; i < infoB.Count; i++)
            {
                Destroy(infoB[i].gameObject);
            }

            for (int i = 0; i < lines.Count; i++)
            {
                Destroy(lines[i].gameObject);
            }

            infoB.Clear();
            lines.Clear();
        }

        public void SetCurrentProduct(Shopify.Unity.Product product)
        {
            if(hatData.GethatName() == hatName)
            {
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


            var variants = (List<Shopify.Unity.ProductVariant>)product.variants();

            string metafield = variants[0].metafields().edges()[0].node().value();

            var splitData = metafield.Split('\v');
            string data = "";
            for (int i = 0; i < splitData.Length; i++)
            {
                data = data + splitData[i];
            }

            info_JSON = JsonUtility.FromJson<Info_JSON>(data);

            //specifications
            for (int i = 0; i < info_JSON.specs_variant.Count; i++)
            {
                string title = "<b>" +info_JSON.specs_variant[i].title + "</b>" + " : ";

                string text = info_JSON.specs_variant[i].text;
                text = text.Replace("&#34", "''");
                specifications.text = specifications.text + title + text + "\n";
            }

            //materials
            for (int i = 0; i < info_JSON.materials.Count; i++)
            {
                string title = "<b>" + info_JSON.materials[i].title + "</b>" + " : ";
                string text = info_JSON.materials[i].text;

                materials.text = materials.text + title + text + "\n";
            }

            //information basic
            for (int i = 0; i < info_JSON.summary_box.Count; i++)
            {
                string title =  info_JSON.summary_box[i].title ;
                string text = "<b>" + info_JSON.summary_box[i].text + "</b>";

                var info = Instantiate(infoBasicPanel, contentInfoBasic);
                info.SetActive(true);
                info.GetComponent<InfoBasic>().title.text = title;
                info.GetComponent<InfoBasic>().text.text = text;

                if(i< info_JSON.summary_box.Count-1)
                {
                    var lin = Instantiate(line, contentInfoBasic);
                    lin.SetActive(true);
                    lines.Add(lin);
                }
                infoB.Add(info);

            }




         //   Utils.GetBrimGruop(product, brimGruop , brimGruopTitle);
          // Utils.GetCrownShape(product, crown, crownTitle);

            // productImage.sprite = hatData.GethatImage();

            var options = new List<string>();

            var List_color_code_map = product.tags();

            var img = product.images();

            foreach (var variant in variants)
            {
                if (variant.availableForSale())
                {

                    string name = variant.title();
                    string[] names = name.Split('/');

                    var NameColor = names[0].Trim();
                    var NameSize = names[1].Trim();

                    if (m_ColorsAndSizes.Count > 0)
                    {

                        if (SearchColorName(m_ColorsAndSizes, NameColor) == false)
                        {
                            ColorsAndSizes d = new ColorsAndSizes();
                            d.NameColor = NameColor;
                            d.sku = Utils.GetSKU(List_color_code_map);
                            string tagColor = "color_code_map:" + NameColor;
                            d.color_code_map = Utils.GetColorCodeMap(List_color_code_map, tagColor);

                            List<ReferienceHats> m_ReferienceHats = new List<ReferienceHats>();

                            string _URLImage = "";
                            string _URLExperienceImage = "";
                            foreach (Shopify.Unity.ImageEdge item in img.edges())
                            {
                                string URLglobal = item.node().transformedSrc("large");
                                string URLExperienceglobal = item.node().transformedSrc("grande");
                                if (d.sku != "" && d.color_code_map != "")
                                {
                                    string text = d.sku + "-" + d.color_code_map;
                                    if (URLglobal.Contains(text + "-F01"))
                                    {
                                        _URLImage = URLglobal;
                                    }

                                    //if(SearchReferienceceName(rh, URLglobal) == false)
                                    //{

                                    //}
                                    SearchTypeViewHat(m_ReferienceHats, URLglobal, text);

                                    //ordenar la lista(vistas del sombrero)
                                    m_ReferienceHats = Utils.onSortReferienceList(m_ReferienceHats);

                                    string v = d.sku + "-" + d.color_code_map;

                                    if (URLExperienceglobal.Contains(v + "-HF01"))
                                        _URLExperienceImage = URLExperienceglobal;
                                    else if (URLExperienceglobal.Contains(v + "-HM01"))
                                        _URLExperienceImage = URLExperienceglobal;
                                    else if (URLExperienceglobal.Contains(v + "-MF01"))
                                        _URLExperienceImage = URLExperienceglobal;

                                }

                            }

                            d.URLImage = _URLImage;
                            d.URLExperience = _URLExperienceImage;
                            d.sizes.Add(NameSize);

                            d.ReferenceHats = m_ReferienceHats;

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

                        List<ReferienceHats> m_ReferienceHats = new List<ReferienceHats>();

                        string _URLImage = "";
                        string _URLExperienceImage = "";
                        foreach (Shopify.Unity.ImageEdge item in img.edges())
                        {
                            string URLglobal = item.node().transformedSrc("resolution_1024");
                            string URLExperienceglobal = item.node().transformedSrc("grande");

                            if (d.sku != "" && d.color_code_map != "")
                            {
                                string text = d.sku + "-" + d.color_code_map;
                                if (URLglobal.Contains(text + "-F01"))
                                {
                                    _URLImage = URLglobal;
                                }

                                //if (SearchReferienceceName(rh, URLglobal) == false)
                                //{

                                //}
                                SearchTypeViewHat(m_ReferienceHats, URLglobal, text);

                                //ordenar la lista(vistas del sombrero)
                                m_ReferienceHats = Utils.onSortReferienceList(m_ReferienceHats);

                                string v = d.sku + "-" + d.color_code_map;

                                if (URLExperienceglobal.Contains(v + "-HF01"))
                                    _URLExperienceImage = URLExperienceglobal;
                                else if (URLExperienceglobal.Contains(v + "-HM01"))
                                    _URLExperienceImage = URLExperienceglobal;
                                else if (URLExperienceglobal.Contains(v + "-MF01"))
                                    _URLExperienceImage = URLExperienceglobal;
                            }



                        }

                      

                        d.URLImage = _URLImage;
                        d.URLExperience = _URLExperienceImage;
                        d.sizes.Add(NameSize);

                        d.ReferenceHats = m_ReferienceHats;

                        m_ColorsAndSizes.Add(d);
                        
                    }

                    options.Add(variant.title());
                }
                
            }

            //if (m_ColorsAndSizes.Count > 1)
            //    tryButton.interactable = true;
            //else
            //    tryButton.interactable = false;

             //GetImages
            OnGetHatImageColors();
            OnGetHatReferience();
            OnGetHatExperience();

            //instantiate colors
            if (m_ColorsAndSizes.Count <= 0)
            {
                addToCartButton.interactable = false;
            }
            for (int i = 0; i < m_ColorsAndSizes.Count; i++)
            {
                var colorLocalButton = Instantiate(colorButton.gameObject, colorButton.gameObject.transform.parent);
                colorLocalButton.name = m_ColorsAndSizes[i].NameColor;
                int indexHat = i;
                colors.Add(colorLocalButton);
                colorLocalButton.GetComponent<Button>().onClick.AddListener(delegate { InstantiateSizes(true,colorLocalButton.name, indexHat);});
                colorLocalButton.SetActive(true);
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
                    StartCoroutine(OnDownloadImage(s,i,0,(spri,tex, index, indexchild) =>
                    {
                        colors[index].gameObject.transform.GetChild(0).GetChild(0).transform.GetComponent<Image>().sprite = Utils.CutTexture(tex);
                        m_ColorsAndSizes[index].HatImage = spri;
                    }));
                }
            }

            StartCoroutine(OnCheckFinishImage(m_ColorsAndSizes));
        }

        private void OnGetHatReferience()
        {
            if (m_ColorsAndSizes.Count > 0)
            {
                for (int i = 0; i < m_ColorsAndSizes.Count; i++)
                {
                    for (int j = 0; j < m_ColorsAndSizes[i].ReferenceHats.Count; j++)
                    {
                        string s = m_ColorsAndSizes[i].ReferenceHats[j].URL;

                        if (!string.IsNullOrEmpty(s))
                        {
                            StartCoroutine(OnDownloadImage(s, i, j, (spri, tex, index, indexChild) =>
                            {
                                m_ColorsAndSizes[index].ReferenceHats[indexChild].Image = spri;
                            }));
                        }
                    }
                }
            }
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
                        StartCoroutine(OnDownloadImage(s, i,0, (spri, tex, index, indexChild) =>
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

            StartCoroutine( OnChangeImagePortada(name));

            DeleteSizes();
            DeleteReferience();
            StartCoroutine(OnChangeHat(indexHat));

            foreach (var item in colors)
                item.transform.GetChild(1).GetComponent<Image>().enabled = false;

            for (int i = 0; i < m_ColorsAndSizes.Count; i++)
            {
                if (name == m_ColorsAndSizes[i].NameColor)
                {
                    colors[i].gameObject.transform.GetChild(1).GetComponent<Image>().enabled = true;

                    //Instanciar botones de medidas

                    List<string> localSize = Utils.OnSortSizeList(m_ColorsAndSizes[i].sizes);

                    for (int j  = 0; j < localSize.Count ; j++)
                    {
                        var size = Instantiate(SizeButton.gameObject, SizeButton.gameObject.transform.parent);
                        size.name = localSize[j];
                        sizes.Add(size);

                        var nameSize = size.name;
                        char s = nameSize.FirstOrDefault();
                        string[] characters = nameSize.Split('-');
                        string theName = s.ToString();

                        if(size.name == "One Size")
                        {
                            string[] charac = nameSize.Split(' ');
                            if(charac.Length > 1)
                                theName = charac[0].FirstOrDefault().ToString() + charac[1].FirstOrDefault().ToString();

                        }


                        if (characters.Length > 1)
                            theName = characters[0] + characters[1].FirstOrDefault().ToString();

                        
                        size.transform.GetChild(0).GetComponent<Text>().text = theName;

                        size.GetComponent<Button>().onClick.AddListener(delegate { GetColorAndSize(name, size.name); });
                        size.SetActive(true);
                        GetColorAndSize(name, m_ColorsAndSizes[i].sizes[0]);
                    }

                    //instanciar botones de cambio de sombrero
                    for (int r = 0; r < m_ColorsAndSizes[i].ReferenceHats.Count; r++)
                    {
                        var referience = Instantiate(referienceButton.gameObject, referienceButton.gameObject.transform.parent);
                        referience.GetComponent<Button>().onClick.RemoveAllListeners();
                        referience.GetComponent<Button>().onClick.AddListener(delegate { OnChangeImagehatWithView(referience); });
                        referiences.Add(referience);
                        referience.SetActive(true);
                    }

                    StartCoroutine(OnChangeViewHats(i));
                    
                }
               
            }
        }

        private void OnChangeImagehatWithView(GameObject obj)
        {
            productImage.sprite = obj.transform.GetChild(0).GetComponent<Image>().sprite;
        }

        private IEnumerator OnChangeViewHats(int index)
        {
            for (int i = 0; i < m_ColorsAndSizes[index].ReferenceHats.Count; i++)
            {
                yield return new WaitUntil(() => m_ColorsAndSizes[index].ReferenceHats[i].Image != null);
                referiences[i].transform.GetChild(0).transform.GetComponent<Image>().sprite = m_ColorsAndSizes[index].ReferenceHats[i].Image;
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
                        //icon.sprite = m_ColorsAndSizes[i].HatExperience;
                        icon.sprite = Utils.CutTextureIcon(m_ColorsAndSizes[i].HatExperience.texture);
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

        private IEnumerator OnDownloadImage(string myURL,int index, int IndexChild, UnityAction<Sprite,Texture2D, int, int> image)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(myURL);
            yield return www.SendWebRequest();

            Texture2D tex = DownloadHandlerTexture.GetContent(www);
            Sprite spri = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            image?.Invoke(spri, tex, index, IndexChild);
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

        private void DeleteReferience()
        {
            int amount = referienceButton.gameObject.transform.parent.childCount;
            if (amount > 1)
            {
                for (int j = amount - 1; j > 0; j--)
                {
                    Destroy(referienceButton.gameObject.transform.parent.GetChild(j).gameObject);
                }
            }
            referiences.Clear();
        }

        private bool SearchColorName(List<ColorsAndSizes> list, string name)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].NameColor == name)
                    return true;
            }

            return false;
        }

        

        private void SearchTypeViewHat(List<ReferienceHats> rh, string URLglobal, string text)
        {

            if (URLglobal.Contains(text + "-F01"))
            {
                ReferienceHats s = new ReferienceHats
                {
                    name = "Front",
                    URL = URLglobal
                };

                if(SearchReferienceceName(rh,text + "-F01")==false)
                    rh.Add(s);
            }
            else if (URLglobal.Contains(text + "-S01"))
            {
                ReferienceHats s = new ReferienceHats
                {
                    name = "Left",
                    URL = URLglobal
                };
                if (SearchReferienceceName(rh, text + "-S01") == false)
                    rh.Add(s);
            }
            else if (URLglobal.Contains(text + "-B01"))
            {
                ReferienceHats s = new ReferienceHats
                {
                    name = "Back",
                    URL = URLglobal
                };
                if (SearchReferienceceName(rh, text + "-B01") == false)
                    rh.Add(s);
            }
            else if (URLglobal.Contains(text + "-T01"))
            {
                ReferienceHats s = new ReferienceHats
                {
                    name = "Top",
                    URL = URLglobal
                };
                if (SearchReferienceceName(rh, text + "-T01") == false)
                    rh.Add(s);
            }
            else if (URLglobal.Contains(text + "-U01"))
            {
                ReferienceHats s = new ReferienceHats
                {
                    name = "Under",
                    URL = URLglobal
                };
                if (SearchReferienceceName(rh, text + "-U01") == false)
                    rh.Add(s);
            }


        }

        private bool SearchReferienceceName(List<ReferienceHats> list, string name)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].URL.Contains(name))
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
