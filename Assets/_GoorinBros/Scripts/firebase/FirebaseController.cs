using Firebase;
using Firebase.Storage;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public static class FirebaseController
{
    static FirebaseStorage storage;
    static StorageReference storage_ref;
    static StorageReference sRef;

    static StorageReference gs_referenceM;
    static StorageReference gs_referenceC;

    public static string ConstantStorageURL = "gs://goorin-3d-models.appspot.com";

    public static bool stopDownload;

    public static UnityAction OnErrorAssetBundle;

    public static void InitialStorageFirebase(string storageURL)
    {
        storage = FirebaseStorage.DefaultInstance;
        storage_ref = storage.GetReferenceFromUrl(storageURL);

    }

    public static void GetURLDownload(string hatName, UnityAction<UnityEngine.Object, UnityEngine.Object> URLS)
    {
#if UNITY_ANDROID
        string Platform = "android";
#elif UNITY_IOS
      string Platform = "ios";
#endif
        string modelURL = ConstantStorageURL + "/" + "Hats" + "/" + Platform + "/" + hatName;
        string contentURL = ConstantStorageURL + "/" + "Hats" + "/" + Platform + "/" + hatName + "_content";
        gs_referenceM = storage.GetReferenceFromUrl(modelURL);
        gs_referenceC = storage.GetReferenceFromUrl(contentURL);
        GG(gs_referenceM.GetDownloadUrlAsync(), gs_referenceC.GetDownloadUrlAsync(), hatName, URLS);

    }

    private static async void GG(Task<Uri> uriM, Task<Uri> uriC, string name, UnityAction<UnityEngine.Object, UnityEngine.Object> _URLS)
    {
        try
        {
            var s = await uriM;
            var d = await uriC;

            string myURLM = "";
            UnityEngine.Object model;
            string myURLC = "";
            UnityEngine.Object content;

            if (!uriM.IsFaulted && !uriM.IsCanceled)
            {
                myURLM = uriM.Result.OriginalString;
                Debug.Log("Download Model: " + uriM.Result.OriginalString);

                OnDownloadAssetBundles(name, myURLM, (mo) =>
                {
                    model = mo;
                    stopDownload = false;

                    if (!uriC.IsFaulted && !uriC.IsCanceled)
                    {
                        myURLC = uriC.Result.OriginalString;
                        Debug.Log("Download Content: " + uriC.Result.OriginalString);

                        OnDownloadAssetBundles(name + "_content", myURLC, (co) =>
                        {
                            content = co;
                            _URLS?.Invoke(model, content);
                            stopDownload = false;
                        });
                    }
                });
            }
            else
            {
               
            }
        }
        catch (Exception e)
        {
            OnErrorAssetBundle();
        }
        
    }

    private static void OnDownloadAssetBundles(string Name, string UrlModel, UnityAction<UnityEngine.Object> model)
    {
        stopDownload = true;
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(UrlModel);
        var a = www.SendWebRequest();

        a.completed += (b) => {
            if (stopDownload == true)
            {
                
                stopDownload = false;
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                AssetBundleRequest object3D = bundle.LoadAssetAsync<UnityEngine.Object>(Name);
                model?.Invoke(object3D.asset);
                bundle.Unload(false);
            }

            
        };
    }

}