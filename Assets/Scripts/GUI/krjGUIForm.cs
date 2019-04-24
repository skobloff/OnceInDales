using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CLRTB : sbyte
{
    None = 0,
    Left = 1,
    Top = 2,
    Right = 3,
    Bottom = 4,
    Center = 5
}

public class krjGUIForm : krjGUICollection
{
    public List<krjGUIDatasource> dataSources;
    private krjGUICanvas guiCanvas;
    public CLRTB dockType;
    public const int headerHeight = 16;
    public bool enableCloseButton;
    

    public krjGUIForm(int _id, krjGUICollection _parent) : base(_id, _parent)
    {
        dataSources = new List<krjGUIDatasource>();
    }

    public override void init()
    {
        initItems();
        dataSources.ForEach(dataSource => dataSource.init());
        base.init();
    }

    public override float getChildCurrentLeft()
    {
        throw new NotImplementedException();
    }

    public override float getChildCurrentTop()
    {
        throw new NotImplementedException();
    }

    //метод просто хранит код который жалко выбросить
    /*
    public override void draw()
    {
        float leftLocal;
        float topLocal;
        float widthLocal;
        float heightLocal;

        switch (dockType)
        {
            case LTRBC.None:
                leftLocal = calcLeft();
                topLocal = calcTop();
                break;
            case LTRBC.Top:
                leftLocal = parent.childCurrentLeft;
                topLocal = parent.childCurrentTop;
                widthLocal = parent.childCurrentRight - parent.childCurrentLeft;
                heightLocal = savedCalcHeight - headerHeight;
                parent.childCurrentTop += heightLocal;
                break;
            case LTRBC.Left: 
                leftLocal = parent.childCurrentLeft;
                topLocal = parent.childCurrentTop;
                widthLocal = savedCalcWidht;
                heightLocal = parent.childCurrentBottom - parent.childCurrentTop;
                parent.childCurrentLeft += widthLocal;
                break;
            case LTRBC.Right:
                leftLocal = parent.childCurrentRight - savedCalcWidht;
                topLocal = parent.childCurrentTop;
                widthLocal = savedCalcWidht;
                heightLocal = parent.childCurrentBottom - parent.childCurrentTop;
                parent.childCurrentRight = leftLocal;
                break;
            case LTRBC.Bottom:
                leftLocal = parent.childCurrentLeft;
                topLocal = parent.childCurrentBottom - savedCalcHeight + headerHeight;
                widthLocal = parent.childCurrentRight - parent.childCurrentLeft;
                heightLocal = savedCalcHeight - headerHeight;
                parent.childCurrentBottom = topLocal;
                break;
        }

    }
    */

    public override void draw()
    {
        if (!visible)
            return;
        foreach (krjGUIDatasource d in dataSources)
        {
            d.executeQuery();
            d.reread();
        }

        currentRect = GUI.Window(id, currentRect, internalDraw, label);
    }

    public void internalDraw(int _id)
    {
        if (_id == id)
        {
            if (enableCloseButton)
            {
                if (GUI.Button(new Rect(currentRect.width - 28,4,24,24), "X"))
                {
                    this.close();
                }
            }
            GUILayout.BeginVertical();
            foreach (KeyValuePair<int, krjGUINode> item in items)
            {
                item.Value.draw();
            }
            GUILayout.EndVertical();
            if (dockType == CLRTB.None)
            {
                GUI.DragWindow();
            }
        }
    }

    public virtual void initItems() //метод в котором нужно создавать все вложенные элементы
    { }

    public krjGUIActionPane addMasterPane()
    {
        krjGUIActionPane pane;

        pane = new krjGUIActionPane(getCanvas().getNewId(), this);
        addNode(pane);
        return pane;
    }

    public void close()
    {
        getCanvas().delNode(id);
    }

    public override string getLabel()
    {
        throw new NotImplementedException();
    }
}
