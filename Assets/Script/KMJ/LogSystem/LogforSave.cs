using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using System.IO.Ports;
using static UnityEngine.EventSystems.EventTrigger;
using System.Security.Cryptography;
using System.Linq;

public class LogforSave : MonoBehaviour
{
    public static LogforSave Instance;

    public string ChapterNum;
    //Log 관련
    public GameObject logBackground;
    public GameObject logWindow;
    public TextMeshProUGUI logNameText;
    public TextMeshProUGUI logDialogueText;
    public LogManager logManager;
    public bool isLogWindowOn = false;
    public string Logname;
    public string Logdialogue;
    public string LogVFX;
    public int index;
    [SerializeField] GameObject logBox;
    [SerializeField] GameObject logBox_NonName;
    [SerializeField] AudioSource LogVoiceAudioSource;

    private float scrollCooldown = 0.5f;
    private float lastScrollTime = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InputManager.Instance.OnCloseLogWindow += CloseLogWindow;
        InputManager.Instance.OnScroll += HandleScrollInput;
    }

    private void CloseLogWindow()
    {
        logWindow.SetActive(false);
        logBackground.SetActive(false);
        isLogWindowOn = false;
    }

    public void LogSetting()
    {
        //logWindow = GameObject.Find("LogWindow");
        //logNameText = GameObject.Find("CharText").GetComponent<TextMeshProUGUI>();
        //logDialogueText = GameObject.Find("LogText").GetComponent<TextMeshProUGUI>();
        logWindow.SetActive(false);
        logBackground.SetActive(false);
        logManager = GameObject.Find("LogManager").GetComponent<LogManager>();
        index = 0;
    }

    public void HandleScrollInput(int direction)
    {
        Debug.Log("ScrollDirection: " + direction);
        if (Time.time - lastScrollTime < scrollCooldown) return; // 쿨타임 적용
        lastScrollTime = Time.time;

        if (direction < 0) //스크롤 업(-1)
        {
            logBackground.SetActive(true);
            logWindow.SetActive(true);
            isLogWindowOn = true;
        }
        //else if (direction > 0) // 스크롤 다운(1)
        //{
        //    StartCoroutine(DelaySetIsLogWindowOff()); // 딜레이 후 isLogWindowOn을 false로 설정
        //}
    }

    private IEnumerator DelaySetIsLogWindowOff()
    {
        logWindow.SetActive(false);
        logBackground.SetActive(false);
        isLogWindowOn = false;
        yield return new WaitForSeconds(3f); // 0.5초 딜레이
        Debug.Log("Log window state set to false after delay.");
    }


    private void SetVoiceAudio(LogManager logManager, int targetIndex)
    {
        LogData matchingLog = null; // 찾은 데이터를 저장할 변수

        foreach (LogData log in logManager.logList.Logs)
        {
            if (log.Index == targetIndex)
            {
                matchingLog = log;
                break; // 찾았으면 반복문 종료
            }
        }

        if (matchingLog != null)
        {
            Debug.Log("Matching LogData found: " + matchingLog.Name);
        }
        else
        {
            Debug.LogWarning("No matching LogData found for Index " + targetIndex);
        }

        if (matchingLog != null && !string.IsNullOrEmpty(matchingLog.VFX))
        {
            if (LogVoiceAudioSource == null)
            {
                LogVoiceAudioSource = gameObject.AddComponent<AudioSource>();
            }

            AudioClip voiceClip = Resources.Load<AudioClip>($"Sound/Voice/{ChapterNum}/{matchingLog.VFX}");

            Debug.Log("LogVFX is " + matchingLog.VFX);

            LogVoiceAudioSource.clip = voiceClip;
            // 오디오 재생
            LogVoiceAudioSource.Play();
        }
        else
        {
            Debug.LogWarning($"No matching LogData found for Index {targetIndex} or VFX is empty.");
        }
    }

    public void SetVoiceAudio(int targetIndex)
    {
        //Debug.Log("****targetIndex is " + targetIndex);
        SetVoiceAudio(logManager, targetIndex);
    }

    public void SavetoLog()
    {
        //Debug.Log("SavetoLog LogVFX is " + LogVFX);
        logManager.AddLog(Logname, Logdialogue, LogVFX, index);
        //Debug.Log("###Log" + Logname + " " + Logdialogue + " " + LogVFX + " " + index);
        logManager.LogToJsonSave(); // 로그 저장 후 즉시 JSON 파일 업데이트

        GameObject newlogBox;
        TextMeshProUGUI newlogNameText;
        TextMeshProUGUI newlogDialogueText;
        if (!Logname.Equals(""))
        {
            // 생성된 logBox 안에 있는 UI 텍스트를 찾아서 값 복사
            newlogBox = Instantiate(logBox, logWindow.GetComponent<Transform>());
            newlogNameText = newlogBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            newlogNameText.text = Logname;
            newlogDialogueText = newlogBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }
        else
        {
            newlogBox = Instantiate(logBox_NonName, logWindow.GetComponent<Transform>());
            newlogDialogueText = newlogBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        Button newlogBoxButton = newlogBox.GetComponent<Button>();
        // 버튼 클릭 시 현재 index 값을 전달하는 람다식 사용
        int currentIndex = index;
        newlogBoxButton.onClick.AddListener(() => SetVoiceAudio(currentIndex));
        newlogDialogueText.text = Logdialogue + "\n";

        Logname = "";
        Logdialogue = "";
        LogVFX = "";
        SaveDialogue();
        index += 1;
    }

    public void SaveDialogue()
    {
        // DialogueList 객체를 JSON 문자열로 변환
        LogJsonList updatedDialogueList = new LogJsonList();

        string json = JsonUtility.ToJson(updatedDialogueList, true);

        // LogManager에서 사용하는 filePath와 동일하게 설정
        string filePath = logManager.filePath;

        try
        {
            // 파일에 JSON 문자열 쓰기
            File.WriteAllText(filePath, json);
            Debug.Log($"JSON 데이터가 저장되었습니다: {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"JSON 데이터를 저장하는 중 오류가 발생했습니다: {e.Message}");
        }
    }
}
