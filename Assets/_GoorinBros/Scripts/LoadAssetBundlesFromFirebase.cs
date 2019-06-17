using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LoadAssetBundlesFromFirebase : MonoBehaviour
{
    public string hatName;
    public Object obj;

    private bool isModel;
    private string URLDownload;
    void Start()
    {
        FirebaseController.InitialStorageFirebase(FirebaseController.ConstantStorageURL);
    }


    private void LoadModel(string Name)
    {
        FirebaseController.GetURLDownload(Name, (URL) => 
        {
            Debug.Log("URL: " + URL);
            URLDownload = URL;
            isModel = true;

        });
    }

    private void GetModel3D()
    {
        LoadModel(hatName);
    }

    private void Update()
    {
        if (isModel == true)
        {
            StartCoroutine(LoadS(hatName, URLDownload));
            isModel = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            hatName = "welfleet";
            LoadModel(hatName);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            hatName = "county line";
            LoadModel(hatName);
        }
    }
    private IEnumerator LoadS(string name,string myURl)
    {
        if (obj != null)
            Destroy(obj);

        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(myURl);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

            if (bundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                yield return null;
            }

            AssetBundleRequest object3D = bundle.LoadAssetAsync<GameObject>(name);
          
            obj = Instantiate(object3D.asset);


            bundle.Unload(false);
        }
    }
}
