using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GlobalInfo : Singleton<GlobalInfo>
{

    protected override void Awake()
    {
        base.Awake();
        SetDontDestroy();
    }

}
