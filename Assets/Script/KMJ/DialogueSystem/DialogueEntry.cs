using System.Collections.Generic;
using UnityEngine; // Assuming you are using Unity, include this if needed

[System.Serializable]
public class CharacterImage
{
    public string Char;
    public string Position;
    public string Size;
    public string Clothes;
    public string Pose;
    public string Eyes;
    public string Mouth;
    public string Blush;
    public string Scar;
}

[System.Serializable]
public class DialogueEntry
{
    public int Index;
    public string BackgroundImg;
    public string SFX;
    public string Voice;
    public string CharName;
    public int PeopleNum;
    public string AfterDialogue;
    public CharacterImage ImgName;
    public CharacterImage ImgName2;
    public string Dialogue;
}

[System.Serializable]
public class DialogueList
{
    public List<DialogueEntry> dialogues;
}
