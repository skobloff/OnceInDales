using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjActFindPeople : krjActBase
{
    private bool isPeopleFind;
    private krjActSelector selector;

    public krjActFindPeople(krjMainCircle _mainCircle) : base(_mainCircle)
    {
    }

    public override krjActCloseStatus close()
    {
        mainCircle.stopRunner(krjActSelector.getKey());
        mainCircle.canvasHelper.krjCanvas.setChildVisible(true);//надо открыть окна
        krjPoint point = selector.lastPoint;
        mainCircle.people.Clear();
        if (isPeopleFind)
        {
            for (int i = 1; i <= 87; i++)
            {
                mainCircle.people.Add(new krjHuman("чел " + i.ToString(), "пол", point.x, point.y));
            }
            mainCircle.canvasHelper.krjCanvas.findOrCreateForm<krjGUIPeopleForm>();
        }
        return krjActCloseStatus.Ok;
    }

    public override void init()
    {
        selector = (krjActSelector)mainCircle.launchRunner<krjActSelector>(krjActSelector.getKey());
        mainCircle.canvasHelper.krjCanvas.setChildVisible(false);//надо закрыть окна
        selector.setParams(new krjPoint(5, 5), new krjPoint(2, 2), null, true);
        isPeopleFind = false;
    }

    public override krjActUpdateStatus update()
    {
        krjActUpdateStatus ret = krjActUpdateStatus.Next;

        if (Input.GetMouseButton(0))
        {
            isPeopleFind = true;
            ret = krjActUpdateStatus.End;
        }
        
        return ret;
    }
}

