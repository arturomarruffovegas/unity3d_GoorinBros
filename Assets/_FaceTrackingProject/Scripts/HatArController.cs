using goorinAR;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum materialLayer
{
    AmbientOcclusion,
    Diffuse,
    Metallic,
    Normal,
    Roughness,
    Shading
}

public class HatArController : MonoBehaviour
{
    private Dictionary<string, GameObject> m_Hat3DList;
    public Transform m_HatPosition;
    public GameObject m_CurrentHat;
    public HatContent m_CurrentHatContent;
    private string m_CurrentHatId;
    private string m_CurrentHatColor;

    private string m_CurrentId;

    [Header("CapturePhoto")]
    public List<Camera> m_HatCameras;
    public Image m_PhotoCapture;

    [Header("Buttons")]
    public Button m_PhotoButton; // si
    public Button m_VideoButton;
    public Button m_SavePhotoButton;
    public Button m_SharePhotoButton;
    public Button m_PhotoPanelBackButton;
    public Button m_PhotoPanelButton;
    public Button m_cicleButton;

    [Header("panels")]
    public GameObject m_PhotoPanel; //si
    public GameObject m_panels; //si
    public GameObject m_PanelInfoCapturePhoto;

    public static string FILE_SCREENSHOT_NAME = System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
    private Texture2D m_PhotoCaptureTexture;

    [Header("FireBase")]
    public LoadAssetBundlesFromFirebase m_FireBaseLoader;

    [Header("Messages")]
    public GameObject m_HatErrorPanel;
    public GameObject m_HatLoadingPanel;

    //[Header("Head Movement")]
    //public GameObject m_Anchor3DHead;
    //public GameObject m_Anchor3D;
    //public float m_Anchor3DYRotation;
    //public float m_Anchor3DHeadXScale;
    //public float m_Anchor3DHeadYScale;

    [Header("Cart")]
    public AddProductToCartEvent OnAddProductToCart = new AddProductToCartEvent();


    // Start is called before the first frame update
    void Start()
    {
        ScreenshotCapturer.SetCameras(m_HatCameras[0], m_HatCameras[1]);
        m_PhotoButton.onClick.AddListener(ScreenshotButtonPressed);
        m_VideoButton.onClick.AddListener(VideoCaptureButtonPressed);
        m_SavePhotoButton.onClick.AddListener(SaveCapturePhoto);
        m_SharePhotoButton.onClick.AddListener(ShareCapturePhoto);
        m_PhotoPanelBackButton.onClick.AddListener(DeactivePhotoPanel);
        m_PhotoPanelButton.onClick.AddListener(DeactivePhotoPanel);
        m_cicleButton.onClick.AddListener(DeactivePhotoPanel);

        HatSlidingContentAR.OnSetHatName += LoadHatAssetBundle;
        FirebaseController.OnErrorAssetBundle += OnErrorAssetBundle;
        LoadAssetBundlesFromFirebase.FinishProccess += InstantiateHat;

    }

    private void OnDestroy()
    {
        HatSlidingContentAR.OnSetHatName -= LoadHatAssetBundle;
        FirebaseController.OnErrorAssetBundle -= OnErrorAssetBundle;
        LoadAssetBundlesFromFirebase.FinishProccess -= InstantiateHat;
    }

    // Update is called once per frame
    void Update()
    {
        //m_Anchor3DYRotation = m_Anchor3D.transform.eulerAngles.y;
        //m_Anchor3DHeadXScale = m_Anchor3DHead.transform.localScale.x;
        //m_Anchor3DHeadYScale = m_Anchor3DHead.transform.localScale.y;
    }

    public void LoadHat(List<string> hatIdList)
    {
        
    }

    //Load Texture From Bundle Local/Online
    public void LoadHatBundle(string hatId, string hatColor)
    {
        Debug.Log(hatId);
        m_CurrentHatId = hatId;
        m_CurrentHatColor = hatColor;
        
        if (m_CurrentHat)
        {
            Destroy(m_CurrentHat.gameObject);
        }
        
        m_CurrentHat = null;

        m_FireBaseLoader.QueryFirebase(hatId);

        //InstantiateHat(null, null);
    }

    private void LoadHatAssetBundle(string name)
    {
        m_CurrentHatId = name;
        if (m_CurrentHat)
        {
            Destroy(m_CurrentHat.gameObject);
        }

        m_CurrentHat = null;
        m_FireBaseLoader.QueryFirebase(name);
        Debug.Log("<color=blue> descargando " + name + "</color>");
        m_HatLoadingPanel.SetActive(true);
        m_HatErrorPanel.SetActive(false);
    }

    private void OnErrorAssetBundle()
    {
        m_HatLoadingPanel.SetActive(false);
        m_HatErrorPanel.SetActive(true);
    }


