using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjGUIText : krjGUINode
{
    public krjGUIText(int _id, krjGUICollection _parent, krjGUIDatasource _dataSource = null, string _fieldName = "") : 
        base(_id, _parent, _dataSource, _fieldName)
    {
    }

    public override void layoutDraw()
    {
        GUILayout.Label(takeData());
    }

    public override void normalDraw()
    {
        GUI.Label(currentRect, takeData());
    }

    public override string getLabel()
    {
        return "жопа001";
    }
}

