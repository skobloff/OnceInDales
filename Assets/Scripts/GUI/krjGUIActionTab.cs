using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjGUIActionTab : krjGUICollection
{
    public krjGUIActionTab(int _id, krjGUICollection _parent) : base(_id, _parent)
    {
    }

    public override void draw()
    {
        GUILayout.BeginHorizontal();
        base.draw();
        GUILayout.EndHorizontal();
    }

    public void addButton<T>(krjMenuItemPreset _preset, int _imageNum, krjGUIDatasource _dataSource = null)
    {
        krjGUIMenuItem newButton = (krjGUIMenuItem)Activator.CreateInstance(typeof(T), 
            new object[] { getCanvas().getNewId(), this, _preset, _imageNum, _dataSource});
        addNode(newButton);
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
}
