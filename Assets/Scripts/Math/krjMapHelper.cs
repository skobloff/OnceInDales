using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjMapHelper {

    private float[,,] localMap;
    protected krjForbiddenMapHelper fmh;
    public int mapSize { get; private set; }

	public krjMapHelper(int _mapSize, krjForbiddenMapHelper _fmh)
    {
        mapSize = _mapSize;
        localMap = new float[mapSize, mapSize, 2];
        fmh = _fmh; 
    }

    public krjPoint num2point(int num)
    {
        int y = (int)num / mapSize;
        int x = num - y * mapSize;
        return new krjPoint(x, y);
    }

    public virtual krjPoint rndPointFromLocalMap(string forbiddenName)
    {
        krjPoint p;

        float value = Random.value;
        for (int i = 0; i < mapSize * mapSize; i++)
        {
            p = num2point(i);
            if (value < localMap[p.x, p.y, 1])
                return p;
        }
        p = num2point(mapSize * mapSize - 1);
        if (forbiddenName != "")
        {
            fmh.setPoint(p, forbiddenName);
        }
        return p;
    }

    public virtual void calsSecondLevel()
    {
        float sum = 0;

        for (int x = 0; x < mapSize; x++)
            for (int y = 0; y < mapSize; y++)
                sum += localMap[x, y, 0];


        float coef;
        if (sum != 0)
            coef = 1 / sum;
        else
            coef = 0;

        float lastValue = 0.0F;

        for (int i = 0; i < mapSize * mapSize; i++)
        {
            krjPoint p = num2point(i);
            lastValue = lastValue + coef * localMap[p.x, p.y, 0];
            localMap[p.x, p.y, 1] = lastValue;
        }

    }

    public virtual void clearLocalMap(int level = 0)
    {
        for (int l = 0; l <= level; l++)
            for (int x = 0; x < mapSize; x++)
                for (int y = 0; y < mapSize; y++)
                {
                    localMap[x, y, l] = 0;
                }
    }

    public void fillLocalMapPoint(krjPoint p, int distance, float value, bool absolute = true)
    {
        int startX = p.x - distance;
        int endX = p.x + distance;
        int startY = p.y - distance;
        int endY = p.y + distance;

        if (startX < 0) startX = 0;
        if (startY < 0) startY = 0;
        if (endX >= mapSize) endX = mapSize - 1;
        if (endY >= mapSize) endY = mapSize - 1;

        fillLocalMap(new krjPoint(startX, startY), new krjPoint(endX, endY), value, absolute);
    }

    public virtual void fillLocalMapPoint(krjPoint p, float value, bool absolute = true)
    {
        if (fmh.isLegal(p))
        {
            if (absolute)
            {
                localMap[p.x, p.y, 0] = value;
            }
            else
            {
                localMap[p.x, p.y, 0] += value;
            }
        }
    }

    public void fillLocalMapCross(krjPoint p, int distance, float value, bool absolute = true)
    {
        int startX = p.x - distance;
        int endX = p.x + distance;
        int startY = p.y - distance;
        int endY = p.y + distance;

        if (startX < 0) startX = 0;
        if (startY < 0) startY = 0;
        if (endX >= mapSize) endX = mapSize - 1;
        if (endY >= mapSize) endY = mapSize - 1;

        fillLocalMap(new krjPoint(startX, p.y), new krjPoint(endX, p.y), value, absolute);
        fillLocalMap(new krjPoint(p.x, startY), new krjPoint(p.x, endY), value, absolute);
    }

    public void fillLocalMap(krjPoint startP, krjPoint endP, float value, bool absolute = true)
    {
        for (int x = startP.x; x <= endP.x; x++)
            for (int y = startP.y; y <= endP.y; y++)
            {
                fillLocalMapPoint(new krjPoint(x, y), value, absolute);
            }
    }
}
