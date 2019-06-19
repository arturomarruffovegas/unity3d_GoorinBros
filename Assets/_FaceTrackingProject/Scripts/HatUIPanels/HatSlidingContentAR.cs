using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public struct HatData
{
    public string hatId;
    public string hatName;
    public string hatBrand;
    public Sprite hatPhoto;
    public string[] hatColorList;
    public string[] hatSizeList;
    public string hatColor;
}

public class HatSlidingContentAR : MonoBehaviour
{
    [Header("Panel Prefab")]
    public GameObject m_ContentHatPanelPrefb;

    //Swipe Movement
    private List<GameObject> m_ListContentHatPanel;
    private float m_ListElementOffsetX = 500.0f;
    private int m_CurrentIndexElement = 0;
    private bool m_IsDragging;
    private float m_Time;
    private Vector3 m_FirstPointDrag;

    public HatArController m_HatArController;

    //Test Data
    public Sprite hatPhoto;
    public Sprite hatPhoto2;
    private List<HatData> m_HatData;

    public  static UnityAction LoadContentGG;
    // Start is called before the first frame update
    void Start()
    {
        m_ListContentHatPanel = new List<GameObject>();
        m_ListElementOffsetX = Screen.width;

        m_HatData = new List<HatData>();
        HatData d1 = new HatData();
        d1.hatId = "welfleet";
        d1.hatName = "hats_benjaminPaul";
        d1.hatBrand = "Fedora";
        d1.hatPhoto = hatPhoto;
        d1.hatColorList = new string[] { "black", "gray" };
        d1.hatSizeList = new string[] { "s", "m", "l" };
        d1.hatColor = "black";

        HatData d2 = new HatData();
        d2.hatId = "county line";
        d2.hatName = "hats_countryLine";
        d2.hatBrand = "Fedora";
        d2.hatPhoto = hatPhoto2;
        d2.hatColorList = new string[] { "black", "gray"};
        d2.hatSizeList = new string[] { "m", "l" };
        d2.hatColor = "black";

        m_HatData.Add(d1);
        m_HatData.Add(d2);

        LoadContentGG += GG;
    }

    // Update is called once per frame
    void GG()
    {

        //SwipeMovement(-1.0f);
        //List<GameObject> listContentHatPanel = new List<GameObject>();
        for (int i = 0; i < m_ListContentHatPanel.Count; i++)
        {
            Destroy(m_ListContentHatPanel[i].gameObject);
        }
        m_ListContentHatPanel.Clear();
        LoadContent(m_HatData);

    }

    private void OnDestroy()
    {
        LoadContentGG -= GG;
        for(int i = 0; i < m_ListContentHatPanel.Count; i++)
        {
            Destroy(m_ListContentHatPanel[i].gameObject);
        }
        m_ListContentHatPanel.Clear();
    }

    public void OnDragPoint()
    {
        m_IsDragging = true;
        m_FirstPointDrag = Input.mousePosition;
    }

    public void StopDragPoint()
    {
        m_IsDragging = false;
        float diference = Input.mousePosition.x - m_FirstPointDrag.x;
        SwipeMovement(diference);
    }

    public void LoadContent(List<HatData> listHatData)
    {
        //for(int i = 0; i < listContentHatPanel.Count; i++)
        for (int i = 0; i < listHatData.Count; i++)
        {
            GameObject element = (GameObject)Instantiate(m_ContentHatPanelPrefb, transform);
            element.SetActive(true);
            //element.transform.parent = transform;            
            element.transform.position += new Vector3(m_ListElementOffsetX * i, 0, 0);
            element.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            //Load data from list (Image, description, etc)
            element.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(ChangeHat);
            //element.GetComponent<HatPanelArPrefab>().LoadInformation("hats_benjaminPaul", "hats_benjaminPaul",
            //                                         "Fedora", hatPhoto, new string[] { "1", "2", "3" }, new string[] { "s", "m", "l" });

            element.GetComponent<HatPanelArPrefab>().LoadInformation(listHatData[i].hatId,
                                                                     listHatData[i].hatName,
                                                                     listHatData[i].hatBrand,
                                                                     listHatData[i].hatPhoto,
                                                                     listHatData[i].hatColorList,
                                                                     listHatData[i].hatSizeList,
                                                                     listHatData[i].hatColor);

            m_ListContentHatPanel.Add(element);
        }

        //Load first element / Load current element 
        m_HatArController.LoadHatBundle(listHatData[m_CurrentIndexElement].hatId, listHatData[m_CurrentIndexElement].hatColor);
    }

    public void ChangeHat()
    {
        //m_HatArController.LoadHat("Hat" + (m_CurrentIndexElement + 1));
        m_HatArController.LoadHatBundle(m_HatData[m_CurrentIndexElement].hatId, m_HatData[m_CurrentIndexElement].hatColor);
    }

    public void SwipeMovement(float swipeDirection)
    {
        float directioValue = (swipeDirection > 0 ? 1.0f : -1.0f);

        if((m_CurrentIndexElement != 0 && directioValue > 0) ||
           (m_CurrentIndexElement < m_ListContentHatPanel.Count - 1 && directioValue < 0))
        {
            m_CurrentIndexElement -= (int)directioValue;

            for (int i = 0; i < m_ListContentHatPanel.Count; i++)
            {
                float endValue = m_ListContentHatPanel[i].transform.position.x + m_ListElementOffsetX * directioValue;
                m_ListContentHatPanel[i].transform.DOMoveX(endValue, 0.2f);
            }
        }

        ChangeHat();
    }
}
