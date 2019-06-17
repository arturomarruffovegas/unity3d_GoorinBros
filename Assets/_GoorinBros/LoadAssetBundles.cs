using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadAssetBundles : MonoBehaviour {
   
  
    public GameObject ObjectBundles;

    private bool s;

    string URLBasic;


    public void Awake()
    {
       StartCoroutine(  LoadAssetB("PreEscolar_Play", "preescolar_play",null));
    }

    public IEnumerator LoadAssetB(string _ObjectName, string _AssetBundlesName, Action SetObject)
    {

#if UNITY_EDITOR
        URLBasic = Application.streamingAssetsPath + "\\AssetBundles\\Android";
#else

        URLBasic = "jar:file://" + Application.dataPath + "!/assets" + "/AssetBundles/Android";
#endif
        AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(URLBasic, _AssetBundlesName));
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            yield return null;
        }

        AssetBundleRequest objects = myLoadedAssetBundle.LoadAllAssetsAsync(typeof(GameObject));
        yield return objects;
        foreach (GameObject o in objects.allAssets)
        {
            if (_ObjectName == o.name)
            {
                ObjectBundles = o;
                yield return ObjectBundles;
            }
            
        }

        foreach (GameObject o in objects.allAssets)
        {
            string Name = o.name;
            string[] NamesList = Name.Split(new string[] { "_" }, StringSplitOptions.None);
            if (NamesList.Length > 2)
            {
                string nom = NamesList[0] + "_" + NamesList[1] + "_" + NamesList[2];
                if(_ObjectName == nom)
                {
                    ObjectBundles = o;
                }
            }
        }

        if (s == false)
        {
            Instantiate(ObjectBundles);
            // ObjectBundles = null;
            s = true;
        }
        myLoadedAssetBundle.Unload(false);
        SetObject?.Invoke();
    }

   
}
