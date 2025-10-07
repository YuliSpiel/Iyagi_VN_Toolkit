using System;
using System.IO;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    public string filePath;
    public LogJsonList logList = new LogJsonList();
    public static LogManager Instance = null;

    private void Awake()
    {
        filePath = Path.Combine(Application.dataPath, "Resources/JSON/Data/LogJson.json");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        if (!File.Exists(filePath))
        {
            Debug.LogWarning("JSON file not found. Loading default file from Resources.");
            //LoadFromResourcesAndSave();
        }

        LoadJson();
    }

    private void LoadJson()
    {
        try
        {
            string filePath = Path.Combine(Application.dataPath, "Resources/JSON/Data/LogJson.json");

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                logList = JsonUtility.FromJson<LogJsonList>(dataAsJson);
                Debug.Log("JSON 파일 로드 완료: " + filePath);
            }
            else
            {
                Debug.LogWarning("JSON 파일이 존재하지 않습니다. 새 파일을 생성합니다.");
                logList = new LogJsonList();
                LogToJsonSave();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("JSON 로드 실패: " + ex.Message);
            logList = new LogJsonList();
            LogToJsonSave();
        }
    }

    public void AddLog(string name, string dialogue, string vfx, int index)
    {
        LogData newLog = new LogData { Name = name, Dialogue = dialogue, VFX = vfx, Index = index };
        logList.Logs.Add(newLog);
        LogToJsonSave();
    }

    public void LogToJsonSave()
    {
        try
        {
            string filePath = Path.Combine(Application.dataPath, "Resources/JSON/Data/LogJson.json");
            string updatedJson = JsonUtility.ToJson(logList, true);

            File.WriteAllText(filePath, updatedJson);
            Debug.Log("JSON 파일이 저장되었습니다: " + filePath);
        }
        catch (Exception ex)
        {
            Debug.LogError("JSON 저장 실패: " + ex.Message);
        }
    }

}
