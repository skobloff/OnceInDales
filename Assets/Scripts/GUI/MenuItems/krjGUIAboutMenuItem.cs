using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjGUIAboutMenuItem : krjGUIMenuItem
{
    public krjGUIAboutMenuItem(int _id, krjGUICollection _parent, krjMenuItemPreset _preset, int _imageNum, krjGUIDatasource _dataSource) : 
        base(_id, _parent, _preset, _imageNum, _dataSource)
    {
    }


    public override string getLabel()
    {
        return "Об игре";
    }

    public override void run()
    {
        throw new NotImplementedException();
    }
}
