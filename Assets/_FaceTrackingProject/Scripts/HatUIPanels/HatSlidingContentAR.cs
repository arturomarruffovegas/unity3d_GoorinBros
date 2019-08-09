using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using Shopify.Unity;
using goorinAR;
using System.Linq;

public class HatDataD
{
    public string hatId;
    public string hatName;
    public string hatBrand;
    public Sprite hatPhoto;
    public List<string> hatColorList = new List<string>();
    public List<string> hatSizeList = new List<string>();
    public string hatColor;
}


public class HatSlidingContentAR : MonoBehaviour
{
    [Header("Panel Prefab")]
    public GameObject m_ContentHatPanelPrefb;

    //Swipe Movement
    private float m_ListElementOffsetX = 500.0f;
    private int m_CurrentIndexElement = 0;
    private bool m_IsDragging;
    private float m_Time;
    private Vector3 m_FirstPointDrag;

    public HatArController m_HatArController;

    //Test Data
    public Sprite hatPhoto;
    public Sprite hatPhoto2;
    private List<HatDataD> m_HatData;

    public static UnityAction LoadContentGG;
    // Start is called before the first frame update

    [SerializeField]
    private RectTransform UIManager;
    private List<GameObject> m_ListContentHatPanel;
    public float ValueContent;
    public int indexInstruction;
    public bool one;
    Vector2 PositionI;
    Vector2 PositionF;
    private List<Vector2> AllPos = new List<Vector2>();
    [SerializeField]
    private GridLayoutGroup Content;
    [SerializeField]
    private ScrollRect m_ScrollRect;

    public static UnityAction<List<ColorsAndSizes>, Product, ProductVariant> OnSearchHat;

    private InformationPanel m_informationPanel;
    [SerializeField]
    private HatCartPanel HatCartPanel;
    public UnityEvent OnViewCart;
    Button cartButton;
    public static UnityAction<string> OnSetHatName;

   
    public ShowProductEvent OnShowProduct = new ShowProductEvent();

    public Shopify.Unity.Product CurrentProduct;
    public Shopify.Unity.ProductVariant CurrentVariant;

    [SerializeField]
    private List<GameObject> colors = new List<GameObject>();
    [SerializeField]
    private List<GameObject> sizes = new List<GameObject>();
    private List<Product> Hatproducts = new List<Product>();
    [SerializeField]
    private int currentIndexHat;

    [SerializeField]
    private Button backButton;
    private HatData hatData;

    [Header("Panel capturePhoto")]
    public Text hatNameCapturePhoto;

    void Start()
    {
        Content.cellSize = new Vector2( UIManager.sizeDelta.x,Content.cellSize.y);
        GalleryPanel.galleryAR += Instan;
        OnSearchHat += SearchHat;

        m_informationPanel = FindObjectOfType<InformationPanel>();
        hatData = FindObjectOfType<HatData>();
        
        EventTrigger trigger = m_ScrollRect.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();

        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { OnDragDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);


        m_ListContentHatPanel = new List<GameObject>();
        m_ListElementOffsetX = Screen.width;

        backButton.onClick.AddListener(() => 
        {
            AppController.OnBackAR();
            StartCoroutine(ResetValues());
           
        });

        m_HatData = new List<HatDataD>();
        HatDataD d1 = new HatDataD();
        d1.hatId = "welfleet";
        d1.hatName = "Benjamin Paul";
        d1.hatBrand = "Fedora";
        d1.hatPhoto = hatPhoto;
        d1.hatColorList = new List<string>();
        d1.hatSizeList = new List<string> { "s", "m", "l" };
        d1.hatColor = "black";

        HatDataD d2 = new HatDataD();
        d2.hatId = "county line";
        d2.hatName = "County Line";
        d2.hatBrand = "Fedora";
        d2.hatPhoto = hatPhoto2;
        d2.hatColorList = new List<string> { "black", "gray"};
        d2.hatSizeList = new List<string> { "m", "l" };
        d2.hatColor = "black";

        m_HatData.Add(d1);
        m_HatData.Add(d2);

       // LoadContentGG += GG;
    }

    private IEnumerator ResetValues()
    {
        yield return new WaitForSeconds(1f);
        ResetList();
    }
    private void ResetList()
    {
        for (int i = 0; i < colors.Count; i++)
        {
            Destroy(colors[i].gameObject);
        }
        colors.Clear();
        for (int i = 0; i < sizes.Count; i++)
        {
            Destroy(sizes[i].gameObject);
        }
        sizes.Clear();
    }

    private void OnDestroy()
    {
        OnSearchHat -= SearchHat;
        GalleryPanel.galleryAR -= Instan;
    }


