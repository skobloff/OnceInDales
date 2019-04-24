using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjGUIAboutForm : krjGUIForm
{
    public krjGUIAboutForm(int _id, krjGUICollection _parent) : base(_id, _parent)
    {
    }

    public override string getLabel()
    {
        return "Об игре";
    }

    public override void initItems()
    {
        throw new NotImplementedException();
    }
}
