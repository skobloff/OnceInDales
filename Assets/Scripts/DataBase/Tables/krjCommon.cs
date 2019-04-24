using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class krjCommon
{
    public Int64 recId { get; private set; }
    public abstract Int64 getNewRecId();

    public krjCommon ()
    {
        recId = getNewRecId();
    }

    public static krjCommon findRecId(List<krjCommon> _source, Int64 _recId)
    {
        for (int I = 0; I < _source.Count; I++)
        {
            if (_recId == (Int64)_source[I].GetType().GetProperty("recId")?.GetValue(_source[I]))
            {
                return _source[I];
            }
        }
        return null;
    }
}
