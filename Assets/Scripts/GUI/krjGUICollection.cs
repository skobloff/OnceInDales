using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OrganizationChild
{
    None,
    Vertical,
    Horizontal
}

public abstract class krjGUICollection : krjGUINode
{
    public SortedList <int, krjGUINode> items;
    public float childCurrentLeft;
    public float childCurrentTop;
    public float childCurrentRight;
    public float childCurrentBottom;

    public krjGUICollection(int _id, krjGUICollection _parent, krjGUIDatasource _dataSource = null, string _fieldName = "") : 
        base(_id, _parent, _dataSource, _fieldName)
    {
        items = new SortedList<int, krjGUINode>();
    }

    public override void init()
    {
        base.init();
        foreach (KeyValuePair<int,krjGUINode> item in items)
        {
            item.Value.init();
        }
    }

    public override void draw()
    {
        if (!visible)
            return;
        foreach (KeyValuePair<int, krjGUINode> item in items)
        {
            item.Value.draw();
        }
    }

    public virtual void addNode(krjGUINode _node)
    {
        items.Add(_node.id, _node);
    }

    public virtual void delNode(int _id)
    {
        if (items.ContainsKey(_id))
        {
            krjGUINode node;
            if (items.TryGetValue(_id, out node))
            {
                node.clear();
                items.Remove(_id);
            }
        }
    }

    internal override void clear()
    {
        base.clear();
        foreach (KeyValuePair<int, krjGUINode> item in items)
        {
            item.Value.clear();
        }
        items.Clear();
    }

    public void setChildVisible(bool _visible)
    {
        foreach(KeyValuePair<int, krjGUINode> kvp in items)
        {
            kvp.Value.visible = _visible;
        }
    }

    public abstract float getChildCurrentLeft();
    public abstract float getChildCurrentTop();
}
