using Firebase;
using Firebase.Storage;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public static class FirebaseController
{
    static FirebaseStorage storage;
    static StorageReference storage_ref;
    static StorageReference sRef;

    static StorageReference gs_reference;

    public static string ConstantStorageURL = "gs://goorin-bros.appspot.com";

    public static void InitialStorageFirebase(string storageURL)
    {
        storage = FirebaseStorage.DefaultInstance;
        storage_ref = storage.GetReferenceFromUrl(storageURL);

    }

    public static void GetURLDownload(string hatName , UnityAction<string> URL)
    {
        string myURL = ""; 
#if UNITY_ANDROID
        string Platform = "android";
#elif UNITY_IOS
        string Platform = "ios";
#endif
        string URLD = ConstantStorageURL + "/" + hatName + "/" + Platform +"/" + hatName;
        gs_reference = storage.GetReferenceFromUrl(URLD);
        gs_reference.GetDownloadUrlAsync().ContinueWith((Task<Uri> task2) =>
        {
            if (!task2.IsFaulted && !task2.IsCanceled)
            {
                myURL = task2.Result.OriginalString;
                Debug.Log("Download URL: " + task2.Result.OriginalString);
                URL(myURL);
            }
        });
    }

    //public static IEnumerator UpdateImageToStorageFirebase(byte[] byteImage, string name, Action<string> text)
    //{
    //    string URLDownload = null;
    //    bool check = false;
    //    if (byteImage != null)
    //    {
    //        sRef = storage_ref.Child("Photos/" + name + ".jpg");
            
    //        sRef.PutBytesAsync(byteImage).ContinueWith((Task<StorageMetadata> task) =>
    //        {
    //            if (task.IsFaulted || task.IsCanceled)
    //                Debug.Log(task.Exception.ToString());
    //            else
    //            {
    //                StorageMetadata metadata = task.Result;
    //                string download_url = sRef.ToString();
    //                Debug.Log("Finished uploading...");
    //                Debug.Log("download url = " + download_url);

    //                //change MIME image
    //                MetadataChange new_metadata = new MetadataChange();
    //                new_metadata.CacheControl = "public,max-age=300";
    //                new_metadata.ContentType = "image/jpeg";

    //                sRef.UpdateMetadataAsync(new_metadata).ContinueWith(task3 => {
    //                    if (!task3.IsFaulted && !task3.IsCanceled)
    //                    {
    //                        Firebase.Storage.StorageMetadata meta = task3.Result;
    //                    }
    //                });

    //                //get URL from download
    //                gs_reference = storage.GetReferenceFromUrl(download_url);
    //                gs_reference.GetDownloadUrlAsync().ContinueWith((Task<Uri> task2) =>
    //                {
    //                    if (!task2.IsFaulted && !task2.IsCanceled)
    //                    {
    //                        URLDownload = task2.Result.OriginalString;
    //                        Debug.Log("Download URL: " + task2.Result.OriginalString);
    //                        check = true;
    //                    }
    //                    else
    //                        check = true;
    //                });

    //            }
    //        });
    //    }
    //    yield return new WaitUntil(() => check);
    //    text(URLDownload);

    //}

}

  