using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public class StatData
{
    public Stats Stats;
    public int Sanity;
    public int EXP;
    public int Chapter;
    public int SceneNum;
    public int index;
}

[Serializable]
public class Stats
{
    public int Yeom;
    public int Sinche;
    public int Jineung;
    public int Insung;
    public int Jikkam;
    public int Luck;
}

[Serializable]
public class StatJsonList
{
    public List<StatData> Items = new List<StatData>();
}
