using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjTriangle
{
    public Vector3 a;
    public Vector3 b;
    public Vector3 c;
    public Vector2 min;
    public Vector2 max;
    public Vector3 ab;
    public Vector3 bc;
    public Vector3 ca;
    public krjTriangle triAB;
    public krjTriangle triBC;
    public krjTriangle triCA;
    public byte type;

    public krjTriangle(Vector3 _a, Vector3 _b, Vector3 _c)
    {
        a = _a;
        b = _b;
        c = _c;

        if (_a.z == _b.z && _b.z == _c.z)
        {
            type = 0;
        }
        else
        {
            type = 1;
            if (_a.z == _b.z)
            {
                a = _c;
                b = _a;
                c = _b;
            }
            if (_c.z == _a.z)
            {
                a = _b;
                b = _c;
                c = _a;
            }
        }

        min = new Vector2();
        min.x = a.x;
        if (min.x > b.x) min.x = b.x;
        if (min.x > c.x) min.x = c.x;
        min.y = a.y;
        if (min.y > b.y) min.y = b.y;
        if (min.y > c.y) min.y = c.y;
        max.x = a.x;
        if (max.x < b.x) max.x = b.x;
        if (max.x < c.x) max.x = c.x;
        max.y = a.y;
        if (max.y < b.y) max.y = b.y;
        if (max.y < c.y) max.y = c.y;
        ab = new Vector3((a.x + b.x) / 2, (a.y + b.y) / 2, (a.z + b.z) / 2);
        bc = new Vector3((b.x + c.x) / 2, (b.y + c.y) / 2, (b.z + c.z) / 2);
        ca = new Vector3((c.x + a.x) / 2, (c.y + a.y) / 2, (c.z + a.z) / 2);
    }

    public bool internalPoint(Vector2 p)
    {
        if (p.x >= min.x && p.x <= max.x && p.y >= min.y && p.y <= max.y)
        {
            return krjMathf.inTriangle(a,b,c,p);
        }
        else
        {
            return false;
        }
    }

    public float calcPointZ(Vector2 p)
    {
        if (type == 0)
        {
            return a.z;
        }
        else
        {
            float delitel = (a.x - p.x) * (b.y - c.y) - (a.y - p.y) * (b.x - c.x);
            if (delitel == 0)
                return 0;
            float mnojitel1 = a.x * p.y - a.y * p.x;
            float mnojitel2 = b.x * c.y - b.y * c.x;
            Vector2 o = new Vector2((mnojitel1 * (b.x - c.x) - (a.x - p.x) * mnojitel2) / delitel, (mnojitel1 * (b.y - c.y) - (a.y - p.y) * mnojitel2) / delitel);
            float r1 = Mathf.Sqrt((a.x - p.x) * (a.x - p.x) + (a.y - p.y) * (a.y - p.y));
            float r2 = Mathf.Sqrt((o.x - p.x) * (o.x - p.x) + (o.y - p.y) * (o.y - p.y));
            return c.z + (a.z - c.z) / (r1 + r2) * r2;
        }
    }

    public static void findNeighbors(List<krjTriangle> _tris)
    {
        foreach (krjTriangle t in _tris)
        {
            if (t.triAB == null || t.triBC == null || t.triCA == null)
            {
                foreach (krjTriangle d in _tris)
                {
                    if (t != d)
                    {
                        if (t.triAB == null)
                        {
                            if (t.a == d.b && t.b == d.a)
                            {
                                t.triAB = d;
                                d.triAB = t;
                            }
                            if (t.a == d.c && t.b == d.b)
                            {
                                t.triAB = d;
                                d.triBC = t;
                            }
                            if (t.a == d.a && t.b == d.c)
                            {
                                t.triAB = d;
                                d.triCA = t;
                            }
                        }
                        if (t.triBC == null)
                        {
                            if (t.b == d.b && t.c == d.a)
                            {
                                t.triBC = d;
                                d.triAB = t;
                            }
                            if (t.b == d.c && t.c == d.b)
                            {
                                t.triBC = d;
                                d.triBC = t;
                            }
                            if (t.b == d.a && t.c == d.c)
                            {
                                t.triBC = d;
                                d.triCA = t;
                            }
                        }
                        if (t.triCA == null)
                        {
                            if (t.c == d.b && t.a == d.a)
                            {
                                t.triCA = d;
                                d.triAB = t;
                            }
                            if (t.c == d.c && t.a == d.b)
                            {
                                t.triCA = d;
                                d.triBC = t;
                            }
                            if (t.c == d.a && t.a == d.c)
                            {
                                t.triCA = d;
                                d.triCA = t;
                            }
                        }
                    }
                }
            }
        }
    }
}
