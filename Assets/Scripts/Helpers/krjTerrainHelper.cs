using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum krjTerrainTexture : int
{
    GRASS = 0, //These numbers depend on the order in which
    SAND = 1, //the textures are loaded onto the terrain
    ROCK = 2,
    ROAD = 3,
    ICE = 4,
    MOH = 5,
    BOX = 6,
    FORBIDDEN_BOX = 7
}

public partial class krjTerrainHelper : MonoBehaviour
{
    public krjParamsHelper paramsHelper;
    private krjGameParams gameParams;
    public Terrain terrainLocal;
    public string fileNameSave;

    private TerrainData tData;

    public float[,] heights;
    private bool heightsUpdate;

    private List<krjMapRegion> regions;
    private int lastRegionNum;
    private SortedList<int, krjMapPoint> points;

    private float[,,] alphaData;
    private bool aplhaUpdate;

    public const int countAlpha = 8;

    private void Awake()  
    {
        regions = new List<krjMapRegion>();
        points = new SortedList<int, krjMapPoint>();
    }

    // Use this for initialization
    void Start ()
    {
        
    }

    public void krjInit()
    {
        int x, y;
        terrainLocal = (Terrain)GameObject.FindObjectOfType(typeof(Terrain));
        heights = new float[paramsHelper.gameParams.heightsMapSize, paramsHelper.gameParams.heightsMapSize];
        for (x = 0; x < paramsHelper.gameParams.heightsMapSize; x++)
            for (y = 0; y < paramsHelper.gameParams.heightsMapSize; y++)
            {
                heights[x, y] = paramsHelper.gameParams.firstLakeHeight;
            }
        heightsUpdate = true;
        tData = Terrain.activeTerrain.terrainData;
        alphaData = tData.GetAlphamaps(0, 0, tData.alphamapWidth, tData.alphamapHeight);
        //Debug.Log(tData.alphamapWidth);
        //Debug.Log(tData.alphamapHeight);
        for (x = 0; x < tData.alphamapWidth; x++)
            for (y = 0; y < tData.alphamapHeight; y++)
            {
                for (int i = 0; i < countAlpha; i++)
                    alphaData[x, y, i] = 0.0F;
                alphaData[x, y, (int)krjTerrainTexture.GRASS] = 1.0F;
            }
        aplhaUpdate = true;
        gameParams = paramsHelper.gameParams;
        List< TreeInstance> TreeInstances = new List<TreeInstance>(terrainLocal.terrainData.treeInstances);
        TreeInstances.Clear();
        terrainLocal.terrainData.treeInstances = TreeInstances.ToArray();
        lastRegionNum = 0;
    }

    private krjMapRegion addRegion(krjRegionType _regionType, string _name, krjTriangle _t = null, int _regionNum = 0)
    {
        int newRegionNum;

        if (_regionNum == 0)
        {
            lastRegionNum++;
            newRegionNum = lastRegionNum;
        }
        else
        {
            newRegionNum = _regionNum;
        }

        krjMapRegion region = new krjMapRegion(this, _regionType, newRegionNum, _t);
        region.name = _name;
        regions.Add(region);
        return region;
    }

    public void setAlpha(krjPoint p, float[] _alpha)
    {
        int i = 0;

        foreach(float f in _alpha)
        {
            if (i >= countAlpha)
            {
                break;
            }
            alphaData[p.x, p.y, i] = f;
            i++;
        }
        aplhaUpdate = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (heightsUpdate)
        {
            tData.SetHeights(0, 0, heights);
            terrainLocal.Flush();
            heightsUpdate = false;
        }
        if (aplhaUpdate)
        {
            tData.SetAlphamaps(0, 0, alphaData);
            aplhaUpdate = false;
        }
    }

    public void reupdate()
    {
        heightsUpdate = true;
        aplhaUpdate = true;
    }

    public krjMapPoint getMapPoint(Vector3 coords)
    {
        krjPoint p = new krjPoint((int)(coords.z + gameParams.mapSize / 2), (int)(coords.x + gameParams.mapSize / 2));
        int key = p.num(gameParams.mapSize);
        if(points.ContainsKey(key))
        {
            return points.Values[points.IndexOfKey(key)];
        }
        else
        {
            return null;
        }
    }

    private void FixedUpdate()
    {

    }
}
