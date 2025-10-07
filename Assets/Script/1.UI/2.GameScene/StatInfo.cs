using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatInfo : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public Button HomeBtn;

    private void Awake()
    {
        txt.text = $"챕터 : {GameManager.Instance.Chapter.CurChapter} " +
                   $"{DataManager.Instance.ChapterNames[GameManager.Instance.Chapter.CurChapter]}\n" +
                   $"챕터 진행도: {GameManager.Instance.Chapter.CurIndex}\n\n" +
                   $"염력 : {GameManager.Instance.Stats.Yeom}\n" +
                   $"신체 : {GameManager.Instance.Stats.Sinche}\n" +
                   $"지능 : {GameManager.Instance.Stats.Jineung}\n" +
                   $"인성 : {GameManager.Instance.Stats.Insung}\n" +
                   $"직감 : {GameManager.Instance.Stats.Jikkam}\n" +
                   $"행운 : {GameManager.Instance.Stats.Luck}\n\n" +
                   $"이성치 : {GameManager.Instance.Stats.Sanity}\n";
        
        HomeBtn.onClick.AddListener(OpenTitle);
    }

    public void OpenTitle()
    {
        SceneManager.LoadScene(0);
    }
}
