using System.Collections.Generic;

[System.Serializable]
public class ResultDialogueEntry
{
    public List<DialogueEntry> SuccessText;
    public List<DialogueEntry> FailText;
}

[System.Serializable]
public class ResultDialogueClass
{
    public List<ResultDialogueEntry> ResultDialogue;
}