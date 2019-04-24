using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjMathf
{
    public static bool inTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 p)
    {
        return (Q(a, b, p) >= 0) && (Q(b, c, p) >= 0) && (Q(c, a, p) >= 0);
    }

    public static float Q(Vector2 _a, Vector2 _b, Vector2 _p)
    {
        return _p.x * (_b.y - _a.y) + _p.y * (_a.x - _b.x) + _a.y * _b.x - _a.x * _b.y;
    }

    public static float[,] bloor(float[,] source, int mapSize, float sigma, int windowHalfSize)
    {


        float[,] ret = new float[mapSize, mapSize];
        float s2 = 2 * sigma * sigma;
        int windowSize = 2 * windowHalfSize + 1;
        float[] window = new float[windowSize];
        int x;
        int y;
        int i;
        float sum;
        int k;
        int l;
        float pix;

        for (i = 1; i <= windowHalfSize; i++)
        {
            window[windowHalfSize - i] = Mathf.Exp(-1.0F * (float)((i) * (i)) / s2);
            window[windowHalfSize + i] = window[windowHalfSize - i];
        }
        window[windowHalfSize] = 1;

        for (y = 0; y < mapSize; y++)
        {
            for (x = 0; x<mapSize; x++)
            {
                sum = 0;
                pix = 0;
                for(k = 0; k < windowSize; k++)
                {
                    l = x + k - windowHalfSize;
                    if (l >= 0 && l < mapSize)
                    {
                        pix = pix + source[l, y] * window[k];
                        sum = sum + window[k];
                    }
                }
                ret[x, y] = pix / sum;
            }
        }

        for (x = 0; x < mapSize; x++)
        {
            for (y = 0; y < mapSize; y++)
            {
                sum = 0;
                pix = 0;
                for (k = 0; k < windowSize; k++)
                {
                    l = y + k - windowHalfSize;
                    if (l >= 0 && l < mapSize)
                    {
                        pix = pix + ret[x, l] * window[k];
                        sum = sum + window[k];
                    }
                }
                ret[x, y] = pix / sum;
            }
        }

        return ret;
    }


}
