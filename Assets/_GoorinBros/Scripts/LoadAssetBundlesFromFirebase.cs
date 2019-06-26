using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class LoadAssetBundlesFromFirebase : MonoBehaviour
{
    public static UnityAction<Object, Object> FinishProccess;

    void Start()
    {
        FirebaseController.InitialStorageFirebase(FirebaseController.ConstantStorageURL);
      
    }
    
    public void QueryFirebase(string Name)
    {
        FirebaseController.GetURLDownload(Name, FinishProccess);
    }
}
