using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class krjFileStreamExt : FileStream
{
    public krjFileStreamExt (string _name, FileMode _fileMode) : base(_name, _fileMode) { }

    public int readInt()
    {
        byte[] buf = new byte[sizeof(int)];
        Read(buf, 0, sizeof(int));
        return BitConverter.ToInt32(buf, 0);
    }

    public float readFloat()
    {
        byte[] buf = new byte[sizeof(float)];
        Read(buf, 0, sizeof(float));
        return BitConverter.ToSingle(buf, 0);
    }

    public void writeInt(int _data)
    {
        byte[] buf = new byte[sizeof(int)];
        buf = BitConverter.GetBytes(_data);
        Write(buf, 0, sizeof(int));
    }

    public void writeFloat(float _data)
    {
        byte[] buf = new byte[sizeof(float)];
        buf = BitConverter.GetBytes(_data);
        Write(buf, 0, sizeof(float));
    }

    public void writeStr(string _data)
    {
        byte[] array = System.Text.Encoding.Default.GetBytes(_data);
        writeInt(array.Length);
        Write(array, 0, array.Length);
    }

    public string readStr()
    {
        byte[] buf = new byte[sizeof(int)];
        Read(buf, 0, sizeof(int));
        int strLenght = BitConverter.ToInt32(buf, 0);
        //Debug.Log(strLenght);
        buf = new byte[strLenght];
        Read(buf, 0, strLenght);
        return System.Text.Encoding.Default.GetString(buf);
    }
}
