using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum krjActUpdateStatus
{
    Next,
    End
}

public enum krjActCloseStatus
{
    Next,
    Ok
}

public abstract class krjActBase
{
    public krjMainCircle mainCircle;

    public krjActBase(krjMainCircle _mainCircle)
    {
        mainCircle = _mainCircle;
    }

    public virtual void init() { }

    public abstract krjActUpdateStatus update();

    public virtual krjActCloseStatus close() { return krjActCloseStatus.Ok; }

}
