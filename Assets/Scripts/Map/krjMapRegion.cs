using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum krjRegionType : byte
{
    None = 0,
    Dale = 1,
    Lake = 2,
    Mountain = 3,
}

public class krjMapRegion
{
    public int number { get; private set; }
    public krjRegionType type;
    public krjTriangle t;
    public SortedList<int, krjMapPoint> points;
    public string name;
    private List<GameObject> gameObjects;
    public krjTerrainHelper terrainHelper;
    private krjParamsHelper paramsHelper;
    private GameObject water;
    private Vector2 min;
    private Vector2 max;

    public krjMapRegion(krjTerrainHelper _terrainHelper, krjRegionType _type, int _number, krjTriangle _t = null)
    {
        t = _t;
        type = _type;
        number = _number;
        terrainHelper = _terrainHelper;
        paramsHelper = terrainHelper.paramsHelper;
        points = new SortedList<int, krjMapPoint>();
        gameObjects = new List<GameObject>();
        if (t == null)
        {
            min = new Vector2(paramsHelper.gameParams.mapSize, paramsHelper.gameParams.mapSize);
            max = new Vector2(-1.0F * paramsHelper.gameParams.mapSize, -1.0F * paramsHelper.gameParams.mapSize);
        }
    }

    public void addPoint(krjMapPoint p)
    {
        int pKey = p.p.num(paramsHelper.gameParams.mapSize);
        if (points.IndexOfKey(pKey) < 0)
        {
            points.Add(pKey, p);
            if (max.x < p.p.vector2().x) max.x = p.p.vector2().x;
            if (max.y < p.p.vector2().y) max.y = p.p.vector2().y;
            if (min.x > p.p.vector2().x) min.x = p.p.vector2().x;
            if (min.y > p.p.vector2().y) min.y = p.p.vector2().y;
        }

        //createWaterObject();
    }

    public void createWaterObject()
    {
        if (type == krjRegionType.Lake)
        {
            if (points.Count > 1 && water == null)
            {
                water = paramsHelper.createWater(min, max);
            }
            if (water != null)
            {
                Vector2 vd = max - min;
                Vector2 v2 = min + vd / 2;
                Vector3 vv = new Vector3(v2.y - paramsHelper.gameParams.mapSize / 2,
                    (paramsHelper.gameParams.secondLakeHeight + 0.009F) * 10,
                    v2.x - paramsHelper.gameParams.mapSize / 2);
                water.transform.position = vv;
                water.transform.localScale = new Vector3(vd.x, 0, vd.y);
            }
        }
    }

    public void setAlpha(krjPoint _p, float[] _aplha)
    {
        terrainHelper.setAlpha(_p, _aplha);
    }

    public void delPoint(krjMapPoint p)
    {
        int pKey = p.p.num(paramsHelper.gameParams.mapSize);
        if (points.IndexOfKey(pKey) > 0)
        {
            points.Remove(pKey);
        }
    }
}
