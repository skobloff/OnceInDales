using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjGUICreateHeroMenuItem : krjGUIMenuItem
{
    public krjGUICreateHeroMenuItem(int _id, krjGUICollection _parent, krjMenuItemPreset _preset, int _imageNum, krjGUIDatasource _dataSource) : 
        base(_id, _parent, _preset, _imageNum, _dataSource)
    {
    }

    public override string getLabel()
    {
        return "Create hero";
    }

    public override void run()
    {
        if (dataSource.selectedRecId != 0)
        {
            krjHuman people = (krjHuman)dataSource.findRecId(dataSource.selectedRecId);
            krjHero hero = new krjHero(people.name, people.gender, people.x, people.y);
            hero.transformObject = getCanvas().canvasHelper.mainCircle.createHero(getCanvas().canvasHelper.mainCircle.resources.prefabHero, people.x, people.y);
            hero.transformObject.gameObject.AddComponent<BoxCollider>();
            getCanvas().canvasHelper.mainCircle.heroes.Add(hero);
           
        }
    }
}
