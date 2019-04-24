using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum krjMenuItemPreset
{
    None,
    ActionPaneBigButton
}

public abstract class krjGUIMenuItem : krjGUINode
{
    public krjMenuItemPreset preset;

    public krjGUIMenuItem(int _id, 
        krjGUICollection _parent, 
        krjMenuItemPreset _preset, 
        int _imageNum,
        krjGUIDatasource _dataSource) : base(_id, _parent)
    {
        preset = _preset;
        imageNum = _imageNum;
        dataSource = _dataSource;
    }

    public override void normalDraw()
    {
        if(GUI.Button(currentRect,content,style))
        {
            run();
        }
    }

    public override void layoutDraw()
    {
        if (GUILayout.Button(content, style))
        {
            run();
        }
    }

    public override void init()
    {
        base.init();
        style = new GUIStyle(parent.getCanvas().GeneralSkin.button);
        switch (preset)
        {
            case krjMenuItemPreset.ActionPaneBigButton:
                style.fixedHeight = 72;
                style.imagePosition = ImagePosition.ImageAbove;
                style.stretchWidth = false;
                layout = true;
                break;
        }
        active = AutoNoYes.Auto;
        enable = AutoNoYes.Auto;
        left = 4;
        top = 20;
        height = 24;
        width= 128;
        currentRect = new Rect(left, top, width, height);
    }

    public abstract void run();

}
