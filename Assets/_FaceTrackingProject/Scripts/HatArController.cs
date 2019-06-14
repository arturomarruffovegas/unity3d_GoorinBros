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
    private GameObject m_CurrentHat;
    private string m_CurrentId;

    [Header("CapturePhoto")]
    public List<Camera> m_HatCameras;
    public GameObject m_PhotoPanel;
    public Button m_PhotoButton;
    public Button m_VideoButton;
    public GameObject m_ProductPanel;
    public GameObject m_RightPanel;
    public Image m_PhotoCapture;
    public static string FILE_SCREENSHOT_NAME = System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
    private Texture2D m_PhotoCaptureTexture;
    public Button m_SavePhotoButton;
    public Button m_PhotoPanelBackButton;

    //Temp List Texture


    // Start is called before the first frame update
    void Start()
    {
        ScreenshotCapturer.SetCameras(m_HatCameras[0], m_HatCameras[1]);
        m_PhotoButton.onClick.AddListener(ScreenshotButtonPressed);
        m_VideoButton.onClick.AddListener(VideoCaptureButtonPressed);
        m_SavePhotoButton.onClick.AddListener(SaveCapturePhoto);
        m_PhotoPanelBackButton.onClick.AddListener(DeactivePhotoPanel);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadHat(List<string> hatIdList)
    {
        
    }

    //Load Texture From Bundle Local/Online
    public void LoadHatBundle(string hatId, string hatColor)
    {
        m_RightPanel.SetActive(true);
        //Download bundle from firebase
        //---
        if (m_CurrentHat)
        {
            Destroy(m_CurrentHat.gameObject);
        }
        
        m_CurrentHat = null;
        Debug.Log(hatId + "/hats_" + hatId + "_" + hatColor);
        m_CurrentHat = (GameObject)Instantiate(Resources.Load(hatId + "/hats_" + hatId +"_" + hatColor));
        m_CurrentId = hatId;
        m_CurrentHat.transform.parent = m_HatPosition;
        m_CurrentHat.transform.localPosition = Vector3.zero;
        m_CurrentHat.transform.localRotation = Quaternion.identity;
        m_CurrentHat.transform.localScale = Vector3.one;
        
        LoadTextureforBundle(m_CurrentHat, hatId, hatColor);
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
        Renderer[] rendererChilds = m_CurrentHat.GetComponentsInChildren<Renderer>();
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
                    mats[i].SetTexture("_MainTex", textFile.hatMaterials[j].hatTextures[index]);
                }
            }
        }
    }

    public void ActivePhotoPanel()
    {
        m_PhotoPanel.SetActive(true);
        m_ProductPanel.SetActive(false);
        m_RightPanel.SetActive(false);
    }

    public void DeactivePhotoPanel()
    {
        m_PhotoPanel.SetActive(false);
        m_ProductPanel.SetActive(true);
        m_RightPanel.SetActive(true);
    }

    public void ScreenshotButtonPressed()
    {
        ActivePhotoPanel();
        m_PhotoCaptureTexture = ScreenshotCapturer.Capture();
        m_PhotoCapture.sprite = ConvertToSprite(m_PhotoCaptureTexture);
    }

    public void SaveCapturePhoto()
    {
        Debug.Log(Application.persistentDataPath);
        string filename = FILE_SCREENSHOT_NAME;
        string PhotoPath = System.IO.Path.Combine(Application.persistentDataPath, filename);
        System.IO.File.WriteAllBytes(PhotoPath, m_PhotoCaptureTexture.EncodeToJPG(100));
    }

    public void VideoCaptureButtonPressed()
    {
        
    }

    public static Sprite ConvertToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
}
