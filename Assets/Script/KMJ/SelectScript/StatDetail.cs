using System.Collections.Generic;


[System.Serializable]
public class StatDetail
{
    public string Before;
    public string LevelText;
    public int LevelNum;
    public string Success;
    public string Fail;
}

[System.Serializable]
public class Stat
{
    public StatDetail Yeom;
    public StatDetail Sinche;
    public StatDetail Jineung;
    public StatDetail Insung;
    public StatDetail Jikkam;
    public StatDetail Luck;
}

[System.Serializable]
public class SelectDetail
{
    public int Chapter;
    public int SelectNumber;
    public int MoveCount;
    public Stat Stat;
}

[System.Serializable]
public class SelectData
{
    public List<SelectDetail> Select;
}
