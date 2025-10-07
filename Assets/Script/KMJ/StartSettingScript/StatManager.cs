using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StatManager : MonoBehaviour
{
    private string filePath;
    public StatJsonList statJsonList;
    public static StatManager Instance = null;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "StatJson");

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

        // 파일이 없으면 StreamingAssets에서 복사
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("JSON file not found. Copying default file from StreamingAssets.");
            CopyFromStreamingAssets();
        }

        // JSON 파일 로드
        LoadJson();
    }

    private void CopyFromStreamingAssets()
    {
        string sourcePath = Path.Combine(Application.streamingAssetsPath, "StatJson.json");

        if (File.Exists(sourcePath))
        {
            File.Copy(sourcePath, filePath);
            Debug.Log("Default JSON file copied to persistent data path.");
        }
        else
        {
            Debug.LogError("Default JSON file not found in StreamingAssets.");
        }
    }

    private void LoadJson()
    {
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            statJsonList = JsonUtility.FromJson<StatJsonList>(dataAsJson);
        }
        else
        {
            Debug.LogError("Failed to load JSON file. Creating default values.");

            // 기본값 설정
            StatData defaultData = new StatData
            {
                Stats = new Stats
                {
                    Yeom = 0,
                    Sinche = 0,
                    Jineung = 0,
                    Insung = 0,
                    Jikkam = 0,
                    Luck = 0
                },
                Sanity = 0,
                EXP = 0,
                Chapter = 0,
                SceneNum = 0,
                index = 0
            };

            statJsonList = new StatJsonList();
            statJsonList.Items.Add(defaultData);

            StatSave(); // 파일 저장
        }
    }

    public void StatSave()
    {
        // JSON 파일에 반영
        string updatedJson = JsonUtility.ToJson(statJsonList, true);
        File.WriteAllText(filePath, updatedJson);

        Debug.Log("JSON file saved to: " + filePath);
    }
}