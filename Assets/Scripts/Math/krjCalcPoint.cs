using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct krjCalcPoint
{
    public float weight;
    public float value;

    public krjCalcPoint(float _weight)
    {
        weight = _weight;
        value = 0;
    }

    public krjCalcPoint(float _weight, float _value)
    {
        weight = _weight;
        value = _value;
    }
}
