using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjGUIFindPeopleMenuItem : krjGUIMenuItem
{
    public krjGUIFindPeopleMenuItem(int _id, krjGUICollection _parent, krjMenuItemPreset _preset, int _imageNum, krjGUIDatasource _dataSource) : 
        base(_id, _parent, _preset, _imageNum, _dataSource)
    {
    }

    public override string getLabel()
    {
        return "Find peoples";
    }

    public override void run()
    {
        getCanvas().canvasHelper.mainCircle.launchAction<krjActFindPeople>();
    }
}
