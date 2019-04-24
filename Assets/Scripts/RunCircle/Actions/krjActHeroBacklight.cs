using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjActHeroBacklight : krjActBase
{
    private Int64 currentHeroRecId;
    private krjHero currentHero;
    private Transform heroBacklight;

    public krjActHeroBacklight(krjMainCircle _mainCircle) : base(_mainCircle)
    {
    }

    public override krjActUpdateStatus update()
    {
        if (currentHeroRecId != mainCircle.selectedHero)
        {
            currentHeroRecId = mainCircle.selectedHero;
            currentHero = (krjHero)krjCommon.findRecId(mainCircle.heroes, currentHeroRecId);
        }

        if (currentHero != null)
        {
            if (heroBacklight == null)
            {
                heroBacklight = UnityEngine.Object.Instantiate(mainCircle.resources.prefabHeroBacklight,
                    new Vector3(currentHero.x - 256 + 0.5F, mainCircle.terrain.heights[currentHero.y * 2 + 1, currentHero.x * 2 + 1] * 10.0F, currentHero.y - 256 + 0.5F),
                    Quaternion.Euler(-90,0,0));
            }
            else
            {
                heroBacklight.transform.position = new Vector3(currentHero.x - 256 + 0.5F, mainCircle.terrain.heights[currentHero.y * 2 + 1, currentHero.x * 2 + 1] * 10.0F, currentHero.y - 256 + 0.5F);
            }
        }

        return krjActUpdateStatus.Next;
    }

    public static string getKey()
    {
        return typeof(krjActHeroBacklight).ToString();
    }

}
