﻿using UnityEngine;
using System.Collections;

// Unity singleton implementation. Call SetDontDestroy() to persist objects between scenes.
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

    protected void SetDontDestroy()
    {
        DontDestroyOnLoad(gameObject);
    }
}
