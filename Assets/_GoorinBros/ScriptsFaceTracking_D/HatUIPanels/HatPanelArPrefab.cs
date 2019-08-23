using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HatPanelArPrefab : MonoBehaviour
{
    [HideInInspector]
    public string m_HatId;

    public int index;
    public Text m_HatName;
    public Text m_HatBrand;
    public Image m_HatPhoto;
    public RectTransform m_HatColorList;
    public RectTransform m_HatSizeList;
    public Button cart;
    public GameObject m_ColorOptionPrefab;
    public GameObject m_SizeOptionPrefab;

    private List<GameObject> m_HatColors;
    private List<GameObject> m_HatSizes;

    private HatArController m_HatArController;

    // Start is called before the first frame update
    void Start()
    {
        m_HatArController = GameObject.FindObjectOfType<HatArController>();
        m_HatColors = new List<GameObject>();
        m_HatSizes = new List<GameObject>();
    }

    public void LoadInformation(string hatId, string hatName, string hatBrand, Sprite hatPhoto, List<string> hatColorList, List<string> hatSizeList, string hatColor)
    {
        m_HatId = hatId;
        m_HatName.text = hatName;
        m_HatBrand.text = hatBrand;
        m_HatPhoto.sprite = hatPhoto;

       
            for (int i = 0; i < hatColorList.Count; i++)
            {
                GameObject col = (GameObject)Instantiate(m_ColorOptionPrefab, m_HatColorList.transform);
                col.SetActive(true);

                //Change Color Temp
                col.GetComponent<Image>().color = Random.ColorHSV();

                col.GetComponent<HatColorButtonAR>().InitializeValues(hatId, hatColorList[i]);


                //m_HatColors.Add(col);
            }
       

        for (int i = 0; i < hatSizeList.Count; i++)
        {
            GameObject siz = (GameObject)Instantiate(m_SizeOptionPrefab, m_HatSizeList.transform);
            siz.SetActive(true);
            siz.transform.GetChild(0).GetComponent<Text>().text = hatSizeList[i];
            //m_HatSizes.Add(siz);
        }
    }

    public void ChangeHatMaterial(string hatColor)
    {
        
    }

    //private void OnDestroy()
    //{
    //    for (int i = 0; i < m_HatColors.Count; i++)
    //    {
    //        Destroy(m_HatColors[i].gameObject);
    //    }

    //    for (int i = 0; i < m_HatSizes.Count; i++)
    //    {
    //        Destroy(m_HatSizes[i].gameObject);
    //    }
    //}
}