    public void InstantiateHat(Object s, Object c)
    {
        Debug.Log("<color=blue> descargo </color>");
        m_HatLoadingPanel.SetActive(false);
        m_HatErrorPanel.SetActive(false);
        //m_CurrentHat = (GameObject)Instantiate(Resources.Load(m_CurrentHatId + "/" + m_CurrentHatId));
        m_CurrentHat = (GameObject)Instantiate((GameObject)s);
        //m_CurrentHatContent  = Resources.Load<HatContent>(m_CurrentHatId + "/" + m_CurrentHatId + "_content") as HatContent;
        m_CurrentHatContent = (HatContent)c;

        m_CurrentId = m_CurrentHatId;
        m_HatPosition = FindObjectOfType<FaceTrackerController2>().m_hatPosition;
        m_CurrentHat.transform.parent = m_HatPosition;
        m_CurrentHat.transform.localPosition = Vector3.zero;
        m_CurrentHat.transform.localRotation = Quaternion.identity;
        m_CurrentHat.transform.localScale = Vector3.one;

       // LoadMaterialsforBundle(m_CurrentHat, m_CurrentHatContent, m_CurrentId, m_CurrentHatColor);

        //Loading Panel
       // m_HatLoadingPanel.SetActive(false);
    }

    public void LoadTextureforBundle(GameObject hatbundle, string hatId, string hatColor)
    {
        Renderer[] rendererChilds = hatbundle.GetComponentsInChildren<Renderer>();
        //List<Material> hatMaterials = new List<Material>();
        for (int i = 0; i < rendererChilds.Length; i++)
        {
            //Get Textures using material name, and id
            //--
            //material: hats_benjaminPaul_Hats_shading_black
            //textures: hats_benjaminPaul_Hats_AO_black, hats_benjaminPaul_Hats_difuse_black, etc
            List<Texture> hatTextures = LoadTextures(hatId, rendererChilds[i].materials[0].name, hatColor);

            //Find layer using material name and layername
            for (int j = 0; j < hatTextures.Count; j++)
            {
                VerifyLayer(rendererChilds[i].materials[0], hatTextures[j].name, hatTextures[j]);
            }
        }       
    }

    public void LoadMaterialsforBundle(GameObject hatbundle, HatContent hatContent, string hatId, string hatColor)
    {
        Debug.Log(hatColor);
        Renderer[] rendererChilds = hatbundle.GetComponentsInChildren<Renderer>();

        for (int j = 0; j < rendererChilds.Length; j++)
        {
            for (int i = 0; i < hatContent.hatMaterials[0].hatMaterials.Count; i++)
            {
                rendererChilds[i].material = hatContent.hatMaterials[0].hatMaterials[i];
            }
        }

    }

    public List<Texture> LoadTextures(string hatId, string hatMaterialName, string hatColor = "black")
    {
        //Download textures from firebase
        //---
        List<Texture> hatTextures = new List<Texture>();

        string hatName = hatId;
        //Local Mode
        
        int index0 = hatMaterialName.IndexOf("hats_");
        //hats_benjaminPaul_Hats_shading_black
        string subName = hatMaterialName.Substring(index0);

        int index1 = subName.IndexOf("_");
        //benjaminPaul_Hats_shading_black
        string subName2 = subName.Substring(index1 + 1);

        int index2 = subName2.IndexOf("_");
        //Hats_shading_black
        string subName3 = subName2.Substring(index2 + 1);

        int index3 = subName3.IndexOf("_");
        //Hats
        string subName4 = subName3.Substring(0, index3);

        //Find all textures that contains that name
        Object[] filePaths = Resources.LoadAll(hatName, typeof(Texture));

        //string[] filePaths = Directory.GetFiles(TmpRoute);
        for(int i = 0; i < filePaths.Length; i++)
        {
            if (filePaths[i].name.Contains(subName4))
            {
                if (hatColor != "" && filePaths[i].name.Contains(hatColor))
                {
                    Texture t = (Texture2D)filePaths[i];
                    hatTextures.Add(t);
                }
            }
        }
        return hatTextures;
    }

    public void VerifyLayer(Material hatMaterial, string layerName, Texture hatTexture)
    {
        string[] layersName = { "AO", "difuse", "metalic", "normal", "roughness", "shading" };
        string materialName = hatMaterial.name;

        if (layerName.Contains(layersName[(int)materialLayer.AmbientOcclusion]))
        {
            hatMaterial.SetTexture("_Occlusion", hatTexture);
        }
        else if (layerName.Contains(layersName[(int)materialLayer.Diffuse]))
        {
            hatMaterial.SetTexture("_MainTex", hatTexture);
        }
        else if (layerName.Contains(layersName[(int)materialLayer.Metallic]))
        {
            hatMaterial.SetTexture("_MetallicGlossMap", hatTexture);
        }
        else if (layerName.Contains(layersName[(int)materialLayer.Normal]))
        {
            hatMaterial.SetTexture("_BumpMap", hatTexture);
        }
        else if (layerName.Contains(layersName[(int)materialLayer.Roughness]))
        {
            hatMaterial.SetTexture("_SpecGlossMap", hatTexture);
        }
    }