    private void SearchHat(List<ColorsAndSizes> colorsAndSizes,Product product,ProductVariant productVariant)
    {
        for (int i = 0; i < m_ListContentHatPanel.Count; i++)
        {
            if (m_ListContentHatPanel[i].GetComponent<HatPanelArPrefab>().m_HatId == product.title())
            {
                Vector2 size = Content.GetComponent<RectTransform>().sizeDelta;
                float Value = size.x / m_ListContentHatPanel.Count;
                float Pos = Value * i;
                float V = (Value / 2);
                float S = (Pos + V);

                ValueContent = (S);
                Content.GetComponent<RectTransform>().DOLocalMoveX(-ValueContent, 0.01f);
                indexInstruction = i;
                currentIndexHat = i;
                one = true;

                CurrentProduct = product;
                OnSetHatName(product.title().ToLower());
                CurrentVariant = productVariant;

                hatNameCapturePhoto.text = product.title();

                if (colorsAndSizes != null )
                {

                    for (int j = 0; j < colorsAndSizes.Count; j++)
                    {
                        Transform parent = m_ListContentHatPanel[i].GetComponent<HatPanelArPrefab>().m_ColorOptionPrefab.transform.parent;
                        GameObject colorButton = m_ListContentHatPanel[i].GetComponent<HatPanelArPrefab>().m_ColorOptionPrefab;
                        GameObject sizePrefabButton = m_ListContentHatPanel[i].GetComponent<HatPanelArPrefab>().m_SizeOptionPrefab;
                        cartButton = m_ListContentHatPanel[i].GetComponent<HatPanelArPrefab>().cart;
                        Sprite IconColor = colorsAndSizes[j].HatImage;
                        UnityEngine.UI.Image hatPhoto = m_ListContentHatPanel[i].GetComponent<HatPanelArPrefab>().m_HatPhoto;
                        var objC = Instantiate(colorButton, parent);

                        NewMethod(IconColor, objC);

                        objC.name = colorsAndSizes[j].NameColor;
                        int indexHat = j;
                        objC.SetActive(true);
                        colors.Add(objC);
                        objC.GetComponent<Button>().onClick.RemoveAllListeners();
                        objC.GetComponent<Button>().onClick.AddListener(delegate
                        {
                            InstantiateSizes(sizePrefabButton, hatPhoto, colorsAndSizes,
                                             objC.name, indexHat);
                        });

                        InstantiateSizes(sizePrefabButton, hatPhoto, colorsAndSizes, colorsAndSizes[0].NameColor, 0);

                        if (colorsAndSizes.Count <=1)
                        {
                            objC.SetActive(false);
                        }


                    }

                    if (colorsAndSizes.Count>0)
                        m_ListContentHatPanel[i].GetComponent<HatPanelArPrefab>().m_HatPhoto.sprite = colorsAndSizes[0].HatImage;

                }

                if (cartButton != null)
                {
                    cartButton.interactable = true;
                    cartButton.onClick.RemoveAllListeners();
                    cartButton.onClick.AddListener(() =>
                    {
                        m_informationPanel.OnAddProductToCart.Invoke(CurrentProduct, CurrentVariant);
                        OnViewCart.Invoke();
                        AppController.OnShoopingCartPanelAR();

                    });
                }
                else
                {
                    cartButton.interactable = false;
                }

            }
        }
    }

    private static void NewMethod(Sprite IconColor, GameObject objC)
    {
        objC.transform.GetChild(0).GetChild(0).transform.GetComponent<UnityEngine.UI.Image>().sprite = Utils.CutTexture(IconColor.texture);
    }

    private void InstantiateSizes(GameObject sizePrefab, UnityEngine.UI.Image image ,
                                  List<ColorsAndSizes> colorsAndSizes,string name, 
                                  int indexhat)
    {
        DeleteSizes(sizePrefab);

        image.sprite = colorsAndSizes[indexhat].HatImage;

        foreach (var item in colors)
            item.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().enabled = false;

        for (int i = 0; i < colorsAndSizes.Count; i++)
        {
            if (name == colorsAndSizes[i].NameColor)
            {
                colors[i].gameObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().enabled = true;

                for (int j = 0; j < colorsAndSizes[i].sizes.Count; j++)
                {
                    var size = Instantiate(sizePrefab, sizePrefab.transform.parent);
                    size.name = colorsAndSizes[i].sizes[j];
                    sizes.Add(size);
                    var nameSize = size.name;
                    char s = nameSize.FirstOrDefault();
                    string[] characters = nameSize.Split('-');
                    string theName = s.ToString();

                    if (characters.Length > 1)
                        theName = characters[0] + characters[1].FirstOrDefault().ToString();

                    size.transform.GetChild(0).GetComponent<Text>().text = theName;

                    size.GetComponent<Button>().onClick.RemoveAllListeners();
                    size.GetComponent<Button>().onClick.AddListener(delegate{ GetColorAndSize(name, size.name); });
                    size.SetActive(true);
                    GetColorAndSize(name, colorsAndSizes[i].sizes[0]);
                }
            }
        }
    }

