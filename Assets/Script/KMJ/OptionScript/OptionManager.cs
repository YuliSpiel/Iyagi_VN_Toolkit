using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using Unity.VisualScripting;

public class OptionManager : MonoBehaviour
{
    [SerializeField] int _TypingSpeed;
    [SerializeField] int _TypingAlpha;
    [SerializeField] int _TypingSkipSpeed;

    [SerializeField] int _BGMVolume;
    [SerializeField] int _SFXVolume;

    [SerializeField] int _SashaVoiceVolume;
    [SerializeField] int _EdanVoiceVolume;
    [SerializeField] int _SionVoiceVolume;

    // 슬라이더 UI 변수
    public Slider typingSpeedSlider;
    public Slider typingAlphaSlider;
    public Slider typingSkipSpeedSlider;
    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider sashaVoiceVolumeSlider;
    public Slider edanVoiceVolumeSlider;
    public Slider sionVoiceVolumeSlider;

    public static OptionManager Instance = null;

    private string jsonFilePath;

    private void Awake()
    {
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

        jsonFilePath = "Assets/Resources/JSON/Data/AllObjectJson.json";  // JSON 파일 경로 설정
    }

    // Start 메서드에서 슬라이더 값 초기화
    void Start()
    {
        LoadOptions(); // 게임 시작 시 옵션 로드
        InitializeSliders(); // 슬라이더 값 초기화
        RegisterSliderListeners(); // 슬라이더 이벤트 리스너 등록
    }

    void InitializeSliders()
    {
        typingSpeedSlider.value = TypingSpeed;
        typingAlphaSlider.value = TypingAlpha;
        typingSkipSpeedSlider.value = TypingSkipSpeed;
        bgmVolumeSlider.value = BGMVolume;
        sfxVolumeSlider.value = SFXVolume;
        sashaVoiceVolumeSlider.value = SashaVoiceVolume;
        edanVoiceVolumeSlider.value = EdanVoiceVolume;
        sionVoiceVolumeSlider.value = SionVoiceVolume;
    }

    void RegisterSliderListeners()
    {
        typingSpeedSlider.onValueChanged.AddListener(OnTypingSpeedChanged);
        typingAlphaSlider.onValueChanged.AddListener(OnTypingAlphaChanged);
        typingSkipSpeedSlider.onValueChanged.AddListener(OnTypingSkipSpeedChanged);
        bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        sashaVoiceVolumeSlider.onValueChanged.AddListener(OnSashaVoiceVolumeChanged);
        edanVoiceVolumeSlider.onValueChanged.AddListener(OnEdanVoiceVolumeChanged);
        sionVoiceVolumeSlider.onValueChanged.AddListener(OnSionVoiceVolumeChanged);
    }

    // 슬라이더 값이 변경되었을 때 호출될 메서드들
    public void OnTypingSpeedChanged(float value) { TypingSpeed = Mathf.RoundToInt(value); SaveOptions(); }
    public void OnTypingAlphaChanged(float value) { TypingAlpha = Mathf.RoundToInt(value); SaveOptions(); }
    public void OnTypingSkipSpeedChanged(float value) { TypingSkipSpeed = Mathf.RoundToInt(value); SaveOptions(); }
    public void OnBGMVolumeChanged(float value) { BGMVolume = Mathf.RoundToInt(value); SaveOptions(); }
    public void OnSFXVolumeChanged(float value) { SFXVolume = Mathf.RoundToInt(value); SaveOptions(); }
    public void OnSashaVoiceVolumeChanged(float value) { SashaVoiceVolume = Mathf.RoundToInt(value); SaveOptions(); }
    public void OnEdanVoiceVolumeChanged(float value) { EdanVoiceVolume = Mathf.RoundToInt(value); SaveOptions(); }
    public void OnSionVoiceVolumeChanged(float value) { SionVoiceVolume = Mathf.RoundToInt(value); SaveOptions(); }

    // 옵션 값 저장
    void SaveOptions()
    {
        OptionData optionsData = new OptionData
        {
            TypingSpeed = TypingSpeed,
            TypingAlpha = TypingAlpha,
            TypingSkipSpeed = TypingSkipSpeed,
            BGMVolume = BGMVolume,
            SFXVolume = SFXVolume,
            SashaVoiceVolume = SashaVoiceVolume,
            EdanVoiceVolume = EdanVoiceVolume,
            SionVoiceVolume = SionVoiceVolume
        };

        AllObjectJson data = new AllObjectJson();
        data.Options.Clear();
        data.Options.Add(optionsData); // 새 옵션 값을 추가

        string json = JsonUtility.ToJson(data, true); // JSON으로 직렬화
        File.WriteAllText(jsonFilePath, json); // 파일에 저장
    }

    // 옵션 값 로드
    void LoadOptions()
    {
        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath); // 파일에서 읽기
            AllObjectJson data = JsonUtility.FromJson<AllObjectJson>(json); // JSON 역직렬화

            OptionData optionsData = data.Options.Count > 0 ? data.Options[0] : new OptionData();
            TypingSpeed = optionsData.TypingSpeed;
            TypingAlpha = optionsData.TypingAlpha;
            TypingSkipSpeed = optionsData.TypingSkipSpeed;
            BGMVolume = optionsData.BGMVolume;
            SFXVolume = optionsData.SFXVolume;
            SashaVoiceVolume = optionsData.SashaVoiceVolume;
            EdanVoiceVolume = optionsData.EdanVoiceVolume;
            SionVoiceVolume = optionsData.SionVoiceVolume;
        }
    }

    public int TypingSpeed
    {
        get { return _TypingSpeed; }
        set { _TypingSpeed = Math.Clamp(value, 0, 100); }
    }

    public int TypingAlpha
    {
        get { return _TypingAlpha; }
        set { _TypingAlpha = Math.Clamp(value, 0, 100); }
    }

    public int TypingSkipSpeed
    {
        get { return _TypingSkipSpeed; }
        set { _TypingSkipSpeed = Math.Clamp(value, 0, 100); }
    }


    public int BGMVolume
    {
        get { return _BGMVolume; }
        set { _BGMVolume = Math.Clamp(value, 0, 100); }
    }

    public int SFXVolume
    {
        get { return _SFXVolume; }
        set { _SFXVolume = Math.Clamp(value, 0, 100); }
    }


    public int SashaVoiceVolume
    {
        get { return _SashaVoiceVolume; }
        set { _SashaVoiceVolume = Math.Clamp(value, 0, 100); }
    }

    public int EdanVoiceVolume
    {
        get { return _EdanVoiceVolume; }
        set { _EdanVoiceVolume = Math.Clamp(value, 0, 100); }
    }

    public int SionVoiceVolume
    {
        get { return _SionVoiceVolume; }
        set { _SionVoiceVolume = Math.Clamp(value, 0, 100); }
    }
}