    public void ChangeMaterialBundle(string hatId, string hatColor)
    {
        //Renderer[] rendererChilds = m_CurrentHat.GetComponentsInChildren<Renderer>();
        //for (int i = 0; i < rendererChilds.Length; i++)
        //{
        //    //Get Textures using material name, and id
        //    //--
        //    //material: hats_benjaminPaul_Hats_shading_black
        //    //textures: hats_benjaminPaul_Hats_AO_black, hats_benjaminPaul_Hats_difuse_black, etc
        //    List<Texture> hatTextures = LoadTextures(hatId, rendererChilds[i].materials[0].name, hatColor);

        //    //Find layer using material name and layername
        //    for (int j = 0; j < hatTextures.Count; j++)
        //    {
        //        VerifyLayer(rendererChilds[i].materials[0], hatTextures[j].name, hatTextures[j]);
        //    }
        //}
        Debug.Log("Color: " + hatColor);
        Renderer[] rendererChilds = m_CurrentHat.GetComponentsInChildren<Renderer>();

        Debug.Log("mat: " + rendererChilds.Length);
        Debug.Log("content: " + m_CurrentHatContent.hatMaterials.Count);

        for (int i = 0; i < m_CurrentHatContent.hatMaterials.Count; i++)
        {
            if (m_CurrentHatContent.hatMaterials[i].materialName == hatColor)
            {
                Debug.Log("currentColor: " + m_CurrentHatContent.hatMaterials[i].materialName);

                for (int j = 0; j < rendererChilds.Length; j++)
                {
                    rendererChilds[j].material = m_CurrentHatContent.hatMaterials[i].hatMaterials[j];
                }
            }
        }

        
    }

    //Load from Resources
    public void LoadHat(string hatId)
    {
        Destroy(m_CurrentHat);
        m_CurrentHat = null;
        m_CurrentHat = (GameObject)Instantiate(Resources.Load(hatId));
        m_CurrentId = hatId;
        m_CurrentHat.transform.parent = m_HatPosition;
        m_CurrentHat.transform.localPosition = Vector3.zero;
        m_CurrentHat.transform.localRotation = Quaternion.identity;
        m_CurrentHat.transform.localScale = Vector3.one;

        
    }

    public void ChangeMaterial(int index)
    {
        HatContent textFile = Resources.Load<HatContent>(m_CurrentId + "_Scriptable") as HatContent;
        Debug.Log(m_CurrentId + "_Scriptable");

        Renderer[] a = m_CurrentHat.GetComponentsInChildren<Renderer>();
        List<Material> mats = new List<Material>();
        for(int i = 0; i < a.Length; i++)
        {
            for(int j = 0; j < a[i].materials.Length; j++)
            {
                mats.Add(a[i].materials[j]);
            }
        }

        for (int i = 0; i < mats.Count; i++)
        {
            for (int j = 0; j < textFile.hatMaterials.Count; j++)
            {
                string modelMaterialName = mats[i].name;
                string scriptableMaterialName = textFile.hatMaterials[j].materialName;                

                if (modelMaterialName.Contains(scriptableMaterialName))
                {                    
                   // mats[i].SetTexture("_MainTex", textFile.hatMaterials[j].hatTextures[index]);
                }
            }
        }
    }
    
    public void DeactivePhotoPanel()
    {
        m_PhotoPanel.SetActive(false);
        m_panels.SetActive(true);
       
    }

    public void ScreenshotButtonPressed()
    {

        m_panels.SetActive(false);
        m_PanelInfoCapturePhoto.SetActive (true);

        StartCoroutine(Utils.TakePhoto((img) =>
        {
            m_PhotoCaptureTexture = img;
            m_PhotoCapture.sprite = ConvertToSprite(m_PhotoCaptureTexture);

            m_PhotoPanel.SetActive(true);
            m_PanelInfoCapturePhoto.SetActive(false);

        }));
       
    }

    public void VideoCaptureButtonPressed()
    {
        
    }

    public void SaveCapturePhoto()
    {
        if (m_PhotoCaptureTexture != null)
        {
            NativeGallery.SaveImageToGallery(m_PhotoCaptureTexture, "Goorin Bros", FILE_SCREENSHOT_NAME);
            DeactivePhotoPanel();
        }
    }
    public void ShareCapturePhoto()
    {
        if (m_PhotoCaptureTexture != null)
        {
            string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
            File.WriteAllBytes(filePath, m_PhotoCaptureTexture.EncodeToPNG());

            new NativeShare().AddFile(filePath).Share();
        }
    }


    public static Sprite ConvertToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    
}
