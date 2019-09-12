using goorinAR;
using Shopify.Examples.Helpers;
using Shopify.Unity;
using Shopify.Unity.SDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitialApp : MonoBehaviour
{
    [Header("Shopify")]
    public string AccessToken;
    public string ShopDomain;

    public List<string> shapes = new List<string>();

    public List<Sprite> iconsShape = new List<Sprite>();

    public static List<Product> m_product = new List<Product>();

    [SerializeField]
    private GameObject instructionalPanel;
    [SerializeField]
    private Button goInstructional;

    public bool viewInstructional;

    [Header("Login")]
    [SerializeField]
    private GameObject loginPanel;
    [SerializeField]
    private InputField inputField;

    public void CheckCode()
    {
        if(inputField.text == "GoorinBros")
        {
            loginPanel.SetActive(false);
        }
    }

    public void Start()
    {
        //yield return new WaitForSeconds(2f);
        //SceneManager.LoadScene(1);

      // PlayerPrefs.DeleteAll();

        viewInstructional = IntToBool(PlayerPrefs.GetInt(InfoPlayerPrefs()));

        if (viewInstructional)
        {

            goInstructional.onClick.AddListener(OnDisableInstructionalPanel);
            OnDisableInstructionalPanel();
        }
        else
        {
            goInstructional.onClick.AddListener(OnDisableInstructionalPanel);
        }

       
    }

    public void OnEnableInstructionalPanel()
    {
        instructionalPanel.SetActive(true);
    }

    public void OnDisableInstructionalPanel()
    {

        PlayerPrefs.SetInt(InfoPlayerPrefs(), BoolToInt(true));

        instructionalPanel.SetActive(false);

        Scene scene = SceneManager.GetActiveScene();

        if(scene.buildIndex == 0)
        {
            StartCoroutine(OnNextScene());
        }
    }

    private string InfoPlayerPrefs()
    {
        return "instructional";
    }

    private int BoolToInt(bool b)
    {
        if (b)
            return 1;
        else
            return 0;
    }
    private bool IntToBool(int b)
    {
        if (b >= 1)
            return true;
        else
            return false;
    }

    private IEnumerator OnNextScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }



    //public void Start()
    //{
    //    Tags.SetTag(true);

    //    ShopifyBuy.Init(AccessToken, ShopDomain);

    //  //  DefaultQueries.MaxProductPageSize = 5;  /// 5

    //    FetchProducts((list, cursor) => 
    //    {
    //        foreach (var product in list)
    //        {

    //            var tags = product.tags();
    //            foreach (var item in tags)
    //            {
    //                string[] letters = item.Split(':');
    //                if (letters.Length > 0)
    //                {
    //                    if (letters[0] == "shape")
    //                    {
    //                        if (ValueShape(shapes, letters[1]) == false)
    //                        {
    //                            shapes.Add(letters[1]);
    //                        }

    //                    }
    //                }
    //            }

    //            m_product.Add(product);

    //          //  DefaultQueries.MaxProductPageSize = 10;
    //           // SceneManager.LoadScene(1);
    //        }
    //    });


    //}

    //private bool ValueShape(List<string> m_shapes, string m_shape)
    //{
    //    for (int i = 0; i < m_shapes.Count; i++)
    //    {
    //        if (m_shapes[i] == m_shape)
    //            return true;
    //    }

    //    return false;
    //}

    //private void FetchProducts(Action<List<Product>, string> successCallback)
    //{
    //    ShopifyBuy.Client().products((products, error, after) =>
    //    {

    //        if (error != null)
    //        {
    //            return;
    //        }
    //        successCallback(products, after);

    //    },null); /// 60
    //}


}
