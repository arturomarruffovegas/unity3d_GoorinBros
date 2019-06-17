using UnityEditor;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
public class CreateAssetBundles
{
    //Creates a new menu (Build Asset Bundles) and item (Normal) in the Editor
    [MenuItem("Goorin Bros/Build Asset Bundles/Normal/Android")]
    static void BuildABsNoneAndroid()
    {
        //Create a folder to put the Asset Bundle in.
        // This puts the bundles in your custom folder (this case it's "MyAssetBuilds") within the Assets folder.
        //Build AssetBundles with no special options
        if(!Directory.Exists("Assets/AssetBundles/Android"))
            Directory.CreateDirectory("Assets/AssetBundles/Android");

        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Android", BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    //Creates a new menu (Build Asset Bundles) and item (Normal) in the Editor
    [MenuItem("Goorin Bros/Build Asset Bundles/Normal/iOS")]
    static void BuildABsNoneiOs()
    {
        //Create a folder to put the Asset Bundle in.
        // This puts the bundles in your custom folder (this case it's "MyAssetBuilds") within the Assets folder.
        //Build AssetBundles with no special options
        if (!Directory.Exists("Assets/AssetBundles/iOS"))
            Directory.CreateDirectory("Assets/AssetBundles/iOS");

        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/iOS", BuildAssetBundleOptions.None, BuildTarget.iOS);
    }

    //Creates a new item (Strict Mode) in the new Build Asset Bundles menu
    [MenuItem("Goorin Bros/Build Asset Bundles/Strict Mode/Android")]
    static void BuildABsStrict()
    {
        //Build the AssetBundles in strict mode (build fails if any errors are detected)
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.StrictMode, BuildTarget.Android);
    }

    //Creates a new item (Strict Mode) in the new Build Asset Bundles menu
    [MenuItem("Goorin Bros/Build Asset Bundles/Strict Mode/iOS")]
    static void BuildABsStrictiOs()
    {
        //Build the AssetBundles in strict mode (build fails if any errors are detected)
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/iOS", BuildAssetBundleOptions.StrictMode, BuildTarget.iOS);
    }
}
#endif