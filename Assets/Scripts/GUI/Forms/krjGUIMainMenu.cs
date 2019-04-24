using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjGUIMainMenu : krjGUIForm
{
    private krjGUIPeopleMenuItem peopleButton;
    private krjGUIAboutMenuItem aboutButton;

    public krjGUIMainMenu(int _id, krjGUICollection _parent) : base(_id, _parent)
    {
    }

    public override string getLabel()
    {
        return "Once in dales : main menu";
    }

    public override void init()
    {
        width = getCanvas().width;
        height = 80;
        base.init();
        currentRect = new Rect(0, 0, getCanvas().width, 140);
        dockType = CLRTB.Top;
        //aboutButton.move(150.0F, 0.0F);
    }

    public override void draw()
    {
        currentRect = new Rect(0, 0, getCanvas().canvasHelper.unityCanvas.pixelRect.width, 140);
        GUI.skin = getCanvas().GeneralSkin;
        base.draw();
    }

    public override void initItems()
    {
        krjGUIActionPane pane = addMasterPane();
        krjGUIActionTab actionTab = pane.addTab("Первый таб");
        actionTab.addButton<krjGUIFindPeopleMenuItem>(krjMenuItemPreset.ActionPaneBigButton, 5);
        actionTab.addButton<krjGUIPeopleMenuItem>(krjMenuItemPreset.ActionPaneBigButton, 1);
        actionTab.addButton<krjGUIAboutMenuItem>(krjMenuItemPreset.ActionPaneBigButton, -1);
        actionTab = pane.addTab("Второй таб");
    }
}
