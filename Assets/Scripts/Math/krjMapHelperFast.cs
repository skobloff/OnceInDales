using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjMapHelperFast : krjMapHelper {

    private SortedList<int, krjCalcPoint> fastLocalMap;
    private float[,] heights;
    private bool useHeights;
    private bool invertedHeigts;

    public int point2num(krjPoint p)
    {
        return p.y * mapSize + p.x;
    }

    public void useingHeightsParam(float[,] h, bool _useHeights, bool _invertedHeigts)
    {
        heights = h;
        useHeights = _useHeights;
        invertedHeigts = _invertedHeigts;
    }

    public krjMapHelperFast(int _mapSize, krjForbiddenMapHelper _fmh) 
        : base(_mapSize, _fmh)
    {
        fastLocalMap = new SortedList<int, krjCalcPoint>();
        useHeights = false;
    }

    public override void calsSecondLevel()
    {
        float sum = 0;

        foreach (krjCalcPoint cp in fastLocalMap.Values)
            sum += cp.weight;


        float coef;
        if (sum != 0)
            coef = 1 / sum;
        else
            coef = 0;

        float lastValue = 0.0F;

        SortedList<int, krjCalcPoint> newList = new SortedList<int, krjCalcPoint>();
        foreach (KeyValuePair<int, krjCalcPoint> kvp in fastLocalMap)
        {
            lastValue = lastValue + coef * kvp.Value.weight;
            newList.Add(kvp.Key, new krjCalcPoint(kvp.Value.weight, lastValue));
        }
        fastLocalMap = newList;
    }

    public override void fillLocalMapPoint(krjPoint p, float value, bool absolute = true)
    {

        if (fmh.isLegal(p))
        {
            if (useHeights)
            {
                if (invertedHeigts)
                {
                    value = value * (1 / (heights[p.x, p.y] * heights[p.x, p.y]));
                }
                else
                {
                    value = value * (heights[p.x, p.y] * heights[p.x, p.y]);
                }
            }
            if (absolute)
            {
                if (fastLocalMap.ContainsKey(point2num(p)))
                {
                    fastLocalMap.Remove(point2num(p));
                }
                fastLocalMap.Add(point2num(p), new krjCalcPoint(value));
            }
            else
            {
                krjCalcPoint cp;
                if (fastLocalMap.TryGetValue(point2num(p), out cp))
                {
                    cp.weight = cp.weight + value * value * 0.5F;
                    if (fastLocalMap.ContainsKey(point2num(p)))
                    {
                        fastLocalMap.Remove(point2num(p));
                    }
                    fastLocalMap.Add(point2num(p), cp);
                }
                else
                {
                    fastLocalMap.Add(point2num(p), new krjCalcPoint(value));
                }
            }
        }
    }

    public override krjPoint rndPointFromLocalMap(string _forbiddenName)
    {
        krjPoint p;

        float value = Random.value;
        foreach(KeyValuePair<int, krjCalcPoint> kvp in fastLocalMap)
        {
            if (value < kvp.Value.value)
            {
                krjPoint buf = num2point(kvp.Key);
                if (_forbiddenName != "")
                {
                    fmh.setPoint(buf, _forbiddenName);
                }
                return buf;
            }
        }
        p = num2point(0);
        if (_forbiddenName != "")
        {
            fmh.setPoint(p, _forbiddenName);
        }
        return p;
    }

    public override void clearLocalMap(int level = 0)
    {
        fastLocalMap.Clear();
    }
}
