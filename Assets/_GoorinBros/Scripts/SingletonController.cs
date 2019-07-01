using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonController : MonoBehaviour
{
    public static SingletonController instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
