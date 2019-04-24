using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjGUIDatasource
{
    private List<krjCommon> selectedData;
    private List<krjGUIDataFilter> systemFilters;
    private List<krjGUIDataFilter> userFilters;
    public List<krjCommon> source { get; private set; }

    public int currentNum { get; set; }

    public int pageSize { get; set; }
    public int countPage { get; set;}
    public int currentPage { get; set; }
    public int currentPageSize { get; set; }

    public Int64 selectedRecId { get; set; }

    public bool needExecuteQuery { private set; get; }
    public bool needReread { private set; get; }

    public krjGUIDatasource (List<krjCommon> _source)
    {
        source = _source;
    }

    public void setFilters(List<krjGUIDataFilter> _filters)
    {
        userFilters = _filters;
    }

    public virtual void init()
    {
        setNeedExecuteQuery();
    }

    public string getText (string propName, int num)
    {
        krjCommon buf = selectedData[num];
        return buf.GetType().GetProperty(propName)?.GetValue(buf)?.ToString();
    }

    public string getText(string propName)
    {
        krjCommon buf = selectedData[currentNum];
        return buf.GetType().GetProperty(propName)?.GetValue(buf)?.ToString();
    }

    public Int64 getRecId(int num)
    {
        krjCommon buf = selectedData[num];
        
        return (Int64)buf.GetType().GetProperty("recId")?.GetValue(buf);
    }

    public void setNeedExecuteQuery()
    {
        needExecuteQuery = true;
    }

    public void setNeedReread()
    {
        needReread = true;
    }

    public void executeQuery()
    {
        List<krjCommon> systemSelectedData;
        List<krjCommon> userSelectedData;
        List<krjCommon> sortedData;

        if (!needExecuteQuery) return;

        //1. select system filter
        if (isFilterEmpty(systemFilters))
        {
            systemSelectedData = source;
        }
        else
        {
            systemSelectedData = source.FindAll(x => filtering(systemFilters, x));
        }

        //2. select user filter
        if (isFilterEmpty(userFilters))
        {
            userSelectedData = systemSelectedData;
        }
        else
        {
            userSelectedData = systemSelectedData.FindAll(x => filtering(userFilters, x));
        }

        //3. sorting
        sortedData = userSelectedData;
        
        //4. page select
        if (pageSize == 0) pageSize = 1;
        countPage = sortedData.Count / pageSize;
        int lastPageSize = sortedData.Count % pageSize;

        if (lastPageSize > 0) countPage++;

        if (currentPage > countPage) currentPage = 1;
        if (currentPage < 1) currentPage = 1;

        currentPageSize = pageSize;
        if (currentPage == countPage && lastPageSize > 0) currentPageSize = lastPageSize;

        selectedData = sortedData.GetRange((currentPage - 1) * pageSize, currentPageSize);
        currentNum = 0;

        needExecuteQuery = false;
    }

    public void reread()
    {
        if (!needReread) return;

        needReread = false;
    }

    private bool isFilterEmpty(List<krjGUIDataFilter> _filter)
    {
        if (_filter == null) return true;

        foreach (krjGUIDataFilter f in _filter)
        {
            if (f.value != null && f.value != "")
            {
                return false;
            }
        }
        return true;
    }

    private bool filtering(List<krjGUIDataFilter> _filters, krjCommon _common)
    {
        foreach (krjGUIDataFilter f in systemFilters)
        {
            if (_common.GetType().GetProperty(f.name)?.GetValue(_common)?.ToString().Contains(f.value) == true)
                return false;
        }
        return true;
    }

    public krjCommon findRecId(Int64 _recid)
    {
        for(int I=0; I<source.Count; I++)
        {
            if (_recid == (Int64)source[I].GetType().GetProperty("recId")?.GetValue(source[I]))
            {
                return source[I];
            }
        }
        return null;
    }

}
