using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionData
{
    public int TypingSpeed;
    public int TypingAlpha;
    public int TypingSkipSpeed;
    public int BGMVolume;
    public int SFXVolume;
    public int SashaVoiceVolume;
    public int EdanVoiceVolume;
    public int SionVoiceVolume;
}

[System.Serializable]
public class AllObjectJson
{
    public List<object> Illusts = new List<object>();
    public List<object> Backgrounds = new List<object>();
    public List<object> Keywords = new List<object>();
    public List<Dictionary<string, bool>> Endings = new List<Dictionary<string, bool>>();
    public List<OptionData> Options = new List<OptionData>();
}
