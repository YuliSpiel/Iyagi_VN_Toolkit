using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public class LogData
{
    public string Name;
    public string Dialogue;
    public string VFX;
    public int Index;
}

[Serializable]
public class LogJsonList
{
    public List<LogData> Logs;

    public LogJsonList()
    {
        Logs = new List<LogData>(); // 리스트 초기화
    }
}