using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjPoint : IComparer
{
    public int x { set; get; }
    public int y { set; get; }

    public Vector2 vector2()
    {
        return new Vector2(x + 0.5F, y + 0.5F);
    }

    public Vector3 vector3(float z)
    {
        return new Vector3(x + 0.5F, y + 0.5F, z);
    }

    public krjPoint(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public int num(int mapSize)
    {
        if (x < 0 || x > mapSize || y < 0 || y > mapSize)
        {
            return -1;
        }
        else
        {
            return y * mapSize + x;
        }
    }

    public int Compare(krjPoint x, krjPoint y)
    {
        if (x.x < y.x)
        {
            return -1;
        }
        else
        {
            if (x.x > y.x)
            {
                return 1;
            }
            else
            {
                if (x.y < x.y)
                {
                    return -1;
                }
                else
                {
                    if (x.y > y.y)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }

    public bool isLegal(int _mapSize)
    {
        return (x >= 0) && (x < _mapSize) && (y >= 0) && (y < _mapSize);
    }

    int IComparer.Compare(object x, object y)
    {
        return (Compare((krjPoint)x, (krjPoint)y));
    }
}
