using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjActSelector : krjActBase
{
    private bool frameOn;
    private float[,,] alphaBuffer;
    public krjPoint lastPoint { get; private set; }
    private krjPoint size;
    private krjPoint activePoint;
    private krjForbiddenMapHelper forbiddenMap;

    public krjActSelector (krjMainCircle _mainCircle) : base (_mainCircle) {}

    // Use this for initialization
    void Start ()
    {
        frameOn = false;
        size = new krjPoint(1, 1);
        activePoint = new krjPoint(0, 0);
        lastPoint = new krjPoint(0, 0);
    }

    public override krjActCloseStatus close()
    {
        if(frameOn)
        {
            pushFrame(alphaBuffer);
        }
        return krjActCloseStatus.Ok;
    }

    // Update is called once per frame
    public override krjActUpdateStatus update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Collider c = mainCircle.terrain.terrainLocal.GetComponent<Collider>();
        if (c.Raycast(ray, out hit, Mathf.Infinity))
        {
            krjPoint newPoint = new krjPoint((int)(hit.point.x + 256), (int)(hit.point.z + 256));
            if (!frameOn || newPoint.x != lastPoint.x || newPoint.y != lastPoint.y)
            {
                if (frameOn)
                    pushFrame(alphaBuffer);

                lastPoint = newPoint;

                if (lastPoint.x - activePoint.x < 0
                    || lastPoint.x + size.x - activePoint.x > mainCircle.gameParamsHelper.gameParams.mapSize
                    || lastPoint.y - activePoint.y < 0
                    || lastPoint.y + size.y - activePoint.y > mainCircle.gameParamsHelper.gameParams.mapSize)
                {
                    frameOn = false;
                }
                else
                {
                    alphaBuffer = loadFrame();
                    drawMouseBox();
                    frameOn = true;
                }
            }
        }
        return krjActUpdateStatus.Next;
    }

    public bool setParams(krjPoint _size, krjPoint _activePoint, krjForbiddenMapHelper _forbiddenMap, bool _autoStart = false)
    {
        if (_size.x < 0 
            || _size.x > mainCircle.gameParamsHelper.gameParams.mapSize / 2 
            || _size.y < 0 
            || _size.y > mainCircle.gameParamsHelper.gameParams.mapSize / 2
            || _activePoint.x < 0
            || _activePoint.x > _size.x - 1
            || _activePoint.y < 0
            || _activePoint.y > _size.y - 1)
        {
            Debug.Log("Ошибка селектора: " + _size.x.ToString() + " - " + 
                _size.y.ToString() + " ; " + 
                _activePoint.x.ToString() + " - " + 
                _activePoint.y.ToString());
            return false;
        }
        if (frameOn)
        {
            pushFrame(alphaBuffer);
            frameOn = false;
        }
        size = _size;
        activePoint = _activePoint;
        forbiddenMap = _forbiddenMap;
        return true;
    }

    void drawMouseBox()
    {
        float[,,] alphaBufferLocal = loadFrame();
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
            {
                for (int i = 0; i < krjTerrainHelper.countAlpha; i++)
                {
                    if (i != (int)krjTerrainTexture.BOX && i != (int)krjTerrainTexture.FORBIDDEN_BOX)
                    {
                        alphaBufferLocal[x, y, i] = alphaBufferLocal[x, y, i] * 0.5F;
                    }
                }
                if (forbiddenMap != null)
                {
                    if(forbiddenMap.isLegal(new krjPoint(lastPoint.x - activePoint.x + x, lastPoint.y - activePoint.y + y)))
                    {
                        alphaBufferLocal[x, y, (int)krjTerrainTexture.BOX] = 0.5F;
                    }
                    else
                    {
                        alphaBufferLocal[x, y, (int)krjTerrainTexture.FORBIDDEN_BOX] = 0.5F;
                    }
                }
                else
                {
                    alphaBufferLocal[x, y, (int)krjTerrainTexture.BOX] = 0.5F;
                }
            }

        pushFrame(alphaBufferLocal);
    }

    void pushFrame(float[,,] _alphaBuffer)
    {
        mainCircle.terrain.terrainLocal.terrainData.SetAlphamaps(lastPoint.x - activePoint.x, lastPoint.y - activePoint.y, _alphaBuffer);
    }

    float[,,] loadFrame()
    {
        return mainCircle.terrain.terrainLocal.terrainData.GetAlphamaps(lastPoint.x - activePoint.x, lastPoint.y - activePoint.y, size.y, size.x);
    }

    public static string getKey()
    {
        return typeof(krjActSelector).ToString();
    }
}
