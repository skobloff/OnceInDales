using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjGUICanvas : krjGUICollection
{
    public Texture piples;
    public krjCanvasHelper canvasHelper;
    public GUISkin GeneralSkin;
    public GUISkin MainMenuSkin;
    private krjGUIForm mainMenu;
    private krjGUIForm mainStatus;
    private int lastLeft;
    private int lastTop;
    private int lastRight;
    private int lastBottom;
    private int lastId;



    public krjGUICanvas(krjCanvasHelper  _canvasHelper) : base(0, null)
    {
        lastId = 0;
        canvasHelper = _canvasHelper;
    }

    public int getNewId()
    {
        lastId++;
        return lastId;
    }

    public override float getChildCurrentLeft()
    {
        throw new NotImplementedException();
    }

    public override float getChildCurrentTop()
    {
        throw new NotImplementedException();
    }

    public override krjGUICanvas getCanvas()
    {
        return this;
    }

    public override void init()
    {
        parent = this;
        mainMenu = new krjGUIMainMenu(getNewId(), this);
        addNode(mainMenu);
        width = canvasHelper.unityCanvas.pixelRect.width;
        height = canvasHelper.unityCanvas.pixelRect.height;
        
        base.init();
    }

    public GUIStyle getStyleMainMenuButtonNormal()
    {
        GUIStyle ret;
        ret = new GUIStyle();

        return ret;
    }

    public override string getLabel()
    {
        return "";
    }

    public krjGUIForm findOrCreateForm<T>() where T : krjGUIForm
    {
        foreach (KeyValuePair<int,krjGUINode> kvp in items)
        {
            if(kvp.Value.GetType() == typeof(T))
            {
                return (T)kvp.Value;
            }
        }

        krjGUIForm newForm = (T)Activator.CreateInstance(typeof(T), new object[] { getNewId(), this });
        addNode(newForm);
        newForm.init();

        return newForm;
    }
}
