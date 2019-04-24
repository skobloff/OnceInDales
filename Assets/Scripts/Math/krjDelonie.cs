using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct dTri
{
    public int numP1;
    public int numP2;
    public int numP3;
    public bool complite;
    public Vector2 c;
    public float r2;

    public dTri (int _numP1, int _numP2, int _numP3, Vector2 _c, float _r2)
    {
        numP1 = _numP1;
        numP2 = _numP2;
        numP3 = _numP3;
        complite = false;
        c = _c;
        r2 = _r2;
    }
}

public struct dEdge
{
    public int a;
    public int b;
    public dEdge (int _a, int _b)
    {
        a = _a;
        b = _b;
    }
}


public class delonie
{
    private List<Vector3> source;
    public List<dTri> result;
    private int DebugCount = 0;

    public delonie(List<Vector3> _source)
    {
        source = _source;
        result = new List<dTri>();
    }

    public void run()
    {
        List<dEdge> edges = new List<dEdge>();

        createSuperStructure();

        for (int i = 0; i < (source.Count - 3); i++)
        {
            edges.Clear();
            int triNum = 0;
            do
            {
                if (inCircle(source[i], result[triNum]))
                {
                    edges.Add(new dEdge(result[triNum].numP1, result[triNum].numP2));
                    edges.Add(new dEdge(result[triNum].numP2, result[triNum].numP3));
                    edges.Add(new dEdge(result[triNum].numP3, result[triNum].numP1));
                    result.RemoveAt(triNum);
                }
                else
                {
                    triNum++;
                }

            } while (triNum < result.Count);

            int edgeFirst = 0;
            int firstIncrement = 0;
            while (edgeFirst < edges.Count)
            {
                firstIncrement = 1;
                int edgeSecond = edgeFirst + 1;
                while (edgeSecond < edges.Count)
                {
                    if ((edges[edgeFirst].a == edges[edgeSecond].a && edges[edgeFirst].b == edges[edgeSecond].b) || 
                        (edges[edgeFirst].b == edges[edgeSecond].a && edges[edgeFirst].a == edges[edgeSecond].b))
                    {
                        edges.RemoveAt(edgeSecond);
                        edges.RemoveAt(edgeFirst);
                        firstIncrement = 0;
                        break;
                    }
                    else
                    {
                        edgeSecond++;
                    }
                }
                edgeFirst = edgeFirst + firstIncrement;
            }

            foreach(dEdge e in edges)
            {
                result.Add(makeTri(e.a, e.b, i));
            }
        }

        int j = 0;
        
        while(j < result.Count)
        {
            if (result[j].numP1 >= (source.Count - 3) || result[j].numP2 >= (source.Count - 3) || result[j].numP3 >= (source.Count - 3))
            {
                result.RemoveAt(j);
            }
            else
            {
                j++;
            }
        }
        
    }

    private bool inCircle(Vector2 _p, dTri _triangle)
    {
        return _triangle.r2 > (ff(_p.x - _triangle.c.x) + ff(_p.y - _triangle.c.y));
    }

    private void createSuperStructure()
    {
        Vector2 min = new Vector2();
        Vector2 max = new Vector2();
        Vector2 delta;
        Vector2 mid;
        bool firstPass = true;

        foreach (Vector2 v in source)
        {
            if (firstPass)
            {
                min = v;
                max = v;
                firstPass = false;
            }
            if (min.x > v.x) min.x = v.x;
            if (min.y > v.y) min.y = v.y;
            if (max.x < v.x) max.x = v.x;
            if (max.x < v.x) max.x = v.x;
        }
        delta = max - min;
        mid = new Vector2(min.x + delta.x / 2, min.y + delta.y / 2);
        float dmax = delta.x;
        if (dmax < delta.y) dmax = delta.y;

        source.Add(new Vector3(mid.x - 2 * dmax, mid.y - dmax, 0));
        source.Add(new Vector3(mid.x, mid.y + 2 * dmax, 0));
        source.Add(new Vector3(mid.x + 2 * dmax, mid.y - dmax, 0));
        result.Add(makeTri(source.Count - 3, source.Count - 2, source.Count - 1));
    }


    private dTri makeTri(int _nA, int _nB, int _nC)
    {
        int buf;

        if (Q(source[_nA], source[_nB], source[_nC]) > 0)
        {
            buf = _nC;
            _nC = _nA;
            _nA = buf;
        }

        float d = 2 * (source[_nA].x * (source[_nB].y - source[_nC].y) +
            source[_nB].x * (source[_nC].y - source[_nA].y) +
            source[_nC].x * (source[_nA].y - source[_nB].y));

        float a2 = ff(source[_nA].x) + ff(source[_nA].y);
        float b2 = ff(source[_nB].x) + ff(source[_nB].y);
        float c2 = ff(source[_nC].x) + ff(source[_nC].y);
        float bcy = source[_nB].y - source[_nC].y;
        float cay = source[_nC].y - source[_nA].y;
        float aby = source[_nA].y - source[_nB].y;
        float cbx = source[_nC].x - source[_nB].x;
        float acx = source[_nA].x - source[_nC].x;
        float bax = source[_nB].x - source[_nA].x;

        Vector2 c = new Vector2((a2 * bcy + b2 * cay + c2 * aby) / d, (a2 * cbx + b2 * acx + c2 * bax) / d);
        float r2 = ff(source[_nA].x - c.x) + ff(source[_nA].y - c.y);
        return new dTri(_nA, _nB, _nC, c, r2);
    }

    float Q(Vector2 _a, Vector2 _b, Vector2 _p)
    {
        Vector2 v1 = _b - _a;
        Vector2 v2 = _p - _b;
        return v1.x * v2.y - v2.x * v1.y;
    }

    private float ff(float f)
    {
        return f * f;
    }
}
