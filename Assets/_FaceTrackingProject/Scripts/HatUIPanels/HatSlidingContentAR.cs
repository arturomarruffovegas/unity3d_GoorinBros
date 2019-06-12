using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class HatSlidingContentAR : MonoBehaviour
{
    public GameObject m_ContentHatPanel;
    private List<GameObject> m_ListContentHatPanel;
    private float m_ListElementOffsetX = 500.0f;

    private int m_CurrentIndexElement = 0;
    private bool m_IsDragging;
    private float m_Time;
    private Vector3 m_FirstPointDrag;

    public HatArController m_HatArController;

    // Start is called before the first frame update
    void Start()
    {
        m_ListContentHatPanel = new List<GameObject>();
        m_ListElementOffsetX = Screen.width;

        List<GameObject> listContentHatPanel = new List<GameObject>();
        //listContentHatPanel.Add(new GameObject());
        //listContentHatPanel.Add(new GameObject());
        //listContentHatPanel.Add(new GameObject());
        //Debug.Log(listContentHatPanel.Count);
        LoadContent(listContentHatPanel);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            SwipeMovement(-1.0f);
        }
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

    public void LoadContent(List<GameObject> listContentHatPanel)
    {
        //for(int i = 0; i < listContentHatPanel.Count; i++)
        for(int i = 0; i < 3; i++)
        {
            GameObject element = (GameObject)Instantiate(m_ContentHatPanel);
            element.SetActive(true);
            element.transform.parent = transform;            
            element.transform.position += new Vector3(m_ListElementOffsetX * i, 0, 0);
            element.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            //Load data from list (Image, description, etc)

            element.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(ChangeHat);

            m_ListContentHatPanel.Add(element);
            
        }        
    }

    public void ChangeHat()
    {
        m_HatArController.LoadHat("Hat" + (m_CurrentIndexElement + 1));
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
