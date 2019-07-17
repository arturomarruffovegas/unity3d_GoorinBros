using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialSingletonController : MonoBehaviour
{
    public static InitialSingletonController instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}
