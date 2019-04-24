using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjGUIGrid : krjGUICollection
{
    private int currentDrawLine;
    public int maxPageSize { get; set; }
    public int currentPageSize { get; set; }
    public bool selectable { get; set; }

    public krjGUIGrid(int _id, krjGUICollection _parent, krjGUIDatasource _dataSource, bool _selectable = true) : base(_id, _parent, _dataSource)
    {
        maxPageSize = 5;
        selectable = _selectable;
    }

    public override void init()
    {
        base.init();
        dataSource.pageSize = maxPageSize;
    }

    public void addField<T>(string _fieldName)
    {
        krjGUINode newField = (krjGUINode)Activator.CreateInstance(typeof(T),
            new object[] { getCanvas().getNewId(), this, null, _fieldName });
        newField.layout = true;
        addNode(newField);
    }

    public override void draw()
    {
        //currentRect = GUI.Window(id, currentRect, soul, label);
        GUILayout.BeginVertical();
        for (int i = 0; i < dataSource.currentPageSize; i++)
        {
            GUILayout.BeginHorizontal();
            currentDrawLine = i;
            if (selectable)
            {
                bool curBool = false;
                if (dataSource.selectedRecId == dataSource.getRecId(currentDrawLine))
                {
                    curBool = true;
                }
                if (GUILayout.Toggle(curBool, ""))
                {
                    dataSource.selectedRecId = dataSource.getRecId(currentDrawLine);
                }
            }
            base.draw();
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        //тут надо нарисовать кнопки смены страницы
        if (dataSource.currentPage > 3) //рисуем кнопку первой страницы
        {
            if (GUILayout.Button("1"))
            {
                dataSource.currentPage = 1;
                dataSource.setNeedExecuteQuery();
            }
            GUILayout.Space(10);
        }
        int beginNum = dataSource.currentPage - 2;
        if (beginNum < 1) beginNum = 1;
        int endNum = dataSource.currentPage + 2;
        if (endNum > dataSource.countPage) endNum = dataSource.countPage;
        for(int curentNum = beginNum; curentNum <= endNum; curentNum++)
        {
            if (GUILayout.Button(curentNum.ToString()))
            {
                dataSource.currentPage = curentNum;
                dataSource.setNeedExecuteQuery();
            }
        }
        if (dataSource.currentPage < dataSource.countPage - 2) //рисуем кнопку последней страницы
        {
            GUILayout.Space(10);
            if (GUILayout.Button(dataSource.countPage.ToString()))
            {
                dataSource.currentPage = dataSource.countPage;
                dataSource.setNeedExecuteQuery();
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    public override string takeData(string _fieldName)
    {
        return dataSource.getText(_fieldName, currentDrawLine);
    }

    public override string getLabel()
    {
        return "жопа002";
    }

    public override float getChildCurrentLeft()
    {
        throw new NotImplementedException();
    }

    public override float getChildCurrentTop()
    {
        throw new NotImplementedException();
    }
}
