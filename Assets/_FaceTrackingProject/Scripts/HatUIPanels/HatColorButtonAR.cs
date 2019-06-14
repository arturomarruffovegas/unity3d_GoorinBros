using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatColorButtonAR : MonoBehaviour
{
    private string m_HatId;
    private string m_HatColor;
    private HatArController m_HatArController;

    // Start is called before the first frame update
    void Start()
    {
        m_HatArController = GameObject.FindObjectOfType<HatArController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeValues(string hatId, string hatColor)
    {
        m_HatId = hatId;
        m_HatColor = hatColor;

        GetComponent<Button>().onClick.AddListener(ChangeHatMaterial);
    }

    public void ChangeHatMaterial()
    {
        m_HatArController.ChangeMaterialBundle(m_HatId, m_HatColor);
    }
}
