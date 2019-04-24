using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjActIdle : krjActBase
{
    bool isPeopleFind;

    public krjActIdle(krjMainCircle _mainCircle) : base(_mainCircle)
    {
    }

    public override krjActUpdateStatus update()
    {
        krjActUpdateStatus ret = krjActUpdateStatus.Next;
        if (Input.GetMouseButton(0))
        {
            foreach (krjHero hero in mainCircle.heroes)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Collider c = hero.transformObject.GetComponent<Collider>();
                if (c == null)
                {
                    Debug.Log("Не нашли Collider!");
                }
                else if (c.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Debug.Log("Нашли героя!");
                    mainCircle.selectedHero = hero.recId;
                    mainCircle.launchRunner<krjActHeroBacklight>(krjActHeroBacklight.getKey());
                }
            }
        }
        if(mainCircle.actionCount > 0)
        {
            ret = krjActUpdateStatus.End;
            
        }
        return ret;
    }
}
