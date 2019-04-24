using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class krjParamsHelper : MonoBehaviour
{
    public string fileNameParams;
    public GameObject waterBasicDaytime;
    public krjGameParams gameParams { private set;  get; }

    public void Awake()
    {
        //*** настройки игры ***
        XmlSerializer formatter = new XmlSerializer(typeof(krjGameParams)); // передаем в конструктор тип класса
        if (File.Exists(fileNameParams)) //поиск файла настроек
        {
            using (krjFileStreamExt fs = new krjFileStreamExt(fileNameParams, FileMode.OpenOrCreate)) // десериализация
                gameParams = (krjGameParams)formatter.Deserialize(fs);
        }
        else
        {
            gameParams = new krjGameParams();
        }
        gameParams.init(); //инициализация параметров, которые были не инициализированы
        using (krjFileStreamExt fs = new krjFileStreamExt(fileNameParams, FileMode.OpenOrCreate)) //переписывание фафла параметров поверх
            formatter.Serialize(fs, gameParams);
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject createWater(Vector2 min, Vector2 max)
    {
        Vector2 vd = max - min; 
        Vector2 v2 = min + vd / 2;
        Vector3 vv = new Vector3(v2.x - gameParams.mapSize / 2, (gameParams.secondLakeHeight + 0.01F) * 10, v2.y - gameParams.mapSize / 2);
        GameObject go = Instantiate(waterBasicDaytime,  vv, Quaternion.AngleAxis(0.0F, Vector3.up));
        go.transform.localScale = new Vector3(vd.x, 0, vd.y);
        //go.transform.localScale = new Vector3(10, 0, 10);
        return go;
    }
}
