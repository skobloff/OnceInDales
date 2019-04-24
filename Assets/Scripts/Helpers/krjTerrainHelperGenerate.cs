using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class krjTerrainHelper : MonoBehaviour
{
    public void krjGenerate()
    {
        const string fsTown = "town";
        const string fsLake = "lake";
        const string fsMountain = "mountain";

        List<Vector3> sourceList = new List<Vector3>();
        List<Vector2> resultList = new List<Vector2>();
        krjForbiddenMapHelper fmh = new krjForbiddenMapHelper(gameParams.mapSize, fsTown, fsLake, fsMountain);

        krjMapHelper mh = new krjMapHelper(gameParams.mapSize, fmh);

        List<krjPoint> firstLakes = new List<krjPoint>();
        List<krjPoint> firstMountains = new List<krjPoint>();

        //защищаем место под город
        for (int x = 0; x < gameParams.segmentCount; x++)
            for (int y = 0; y < gameParams.segmentCount; y++)
            {
                fmh.setPointSquad(new krjPoint((int)((x + 0.5) * gameParams.segmentSize - 3), (int)((y + 0.5) * gameParams.segmentSize - 3)),
                    new krjPoint((int)((x + 0.5) * gameParams.segmentSize + 1), (int)((y + 0.5) * gameParams.segmentSize + 1)), fsTown);
            }

        //генерим первую гору и первое озеро в каждом сегменте
        for (int x = 0; x < gameParams.segmentCount; x++)
            for (int y = 0; y < gameParams.segmentCount; y++)
            {
                mh.clearLocalMap(1);
                mh.fillLocalMap(new krjPoint(x * gameParams.segmentSize + 1, y * gameParams.segmentSize + 1),
                    new krjPoint((x + 1) * gameParams.segmentSize - 2, (y + 1) * gameParams.segmentSize - 2), 1, true);
                mh.calsSecondLevel();
                krjPoint p = mh.rndPointFromLocalMap(fsMountain);
                firstMountains.Add(p);
                sourceList.Add(p.vector3(gameParams.firstLakeHeight + gameParams.dHeight));
                fmh.setPointFromCenter(p, 6, fsMountain);
                mh.calsSecondLevel();
                p = mh.rndPointFromLocalMap(fsLake);
                firstLakes.Add(p);
                sourceList.Add(p.vector3(gameParams.firstLakeHeight));
            }

        //Debug.Log(sourceList.Count);
        delonie delo = new delonie(sourceList);
        delo.run();
        //Debug.Log(delo.result.Count);

        //создаем первичные регионы с треугольниками
        krjMapRegion region;
        for (int i = 0; i < delo.result.Count; i++)
        {
            region = addRegion(krjRegionType.Dale, "Безымянная долина",
                                    new krjTriangle(
                                    sourceList[delo.result[i].numP1],
                                    sourceList[delo.result[i].numP2],
                                    sourceList[delo.result[i].numP3]));
        }

        //считаем рельеф карты в регионах
        foreach (krjMapRegion r in regions)
        {
            for (float x = r.t.min.x; x <= r.t.max.x; x = x + 0.5F)
                for (float y = r.t.min.y; y <= r.t.max.y; y = y + 0.5F)
                {
                    Vector2 p = new Vector2(x, y);
                    if (r.t.internalPoint(p))
                    {
                        float f = r.t.calcPointZ(p);
                        heights[(int)(x * 2), (int)(y * 2)] = f;
                    }
                }
        }

        //смягчаем рельеф
        heights = krjMathf.bloor(heights, gameParams.heightsMapSize, 3.0F, 30);

        int pointX;
        int pointY;
        krjPoint pp;
        krjMapPoint mp;
        int pKey;
        //генерим описания клеток карты в регионах
        foreach (krjMapRegion r in regions)
        {
            for (float x = r.t.min.x; x <= r.t.max.x; x = x + 1F)
                for (float y = r.t.min.y; y <= r.t.max.y; y = y + 1F)
                {
                    Vector2 p = new Vector2(x, y);
                    if (r.t.internalPoint(p))
                    {
                        pointX = (int)Mathf.Round(x - 0.5F);
                        pointY = (int)Mathf.Round(y - 0.5F);
                        pp = new krjPoint(pointX, pointY);
                        pKey = pp.num(gameParams.mapSize);
                        if (points.IndexOfKey(pKey) < 0)
                        {
                            mp = new krjMapPoint(pp, krjMapPointType.Slope, r);
                            points.Add(pKey, mp);
                        }
                    }
                }
        }

        //генерим озера
        krjMapHelperFast mhf = new krjMapHelperFast(gameParams.mapSize, fmh);
        mhf.useingHeightsParam(heights, true, true);
        List<krjPoint> buffer = new List<krjPoint>();
        foreach (krjPoint p in firstLakes)
        {
            buffer.Clear();
            buffer.Add(p);
            region = addRegion(krjRegionType.Lake, "Безымянное озеро");
            for (int i = 0; i < 40; i++)
            {
                mhf.clearLocalMap(1);
                foreach (krjPoint ppp in buffer)
                {
                    mhf.fillLocalMapCross(ppp, 1, 1.0F, false);
                }
                mhf.calsSecondLevel();
                krjPoint newP = mhf.rndPointFromLocalMap(fsLake);
                buffer.Add(newP);
                pKey = newP.num(gameParams.mapSize);
                if (points.IndexOfKey(pKey) < 0)
                {
                    mp = new krjMapPoint(newP, krjMapPointType.Water, region);
                    points.Add(pKey, mp);
                }
                else
                {
                    points.TryGetValue(pKey, out mp);
                    mp.changeRegion(region);
                    mp.changeType(krjMapPointType.Water);
                }
            }
        }

        foreach (krjMapRegion r in regions)
        {
            if (r.type == krjRegionType.Lake)
            {
                foreach (KeyValuePair<int, krjMapPoint> kvpMP in r.points)
                {
                    makeWater(r, kvpMP.Value);
                    r.createWaterObject();
                    fmh.setPointFromCenter(kvpMP.Value.p, 1, fsLake);
                }
            }
        }

        mhf.useingHeightsParam(heights, true, false);
        fmh.setPointSquad(new krjPoint(0, 0), new krjPoint(gameParams.mapSize - 1, gameParams.mapSize - 1), fsMountain, 0);
        //генерим горы
        foreach (krjPoint p in firstMountains)
        {
            fmh.setPoint(p, fsMountain);
        }

        foreach (krjPoint p in firstMountains)
        {
            buffer.Clear();
            buffer.Add(p);
            region = addRegion(krjRegionType.Mountain, "Безымянная гора");
            for (int i = 0; i < 40; i++)
            {
                mhf.clearLocalMap(1);
                foreach (krjPoint ppp in buffer)
                {
                    mhf.fillLocalMapPoint(ppp, 2, 1.0F, false);
                }
                mhf.calsSecondLevel();
                krjPoint newP = mhf.rndPointFromLocalMap(fsMountain);
                buffer.Add(newP);
                pKey = newP.num(gameParams.mapSize);
                if (points.IndexOfKey(pKey) < 0)
                {
                    mp = new krjMapPoint(newP, krjMapPointType.Mountain, region);
                    points.Add(pKey, mp);
                }
                else
                {
                    points.TryGetValue(pKey, out mp);
                    mp.changeRegion(region);
                    mp.changeType(krjMapPointType.Mountain);
                }
            }
        }

        foreach (krjMapRegion r in regions)
        {
            if (r.type == krjRegionType.Mountain)
            {
                foreach (KeyValuePair<int, krjMapPoint> kvpMP in r.points)
                {
                    makeMountain(r, kvpMP.Value);
                }
            }
        }

        //генерим деревья
        for (int x = 0; x < gameParams.segmentCount; x++)
            for (int y = 0; y < gameParams.segmentCount; y++)
            {
                mhf.clearLocalMap(1);
                mhf.fillLocalMap(new krjPoint(x * gameParams.segmentSize, y * gameParams.segmentSize),
                    new krjPoint((x + 1) * gameParams.segmentSize, (y + 1) * gameParams.segmentSize), 1, true);
                mhf.calsSecondLevel();
                for (int t = 0; t < 80; t++)
                {
                    mhf.calsSecondLevel();
                    krjPoint newP = mhf.rndPointFromLocalMap(fsTown);
                    pKey = newP.num(gameParams.mapSize);
                    if(points.TryGetValue(pKey, out mp))
                    {
                        mp.makeTree();                   
                    }
                    mhf.fillLocalMapPoint(newP, 3, 10, false);
                }
            }
        fileSave();
    }

    private void fileSave()
    {
        // запись в файл
        using (krjFileStreamExt fstream = new krjFileStreamExt(fileNameSave, FileMode.OpenOrCreate))
        {
            //версия файла
            fstream.writeInt(1);
            //размеркарты
            fstream.writeInt(paramsHelper.gameParams.heightsMapSize);
            //карта высот
            for (int x = 0; x < paramsHelper.gameParams.heightsMapSize; x++)
                for (int y = 0; y < paramsHelper.gameParams.heightsMapSize; y++)
                {
                    fstream.writeFloat(heights[x, y]);
                }
            //количество регионов
            fstream.writeInt(regions.Count);
            //регионы
            foreach(krjMapRegion region in regions)
            {
                //номер региона
                fstream.writeInt(region.number);
                //Debug.Log(string.Format("Регион номер запись: {0}", region.number));
                //тип региона
                fstream.writeInt((int)region.type);
                //имя региона
                fstream.writeStr(region.name);
            }
            //количество клеточек карты
            fstream.writeInt(points.Count);
            //клеточки карты
            foreach (KeyValuePair<int,krjMapPoint> kvp in points)
            {
                //индекс точки
                fstream.writeInt(kvp.Key);
                //номер региона
                fstream.writeInt(kvp.Value.region.number);
                //Debug.Log(string.Format("Регион номер запись в точке: {0}", kvp.Value.region.number));
                //поинт
                fstream.writeInt(kvp.Value.p.x);
                fstream.writeInt(kvp.Value.p.y);
                //тип точки
                fstream.writeInt((int)kvp.Value.type);
                //есть дорога
                fstream.writeInt(kvp.Value.haveRoad ? 1 : 0);
                //есть деревья
                fstream.writeInt(kvp.Value.haveTree ? 1 : 0);
                //высота гор
                fstream.writeInt(kvp.Value.mountainHeight);
            }
        }
    }

    public bool fileLoad()
    {
        int regionNumber;
        int regionType;
        string regionName;

        int pointKey;
        int pointRegionNumber;
        int pointX, pointY;
        int pointType;
        int pointHaveRoad;
        int pointHaveTree;
        int pointMountainHeight;
        krjMapPoint point;
        int K, findReg;
           

        // проверяем существует ли файл
        if (!File.Exists(fileNameSave))
        {
            return false;
        }
        using (krjFileStreamExt fstream = new krjFileStreamExt(fileNameSave, FileMode.Open))
        {
            //версия файла
            if(fstream.readInt() != 1)
            {
                return false;
            }

            //размеркарты
            if (fstream.readInt() !=paramsHelper.gameParams.heightsMapSize)
            {
                return false;
            }

            //карта высот
            for (int x = 0; x < paramsHelper.gameParams.heightsMapSize; x++)
                for (int y = 0; y < paramsHelper.gameParams.heightsMapSize; y++)
                {
                    heights[x, y] = fstream.readFloat();
                }

            //количество регионов
            int regionCount = fstream.readInt();
            //регионы
            for (int I = 1; I<=regionCount; I++)
            {
                //номер региона
                regionNumber = fstream.readInt();
                //Debug.Log(string.Format("Регион номер чтение: {0}", regionNumber));
                //тип региона
                regionType = fstream.readInt();
                //имя региона
                regionName = fstream.readStr();
                addRegion((krjRegionType)regionType, regionName, null, regionNumber);
            }

            //количество клеточек карты
            int pointCount = fstream.readInt();
            //клеточки карты
            for (int J = 1; J <= pointCount; J++)
            {
                //индекс точки
                pointKey = fstream.readInt();
                //номер региона
                pointRegionNumber = fstream.readInt();
                //Debug.Log(string.Format("Регион номер запись в точке: {0}", pointRegionNumber));
                //поинт
                pointX = fstream.readInt();
                pointY = fstream.readInt();
                //тип точки
                pointType = fstream.readInt();
                //есть дорога
                pointHaveRoad = fstream.readInt();
                //есть деревья
                pointHaveTree = fstream.readInt();
                //высота гор
                pointMountainHeight = fstream.readInt();
                findReg = 0;
                for (K=0; K<regions.Count; K++)
                {
                    if (regions[K].number == pointRegionNumber)
                    {
                        findReg = K;
                        break;
                    }
                }
                /*
                if (pointRegionNumber == 1240)
                {
                    Debug.Log(string.Format("Регион найден: {0} {1}", pointRegionNumber, regions[findReg].number));
                }
                */
                point = new krjMapPoint(new krjPoint(pointX, pointY), (krjMapPointType)pointType, regions[findReg]);
                point.haveRoad = pointHaveRoad == 1;
                if (pointHaveTree == 1)
                    point.makeTree();
                point.mountainHeight = pointMountainHeight;
                points.Add(pointKey, point);
                regions[findReg].addPoint(point);
            }
        }
        //пост обработка
        foreach(krjMapRegion r in regions)
        {
            if (r.type == krjRegionType.Lake)
            {
                //Debug.Log(string.Format("Регион найден: {0} {1}", r.number, r.points.Count));
                r.createWaterObject();
                /*foreach (KeyValuePair<int, krjMapPoint> kvpMP in r.points)
                {
                    makeWater(r, kvpMP.Value);
                    //r.createWaterObject();
                }
                */
            }
        }
        heightsUpdate = true;
        aplhaUpdate = true;
        return true;
    }

    private void makeWater(krjMapRegion r, krjMapPoint mp)
    {
        heights[(int)(mp.p.vector2().x * 2), (int)(mp.p.vector2().y * 2)] = gameParams.secondLakeHeight;
        heightsUpdate = true;
        krjPoint pp1 = new krjPoint(mp.p.x - 1, mp.p.y - 1);
        krjPoint pp2 = new krjPoint(mp.p.x, mp.p.y - 1);
        krjPoint pp3 = new krjPoint(mp.p.x + 1, mp.p.y - 1);
        krjPoint pp4 = new krjPoint(mp.p.x - 1, mp.p.y);
        krjPoint pp5 = new krjPoint(mp.p.x, mp.p.y);
        krjPoint pp6 = new krjPoint(mp.p.x + 1, mp.p.y);
        krjPoint pp7 = new krjPoint(mp.p.x - 1, mp.p.y + 1);
        krjPoint pp8 = new krjPoint(mp.p.x, mp.p.y + 1);
        krjPoint pp9 = new krjPoint(mp.p.x + 1, mp.p.y + 1);

        makeCoast(pp1);
        makeCoast(pp2);
        makeCoast(pp3);
        makeCoast(pp4);
        makeCoast(pp6);
        makeCoast(pp7);
        makeCoast(pp8);
        makeCoast(pp9);

        if (r.points.ContainsKey(pp6.num(gameParams.mapSize)))
        {
            heights[mp.p.x * 2 + 2, mp.p.y * 2 + 1] = gameParams.secondLakeHeight;
        }
        else
        {
            heights[mp.p.x * 2 + 2, mp.p.y * 2 + 1] = (heights[mp.p.x * 2 + 2, mp.p.y * 2 + 1] + gameParams.secondLakeHeight) / 2;
        }

        if (r.points.ContainsKey(pp4.num(gameParams.mapSize)))
        {
            heights[mp.p.x * 2, mp.p.y * 2 + 1] = gameParams.secondLakeHeight;
        }
        else
        {
            heights[mp.p.x * 2, mp.p.y * 2 + 1] = (heights[mp.p.x * 2, mp.p.y * 2 + 1] + gameParams.secondLakeHeight) / 2;
        }

        if (r.points.ContainsKey(pp8.num(gameParams.mapSize)))
        {
            heights[mp.p.x * 2 + 1, mp.p.y * 2 + 2] = gameParams.secondLakeHeight;
        }
        else
        {
            heights[mp.p.x * 2 + 1, mp.p.y * 2 + 2] = (heights[mp.p.x * 2 + 1, mp.p.y * 2 + 2] + gameParams.secondLakeHeight) / 2;
        }

        if (r.points.ContainsKey(pp2.num(gameParams.mapSize)))
        {
            heights[mp.p.x * 2 + 1, mp.p.y * 2] = gameParams.secondLakeHeight;
        }
        else
        {
            heights[mp.p.x * 2 + 1, mp.p.y * 2] = (heights[mp.p.x * 2 + 1, mp.p.y * 2] + gameParams.secondLakeHeight) / 2;
        }

        if (r.points.ContainsKey(pp9.num(gameParams.mapSize))
            && r.points.ContainsKey(pp6.num(gameParams.mapSize))
            && r.points.ContainsKey(pp8.num(gameParams.mapSize)))
        {
            heights[mp.p.x * 2 + 2, mp.p.y * 2 + 2] = gameParams.secondLakeHeight;
        }
        else
        {
            heights[mp.p.x * 2 + 2, mp.p.y * 2 + 2] = (heights[mp.p.x * 2 + 2, mp.p.y * 2 + 2] + gameParams.secondLakeHeight) / 2;
        }

        if (r.points.ContainsKey(pp7.num(gameParams.mapSize))
            && r.points.ContainsKey(pp4.num(gameParams.mapSize))
            && r.points.ContainsKey(pp8.num(gameParams.mapSize)))
        {
            heights[mp.p.x * 2, mp.p.y * 2 + 2] = gameParams.secondLakeHeight;
        }
        else
        {
            heights[mp.p.x * 2, mp.p.y * 2 + 2] = (heights[mp.p.x * 2, mp.p.y * 2 + 2] + gameParams.secondLakeHeight) / 2;
        }

        if (r.points.ContainsKey(pp3.num(gameParams.mapSize))
            && r.points.ContainsKey(pp6.num(gameParams.mapSize))
            && r.points.ContainsKey(pp2.num(gameParams.mapSize)))
        {
            heights[mp.p.x * 2 + 2, mp.p.y * 2] = gameParams.secondLakeHeight;
        }
        else
        {
            heights[mp.p.x * 2 + 2, mp.p.y * 2] = (heights[mp.p.x * 2 + 2, mp.p.y * 2] + gameParams.secondLakeHeight) / 2;
        }

        if (r.points.ContainsKey(pp1.num(gameParams.mapSize))
            && r.points.ContainsKey(pp4.num(gameParams.mapSize))
            && r.points.ContainsKey(pp2.num(gameParams.mapSize)))
        {
            heights[mp.p.x * 2, mp.p.y * 2] = gameParams.secondLakeHeight;
        }
        else
        {
            heights[mp.p.x * 2, mp.p.y * 2] = (heights[mp.p.x * 2, mp.p.y * 2] + gameParams.secondLakeHeight) / 2;
        }
    }

    private void makeMountain(krjMapRegion r, krjMapPoint mp)
    {
        heightsUpdate = true;
        int i = 0;
        for (int dx = -1; dx < 2; dx++)
            for (int dy = -1; dy < 2; dy++)
            {
                krjPoint pp = new krjPoint(mp.p.x + dx, mp.p.y + dy);
                if (r.points.ContainsKey(pp.num(gameParams.mapSize)))
                    i++;
            }
        mp.makeMountain(i);
        float f = heights[(int)(mp.p.vector2().x * 2), (int)(mp.p.vector2().y * 2)] + (i + 1) * 0.03F;
        heights[(int)(mp.p.vector2().x * 2), (int)(mp.p.vector2().y * 2)] = f;
        for (int ddx = -1; ddx < 2; ddx++)
            for (int ddy = -1; ddy < 2; ddy++)
            {
                float ff = f * 0.8F + f * 0.1F * Random.value;
                float gg = heights[(int)(mp.p.vector2().x * 2 + ddx), (int)(mp.p.vector2().y * 2 + ddy)];
                if (heights[(int)(mp.p.vector2().x * 2 + ddx), (int)(mp.p.vector2().y * 2 + ddy)] < ff)
                    heights[(int)(mp.p.vector2().x * 2 + ddx), (int)(mp.p.vector2().y * 2 + ddy)] = ff;
            }
    }
    private void makeCoast(krjPoint p)
    {
        if (points.ContainsKey(p.num(gameParams.mapSize)))
        {
            krjMapPoint mp = points.Values[points.IndexOfKey(p.num(gameParams.mapSize))];
            if (mp.type == krjMapPointType.Slope)
            {
                mp.changeType(krjMapPointType.Coast);
                heights[mp.p.x * 2 + 1, mp.p.y * 2 + 1] = (heights[mp.p.x * 2 + 1, mp.p.y * 2 + 1] +
                    (heights[mp.p.x * 2 + 1, mp.p.y * 2 + 1] + gameParams.secondLakeHeight) / 2) / 2;
                heightsUpdate = false;
            }
        }
    }

}
