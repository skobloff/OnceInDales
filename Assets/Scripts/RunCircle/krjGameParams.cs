using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class krjGameParams
{
    public int mapSize;
    public int heightsMapSize;
    public int townCount;
    public int segmentCount;
    public int firstTownOffset;
    public int segmentSize;
    public float firstLakeHeight;
    public float secondLakeHeight;
    public float dHeight;
    public string fileNameSave;

    public void init()
    {
        if (mapSize == 0) mapSize = 512;
        if (heightsMapSize == 0) heightsMapSize = 1025;
        if (townCount == 0) townCount = 8;
        if (segmentCount == 0) segmentCount = 16;
        if (firstTownOffset == 0) firstTownOffset = 16;
        if (segmentSize == 0) segmentSize = 32;
        if (firstLakeHeight == 0) firstLakeHeight = 0.11F;
        if (secondLakeHeight == 0) secondLakeHeight = 0.099F;
        if (dHeight == 0) dHeight = 0.2F;
        if (fileNameSave == "" || fileNameSave == null) fileNameSave = "OnceInDales.sav";
    }
}
