using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjGUIActionPane : krjGUICollection
{
    public int selected;
    private string[] tabNames;

    public krjGUIActionPane(int _id, krjGUICollection _parent) : base(_id, _parent)
    {
    }

    public krjGUIActionTab addTab(string _label)
    {
        krjGUIActionTab ret;
        ret = new krjGUIActionTab(getCanvas().getNewId(), this);
        addNode(ret);
        ret.label = _label;
        return ret;
    }

    public override void draw()
    {
        if (tabNames.Length > 1)
        {
            GUILayout.BeginVertical();
            selected = GUILayout.Toolbar(selected, tabNames, GUILayout.ExpandWidth(false));
            items.Values[selected].draw();
            GUILayout.EndVertical();
        }
        else
        {
            items.Values[0].draw();
        }
    }

    public override float getChildCurrentLeft()
    {
        throw new NotImplementedException();
    }

    public override float getChildCurrentTop()
    {
        throw new NotImplementedException();
    }

    public override string getLabel()
    {
        return "";
    }

    public override void init()
    {
        base.init();
        tabNames = new string[items.Count];
        int i = 0;
        foreach (KeyValuePair<int, krjGUINode> item in items)
        {
            tabNames[i] = item.Value.label;
            Debug.Log(item.Value.label);
            i++;
        }
    }
}
