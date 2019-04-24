using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AutoNoYes : sbyte
{
    Auto = -1,
    No = 0,
    Yes = 1
}

public abstract class krjGUINode
{
    public int id;
    public GUIStyle style;
    public GUIContent content;
    public int imageNum;
    public bool layout { get; set; }

    public bool visible { get; set; }

    protected AutoNoYes active;
    protected AutoNoYes enable;
    protected krjGUICollection parent;
    public float left;
    public float top;
    public float height;
    public float width;
    public Rect currentRect;
    public string label;
    public krjGUIDatasource dataSource { get; set; }
    public string fieldName { get;  set;}

    public krjGUINode(int _id, krjGUICollection _parent, krjGUIDatasource _dataSource = null, string _fieldName = "")
    {
        id = _id;
        parent = _parent;
        imageNum = -1;
        label = getLabel();
        dataSource = _dataSource;
        fieldName = _fieldName;
        visible = true;
        layout = false;
    }


    public virtual krjGUICanvas getCanvas()
    {
        return parent.getCanvas();
    }

    public virtual void init()
    {
        content = new GUIContent();
        if (imageNum > -1)
        {
            content.image = getCanvas().canvasHelper.mainCircle.resources.images[imageNum];
        }
        content.text = label;
    }

    public void move(float _dLeft, float _dTop)
    {
        currentRect = new Rect(currentRect.x + _dLeft, currentRect.y + _dTop, currentRect.width, currentRect.height);
    }

    public abstract string getLabel();

    public virtual AutoNoYes getActive()
    {
        if(active == AutoNoYes.Auto)
        {
            return parent.getActive();
        }
        return active;
    }

    internal virtual void clear() {}

    public virtual AutoNoYes getEnable()
    {
        if (enable == AutoNoYes.Auto)
        {
            return parent.getEnable();
        }
        return enable;
    }

    public float calcLeft()
    {
        if (left == (int)AutoNoYes.Auto)
        {
            return parent.getChildCurrentLeft();
        }
        else
        {
            return parent.calcLeft() + left;
        }
    }

    public float calcTop()
    {
        if (top == (int)AutoNoYes.Auto)
        {
            return parent.getChildCurrentTop();
        }
        else
        {
            return parent.calcTop() + top;
        }
    }

    public virtual string takeData(string _fieldName)
    {
        if(dataSource == null)
        {
            return parent.takeData(_fieldName);
        }
        else
        {
            return dataSource.getText(_fieldName);
        }
    }

    public virtual string takeData()
    {
        return takeData(fieldName);
    }

    public virtual void draw()
    {
        if (!visible)
            return;
        if (layout)
        {
            layoutDraw();
        }
        else
        {
            normalDraw();
        }
    }

    public virtual void normalDraw() { } //необходимо переопределять этот метод чтобы элемент прорисовывался
    public virtual void layoutDraw() { } //необходимо переопределять этот метод чтобы элемент прорисовывался
}