    private void GetColorAndSize(string color, string size)
    {
        string value = color + " / " + size;
        var variants = (List<Shopify.Unity.ProductVariant>)CurrentProduct.variants();
        foreach (var item in variants)
        {
            if (item.title() == value)
                CurrentVariant = item;
        }
        
        cartButton.interactable = true;
        foreach (var item in sizes)
            item.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().enabled = false;

        for (int i = 0; i < sizes.Count; i++)
        {
            if (size == sizes[i].name)
                sizes[i].gameObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().enabled = true;
        }
    }

    private void DeleteSizes(GameObject Sizeparent)
    {
        int amount = Sizeparent.transform.parent.childCount;
        if (amount > 1)
        {
            for (int j = amount - 1; j > 0; j--)
            {
                Destroy(Sizeparent.transform.parent.GetChild(j).gameObject);
            }
        }
        sizes.Clear();
    }

    public void Instan(Product p)
    {
        HatDataD _hatData = new HatDataD();
        _hatData.hatId = p.title();
        _hatData.hatName = p.title();
        var variants = (List<Shopify.Unity.ProductVariant>)p.variants();
        
        GameObject element = Instantiate(m_ContentHatPanelPrefb, transform);
        element.SetActive(true);
        element.GetComponent<HatPanelArPrefab>().LoadInformation(_hatData.hatId,
                                                                     _hatData.hatName,
                                                                     _hatData.hatBrand,
                                                                     _hatData.hatPhoto,
                                                                     _hatData.hatColorList,
                                                                     _hatData.hatSizeList,
                                                                     _hatData.hatColor);

        var List_color_code_map = p.tags();
        string colorDefault = "";
        string sku = "";
        colorDefault = Utils.GetColorDefault(List_color_code_map);
        sku = Utils.GetSKU(List_color_code_map);

        var img = p.images();

        foreach (Shopify.Unity.ImageEdge item in img.edges())
        {
            string URLglobal = item.node().transformedSrc("large");

            if (colorDefault != "" && sku != "")
            {
                if (URLglobal.Contains(sku + "-" + colorDefault + "-F01"))
                {
                    string _URLImage = URLglobal;
                    StartCoroutine(Utils.OnDownloadImage(_URLImage, (spri) =>
                    {
                        element.GetComponent<HatPanelArPrefab>().m_HatPhoto.sprite = spri;
                    }));
                }

            }
        }

        m_ListContentHatPanel.Add(element);
        Hatproducts.Add(p);
    }

    private void OnEvent(Product product, GameObject element)
    {
        ResetList();

        sizes.Clear();
        hatData.SethatName(product.title());
        hatData.SethatImage(element.GetComponent<HatPanelArPrefab>().m_HatPhoto.sprite);
        OnShowProduct.Invoke(product);
        StartCoroutine(WaitUntilDownloadImage());

    }

    private IEnumerator WaitUntilDownloadImage()
    {
        yield return new WaitUntil(()=>m_informationPanel.finishDownloadImage ==true);
        m_informationPanel.OnActionTry();
    }

    private void OnDragDelegate(PointerEventData data)
    {
        AllPos.Add(data.position);
    }

    public void EndDragDelegate()
    {
        PositionI = AllPos[0];
        PositionF = AllPos[AllPos.Count - 1];
        float Dis = PositionI.x - PositionF.x;
        if (Dis > 100)
        {
             Debug.Log("<color=blue> Izquierda </color>");
            if (indexInstruction < m_ListContentHatPanel.Count - 1)
            {
                Vector2 size = Content.GetComponent<RectTransform>().sizeDelta;
                float Value = size.x / m_ListContentHatPanel.Count;
                float V = Value / 2;
                if (one == false)
                {
                    float S = Value + V;
                    ValueContent += S;
                    one = true;
                }
                else
                {
                    ValueContent += Value;
                }
                Content.GetComponent<RectTransform>().DOLocalMoveX(-ValueContent, 0.2f);
                indexInstruction++;
                currentIndexHat = indexInstruction;

                StopAllCoroutines();
                OnEvent(Hatproducts[currentIndexHat], m_ListContentHatPanel[currentIndexHat]);
            }
        }
        else if (Dis < -100)
        {
            Debug.Log("<color=blue> Derecha </color>");
            if (indexInstruction > 0)
            {
                Vector2 size = Content.GetComponent<RectTransform>().sizeDelta;
                float Value = size.x / m_ListContentHatPanel.Count;
                float V = Value / 2;

                ValueContent -= Value;
                Content.GetComponent<RectTransform>().DOLocalMoveX(-ValueContent, 0.2f);
                indexInstruction--;
                currentIndexHat = indexInstruction;

                StopAllCoroutines();
                OnEvent(Hatproducts[currentIndexHat], m_ListContentHatPanel[currentIndexHat]);
            }
        }

        AllPos.Clear();
    }



