using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HatContent", menuName = "HatElements/HatContent", order = 1)]
public class HatContent : ScriptableObject
{
    public string hatName;
    public List<HatMateials> hatMaterials;
}

[System.Serializable]
public class HatMateials
{
    public string materialName;
    public List<Material> hatMaterials;
}