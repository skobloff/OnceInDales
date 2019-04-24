using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjGUIPeopleForm : krjGUIForm
{
    public krjGUIDatasource peoples { get; private set; }

    public krjGUIPeopleForm(int _id, krjGUICollection _parent) : base(_id, _parent)
    {
    }

    public override string getLabel()
    {
        return "Людишки";
    }

    public override void init()
    {
        width = 256;
        height = 256;
        left = getCanvas().width / 2 - width / 2;
        top = getCanvas().height / 2 - height / 2;
        currentRect = new Rect(left, top, width, height);
        enableCloseButton = true;
        List<krjCommon> people = getCanvas().canvasHelper.mainCircle.people;

        base.init();
    }

    public override void initItems()
    {
        //датасоурс
        peoples = new krjGUIDatasource(getCanvas().canvasHelper.mainCircle.people);
        dataSources.Add(peoples);

        //кнопки на форме
        krjGUIActionPane pane = addMasterPane();
        krjGUIActionTab actionTab = pane.addTab("Первый таб");
        actionTab.addButton<krjGUICreateHeroMenuItem>(krjMenuItemPreset.ActionPaneBigButton, 7, peoples);

        //грид
        krjGUIGrid grid = new krjGUIGrid(getCanvas().getNewId(), this, peoples);
        addNode(grid);
        grid.addField<krjGUIText>("name");
        grid.addField<krjGUIText>("gender");
        /*
        krjGUIText pole1 = new krjGUIText(getCanvas().getNewId(), grid, null, "name");
        grid.addNode(pole1);
        krjGUIText pole2 = new krjGUIText(getCanvas().getNewId(), grid, null, "gender");
        grid.addNode(pole2);
        */
    }
}
