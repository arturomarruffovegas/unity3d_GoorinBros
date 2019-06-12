using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatArController : MonoBehaviour
{
    private Dictionary<string, GameObject> m_Hat3DList;
    public Transform m_HatPosition;
    private GameObject m_CurrentHat;
    private string m_CurrentId;

    // Start is called before the first frame update
    void Start()
    {
        LoadHat("Hat1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadHat(List<string> hatIdList)
    {
        
    }

    public void LoadHatBundle(string hatId)
    {
        //Download bundle from firebase
    }

    public void LoadTextures(string hatId)
    {
        //Download textures from firebase
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
}