    //void GG()
    //{

    //    //SwipeMovement(-1.0f);
    //    //List<GameObject> listContentHatPanel = new List<GameObject>();
    //    for (int i = 0; i < m_ListContentHatPanel.Count; i++)
    //    {
    //        Destroy(m_ListContentHatPanel[i].gameObject);
    //    }
    //    m_ListContentHatPanel.Clear();
    //    m_HatArController.m_HatLoadingPanel.SetActive(true);
    //    LoadContent(m_HatData);

    //}

    //private void OnDestroy()
    //{
    //    LoadContentGG -= GG;
    //    for(int i = 0; i < m_ListContentHatPanel.Count; i++)
    //    {
    //        Destroy(m_ListContentHatPanel[i].gameObject);
    //    }
    //    m_ListContentHatPanel.Clear();
    //}

    //public void OnDragPoint()
    //{
    //    m_IsDragging = true;
    //    m_FirstPointDrag = Input.mousePosition;
    //}

    //public void StopDragPoint()
    //{
    //    m_IsDragging = false;
    //    float diference = Input.mousePosition.x - m_FirstPointDrag.x;
    //    m_HatArController.m_HatLoadingPanel.SetActive(true);
    //    Debug.Log(diference);
    //    SwipeMovement(diference);
    //}

    //public void LoadContent(List<HatData> listHatData)
    //{
    //    //for(int i = 0; i < listContentHatPanel.Count; i++)
    //    for (int i = 0; i < listHatData.Count; i++)
    //    {
    //        GameObject element = (GameObject)Instantiate(m_ContentHatPanelPrefb, transform);
    //        element.SetActive(true);
    //        //element.transform.parent = transform;            
    //        element.transform.position += new Vector3(m_ListElementOffsetX * i, 0, 0);
    //        element.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

    //        //Load data from list (Image, description, etc)
    //        //element.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(ChangeHat);
    //        //element.GetComponent<HatPanelArPrefab>().LoadInformation("hats_benjaminPaul", "hats_benjaminPaul",
    //        //                                         "Fedora", hatPhoto, new string[] { "1", "2", "3" }, new string[] { "s", "m", "l" });

    //        element.GetComponent<HatPanelArPrefab>().LoadInformation(listHatData[i].hatId,
    //                                                                 listHatData[i].hatName,
    //                                                                 listHatData[i].hatBrand,
    //                                                                 listHatData[i].hatPhoto,
    //                                                                 listHatData[i].hatColorList,
    //                                                                 listHatData[i].hatSizeList,
    //                                                                 listHatData[i].hatColor);

    //        m_ListContentHatPanel.Add(element);
    //    }

    //    //Load first element / Load current element 
    //    m_HatArController.LoadHatBundle(listHatData[m_CurrentIndexElement].hatId, listHatData[m_CurrentIndexElement].hatColor);
    //}

    //public void ChangeHat()
    //{
    //    //m_HatArController.LoadHat("Hat" + (m_CurrentIndexElement + 1));
    //    m_HatArController.LoadHatBundle(m_HatData[m_CurrentIndexElement].hatId, m_HatData[m_CurrentIndexElement].hatColor);
    //}

    //public void SwipeMovement(float swipeDirection)
    //{
    //    float directioValue = (swipeDirection > 0 ? 1.0f : -1.0f);

    //    if((m_CurrentIndexElement != 0 && directioValue > 0) ||
    //       (m_CurrentIndexElement < m_ListContentHatPanel.Count - 1 && directioValue < 0))
    //    {
    //        m_CurrentIndexElement -= (int)directioValue;

    //        for (int i = 0; i < m_ListContentHatPanel.Count; i++)
    //        {
    //            float v = m_ListElementOffsetX * 0.15f;
    //            float endValue = m_ListContentHatPanel[i].transform.position.x + (m_ListElementOffsetX - v) * directioValue;
    //            m_ListContentHatPanel[i].transform.DOMoveX(endValue, 0.3f);
    //        }
    //    }

    //    //ChangeHat();
    //}
}
