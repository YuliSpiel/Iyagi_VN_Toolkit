using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StatDetailManager : MonoBehaviour
{
    private string filePath;
    public SelectData selectData;
    public static StatDetailManager Instance = null;

    private void Awake()
    {
        // 저장 경로 설정
        filePath = Path.Combine(Application.persistentDataPath, "SelectStatJson");

        // 싱글톤 패턴 구현
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

        // JSON 파일이 없으면 Resources에서 로드 후 저장
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("JSON file not found. Loading default file from Resources.");
            LoadFromResourcesAndSave();
        }

        // JSON 파일 로드
        LoadJson();
    }

    private void LoadFromResourcesAndSave()
    {
        TextAsset resourceFile = Resources.Load<TextAsset>("JSON/Select/SelectStatJson");

        if (resourceFile != null)
        {
            selectData = JsonUtility.FromJson<SelectData>(resourceFile.text);
            StatDetailSave(); // 파일 저장
            Debug.Log("Default JSON file loaded from Resources and saved to persistent data path.");
        }
        else
        {
            Debug.LogError("Default JSON file not found in Resources.");
            selectData = new SelectData(); // 기본값 생성
            StatDetailSave(); // 기본값 저장
        }
    }

    private void LoadJson()
    {
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            selectData = JsonUtility.FromJson<SelectData>(dataAsJson);
            Debug.Log("JSON file loaded from persistent data path.");
        }
        else
        {
            Debug.LogWarning("Failed to load JSON file. Creating default values.");
            selectData = new SelectData(); // 기본값 생성
            StatDetailSave(); // 기본값 저장
        }
    }

    public void StatDetailSave()
    {
        // JSON 파일에 반영
        string updatedJson = JsonUtility.ToJson(selectData, true);
        File.WriteAllText(filePath, updatedJson);
        Debug.Log("JSON file saved to: " + filePath);
    }
}
