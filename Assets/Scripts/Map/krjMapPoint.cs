using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum krjMapPointType : byte
{
    None = 0,
    Slope = 1,
    Coast = 2,
    Water = 3,
    Warm = 4,
    Mountain = 5
}

public class krjMapPoint
{
    public krjMapPointType type;
    public krjPoint p { get; private set; }
    public krjMapRegion region {get; set;}
    public bool haveRoad;
    public bool haveTree;
    public int mountainHeight;

    public krjMapPoint(krjPoint _p, krjMapPointType _type, krjMapRegion _region)
    {
        type = _type;
        p = _p;
        region = _region;
        if (region != null)
        {
            region.addPoint(this);
            region.setAlpha(p, getAlpha());
        }
        haveRoad = false;
        haveTree = false;
        mountainHeight = 0;
    }

    public void changeRegion(krjMapRegion _region)
    {
        if (region != null)
        {
            region.delPoint(this);
        }
        region = _region;
        _region.addPoint(this);
    }

    public void changeType(krjMapPointType _newType)
    {
        if (type != _newType)
        {
            type = _newType;
            if (region != null)
            {
                region.setAlpha(p, getAlpha());
            }
        }
    }

    public void makeRoad()
    {
        if (type == krjMapPointType.Slope)
        {
            haveRoad = true;
            if (region != null)
            {
                region.setAlpha(p, getAlpha());
            }
        }
    }

    public void makeMountain(int m)
    {
        mountainHeight = m;
        if (region != null)
        {
            region.setAlpha(p, getAlpha());
        }
    }

    public void makeTree()
    {
        if (type == krjMapPointType.Slope && haveTree == false)
        {
            TreeInstance treeInstance;
            Vector3 position;
            haveTree = true;
            for (int dx = 0; dx < 2; dx++)
                for (int dy = 0; dy < 2; dy++)
                {
                    treeInstance = new TreeInstance();
                    treeInstance.prototypeIndex = 0;
                    position = new Vector3((p.y + 0.05F + dy*0.5F + 0.4F*Random.value) / 512, 0, (p.x + 0.05F + dx * 0.5F + 0.4F * Random.value) / 512);
                    treeInstance.position = position;
                    treeInstance.heightScale = 0.02F + 0.03F * Random.value;
                    treeInstance.widthScale = treeInstance.heightScale;
                    region.terrainHelper.terrainLocal.AddTreeInstance(treeInstance);
                }
            region.terrainHelper.terrainLocal.Flush();
            if (region != null)
            {
                region.setAlpha(p, getAlpha());
            }
        }
    }

    public float[] getAlpha()
    {
        
        float[] ret = new float[krjTerrainHelper.countAlpha];
        for (int i = 0; i < krjTerrainHelper.countAlpha; i++)
            ret[i] = 0.0F;
        switch(type)
        {
            case krjMapPointType.Mountain:
                if (mountainHeight < 5)
                {
                    ret[(int)krjTerrainTexture.ROCK] = 1.0F;
                }
                else
                {
                    ret[(int)krjTerrainTexture.ICE] = 1.0F;
                }
                break;
            case krjMapPointType.Coast:
            case krjMapPointType.Water:
                float min;
                float max;
                min = region.terrainHelper.heights[p.x * 2, p.y * 2];
                max = region.terrainHelper.heights[p.x * 2, p.y * 2];
                for (int dx = 0; dx < 3; dx ++)
                    for (int dy = 0; dy < 3; dy ++)
                    {
                        if (min > region.terrainHelper.heights[p.x * 2 + dx, p.y * 2 + dy]) min = region.terrainHelper.heights[p.x * 2 + dx, p.y * 2 + dy];
                        if (max < region.terrainHelper.heights[p.x * 2 + dx, p.y * 2 + dy]) max = region.terrainHelper.heights[p.x * 2 + dx, p.y * 2 + dy];
                    }
                if ((max - min) > 0.015F)
                {
                    ret[(int)krjTerrainTexture.ROCK] = 1.0F;
                }
                else
                {
                    ret[(int)krjTerrainTexture.SAND] = 1.0F;
                }
                break;
            default:
                if (haveRoad)
                {
                    ret[(int)krjTerrainTexture.ROAD] = 1.0F;
                }
                else
                {
                    if (haveTree)
                    {
                        ret[(int)krjTerrainTexture.MOH] = 1.0F;
                    }
                    else
                    {
                        ret[(int)krjTerrainTexture.GRASS] = 1.0F;
                    }
                }
                break;
        }
        return ret;
    }

}
