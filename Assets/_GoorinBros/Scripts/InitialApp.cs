using goorinAR;
using Shopify.Examples.Helpers;
using Shopify.Unity;
using Shopify.Unity.SDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialApp : MonoBehaviour
{
    [Header("Shopify")]
    public string AccessToken;
    public string ShopDomain;

    public List<string> shapes = new List<string>();

    public static List<Product> m_product = new List<Product>();

    public void Start()
    {
        Tags.SetTag(true);

        ShopifyBuy.Init(AccessToken, ShopDomain);

        DefaultQueries.MaxProductPageSize = 5;

        FetchProducts((list, cursor) => 
        {
            foreach (var product in list)
            {

                var tags = product.tags();
                foreach (var item in tags)
                {
                    string[] letters = item.Split(':');
                    if (letters.Length > 0)
                    {
                        if (letters[0] == "shape")
                        {
                            if (ValueShape(shapes, letters[1]) == false)
                            {
                                shapes.Add(letters[1]);
                            }
                           
                        }
                    }
                }

                m_product.Add(product);

                DefaultQueries.MaxProductPageSize = 10;
                SceneManager.LoadScene(1);
            }
        });
        

    }

    private bool ValueShape(List<string> m_shapes, string m_shape)
    {
        for (int i = 0; i < m_shapes.Count; i++)
        {
            if (m_shapes[i] == m_shape)
                return true;
        }

        return false;
    }
    
    private void FetchProducts(Action<List<Product>, string> successCallback)
    {
        ShopifyBuy.Client().products((products, error, after) =>
        {

            if (error != null)
            {
                return;
            }
            successCallback(products, after);

        },60);
    }
}
