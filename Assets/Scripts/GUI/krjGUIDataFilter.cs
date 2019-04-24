using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjGUIDataFilter
{
    public string name { get; private set; }
    public string value { get; set; }
    public krjGUIDataFilter (string _name)
    {
        name = _name;
    }
}
