using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjForbiddenMapHelper {

    private SortedList<string, byte[,]> map;
    private List<string> names;
    private int mapSize;

    public krjForbiddenMapHelper(int _mapSize, params string[] _names)
    {
        mapSize = _mapSize;
        map = new SortedList<string, byte[,]>();
        names = new List<string>();
        foreach (string name in _names)
        {
            byte[,] buf = new byte[mapSize, mapSize];
            map.Add(name, buf);
            names.Add(name);
        }
    }

    public bool isLegal(krjPoint p)
    {
        bool ret = true;
        if (p.isLegal(mapSize))
        {
            foreach (string name in names)
            {
                byte[,] buf;
                if (map.TryGetValue(name, out buf))
                {
                    if (buf[p.x, p.y] == 1)
                    {
                        ret = false;
                    }
                }
            }
        }
        else
        {
            ret = false;
        }
        return ret;
    }

    public void setPoint(krjPoint p, string name)
    {
        byte[,] buf;
        if (map.TryGetValue(name, out buf))
        {
            buf[p.x, p.y] = 1;
        }
    }

    public void setPointSquad(krjPoint p1, krjPoint p2, string name, byte value = 1)
    {
        byte[,] buf;
        if (map.TryGetValue(name, out buf))
        {
            for (int x = p1.x; x <= p2.x; x++)
                for (int y = p1.y; y <= p2.y; y++)
                    buf[x, y] = value;
        }
    }

    public void setPointFromCenter(krjPoint p, int distance, string name)
    {
        int startX = p.x - distance;
        int endX = p.x + distance;
        int startY = p.y - distance;
        int endY = p.y + distance;

        if (startX < 0) startX = 0;
        if (startY < 0) startY = 0;
        if (endX >= mapSize) endX = mapSize - 1;
        if (endY >= mapSize) endY = mapSize - 1;

        setPointSquad(new krjPoint(startX, startY), new krjPoint(endX, endY), name);
    }

}
