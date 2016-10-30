﻿using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{

    public static T instance;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetDontDestroy()
    {
        DontDestroyOnLoad(gameObject);
    }
}
