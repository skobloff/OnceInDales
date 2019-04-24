using System;
using System.Collections.Generic;
using UnityEngine;

public class krjMainCircle : MonoBehaviour
{
    public Camera mainCamera;
    public krjTerrainHelper terrain;
    public krjCanvasHelper canvasHelper;
    public krjParamsHelper gameParamsHelper;
    public krjGUIResourceHelper resources;
    
    private List<krjActBase> actions;
    public int actionCount { get { return actions.Count; } }
    private krjActBase currentAction;

    private SortedList<string, krjActBase> runners;
    public int runnerCount { get { return runners.Count; } }

    public Int64 selectedHero { get; set; }

    //Tables
    public List<krjCommon> people;
    public List<krjCommon> heroes;

    const int currentFileVersion = 1;
    int selectorDemoNumber = 0;

	// Use this for initialization
	void Start ()
    {
        terrain.krjInit();
        actions = new List<krjActBase>();
        runners = new SortedList<string, krjActBase>();

        //поиск файла сохранения
        if (!terrain.fileLoad())
        {
            //генерация
            terrain.krjGenerate();
        }
        people = new List<krjCommon>();
        heroes = new List<krjCommon>();
    }
	
    public krjActBase launchAction<T>() where T : krjActBase
    {
        krjActBase action = (T)Activator.CreateInstance(typeof(T), new object[] { this });
        actions.Add(action);
        return action;
    }

    public krjActBase launchRunner<T>(string _key) where T : krjActBase
    {
        krjActBase runner;
        if (runners.TryGetValue(_key, out runner))
        {
            return runner;
        }
        else
        {
            runner = (T)Activator.CreateInstance(typeof(T), new object[] { this });
            runners.Add(_key, runner);
            return runner;
        }
    }

    public void stopRunner(string _key)
    {
        krjActBase runner;
        if (runners.TryGetValue(_key, out runner))
        {
            runner.close();
            runners.Remove(_key);
        }
    }

    // Update is called once per frame
    private void Update ()
    {
        //выполняем текущее дейтсвие        
        if (actionCount == 0 && currentAction == null)
        {
            launchAction<krjActIdle>();
        }

        if (actions.Count > 0 && currentAction == null)
        {
            currentAction = actions[0];
            currentAction.init();
        }

        if (currentAction != null)
        {
            switch (currentAction.update())
            {
                case krjActUpdateStatus.End:
                    currentAction.close();
                    currentAction = null;
                    actions.RemoveAt(0);
                    break;
            }
        }

        foreach (KeyValuePair<string,krjActBase> runner in runners)
        {
            switch (runner.Value.update())
            {
                case krjActUpdateStatus.End:
                    runner.Value.close();
                    runners.Remove(runner.Key);
                    break;
            }
        }

    }

    public Transform createHero(Transform _prefab, int _x, int _y)
    {
        return Instantiate(_prefab, new Vector3(_x-256+0.5F,terrain.heights[_y*2+1, _x*2+1]* 10.0F,_y-256+0.5F), Quaternion.identity);
    }

    
}
