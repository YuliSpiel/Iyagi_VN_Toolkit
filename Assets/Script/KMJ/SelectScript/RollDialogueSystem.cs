using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

public class RollDialogueSystem : MonoBehaviour
{
    public string jsonFileName; // JSON 파일 이름 설정 (확장자 없이)

    public SelectStatManager selectStatManager;
    public string Chapter;
    public string SelectNumber;
    public TextMeshProUGUI RollDiceResult;
    public MoveCount moveCount;

    public ResultDialogueClass resultDialogueClass;

    public List<DialogueEntry> selectData;
    public int currentDialogueIndex = 0;
    public bool isTyping = false;
    public Coroutine typingCoroutine;

    public float typingSpeed = 0.75f;
    public float fadeDuration = 0.13f;

    public AudioSource DialogueAudioSource;

    public GameObject circle;
    public StatManager statManager;

    public ResultDialogueEntry currentStatDetail;

    public int textCount;
    public ResultDialogueClass currentSelectDetail; // 여기서 selectData는 필터링된 데이터라고 가정합니다.

    //그래픽 관련
    [SerializeField] DialogueImageSetting dialogueImageSetting;

    //로그 관련
    [SerializeField] LogforSave logforSave;

    private void Start()
    {
        InputManager.Instance.OnNextDialogue += OnNextButtonPressed;
        InputManager.Instance.OnSkipDialogue += HandleSkipDialogue;
    }

    private void OnNextButtonPressed()
    {
        Debug.Log("Roll Dialogue Next");
    }

    private void HandleSkipDialogue()
    {
        typingSpeed = 0.05f;
        OnNextButtonPressed();
    }

    private void OnEnable()
    {
        statManager = GameObject.Find("StatDataManager").GetComponent<StatManager>();
        dialogueImageSetting.DialogueGraphicSetting();
        currentDialogueIndex = 0; // 대화 인덱스 초기화
        dialogueImageSetting.dialogueText.text = ""; // 대화 텍스트 초기화
        LoadDialogues(); // 데이터를 먼저 로드하고 성공/실패에 맞는 텍스트를 설정
        if (selectData != null && selectData.Count > 0)
        {
            ShowDialogueByIndex(); // 데이터가 있을 경우 대화 표시
        }
        else
        {
            Debug.LogError("대화 데이터가 비어 있습니다.");
        }
        logforSave.LogSetting();
    }

    void SetSuccessFail()
    {
        if (currentSelectDetail == null || selectStatManager == null)
        {
            Debug.LogError("currentSelectDetail 또는 selectStatManager가 초기화되지 않았습니다.");
            return;
        }

        string currentStatState = selectStatManager.statState; // "성공" 또는 "실패"
        if (currentStatState == "성공")
        {
            textCount = currentSelectDetail.ResultDialogue[0].SuccessText.Count + 1;
            selectData = currentSelectDetail.ResultDialogue[0].SuccessText;
        }
        else if (currentStatState == "실패")
        {
            textCount = currentSelectDetail.ResultDialogue[0].FailText.Count + 1;
            selectData = currentSelectDetail.ResultDialogue[0].FailText;
        }
        else
        {
            Debug.LogWarning($"알 수 없는 StatDetail 값: {currentStatState}");
            textCount = 0;
            selectData = new List<DialogueEntry>();
        }

        if (textCount > 0)
        {
            currentDialogueIndex = 0; // 초기화
            dialogueImageSetting.dialogueText.text = selectData[0].Dialogue; // 첫 번째 대화 출력
        }
        else
        {
            Debug.LogWarning("textCount가 0입니다. 대화 텍스트를 설정하지 못했습니다.");
        }
    }

    //private float inputCooldown = 0.5f; // 입력 쿨다운 시간
    //private float lastInputTime = 0f;  // 마지막 입력 시간

    //void Update()
    //{
    //    float scrollInput = Input.GetAxis("Mouse ScrollWheel");
    //    // 현재 시간과 마지막 입력 시간 비교
    //    if (Time.time - lastInputTime < inputCooldown) return; // 쿨다운 중이면 입력 무시

    //    if (!logforSave.isLogWindowOn && !isTyping)
    //    {
    //        // Ctrl 키가 눌렸을 때 대사를 빠르게 스킵
    //        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
    //        {
    //            if (isTyping) // 타이핑 중일 때 빠르게 대사 표시
    //            {
    //                typingSpeed = 0.05f; // 빠른 스킵 속도
    //            }
    //            else
    //            {
    //                typingSpeed = 0.05f; // 타이핑 중이 아니더라도 빠르게 넘어가도록 설정
    //                OnNextButtonPressed(); // 대사 넘어가게 호출
    //            }
    //        }
    //        else
    //        {
    //            typingSpeed = 0.75f; // 기본 속도로 되돌림
    //        }

    //        // Check if the Enter or Space key is pressed
    //        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
    //        {
    //            OnNextButtonPressed();
    //        }
    //        if (scrollInput < 0f)
    //        {
    //            lastInputTime = Time.time; // 입력 시간 업데이트
    //            logforSave.HandleScrollInput(1);
    //            OnNextButtonPressed();
    //        }

    //        else if (scrollInput > 0f)
    //        {
    //            logforSave.HandleScrollInput(-1);
    //        }
    //    }
    //    else if (logforSave.isLogWindowOn)
    //    {
    //        // Check if the Enter or Space key is pressed
    //        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)
    //            || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
    //        {
    //            logforSave.logWindow.SetActive(false);
    //            logforSave.logBackground.SetActive(false);
    //            logforSave.isLogWindowOn = false;
    //        }
    //    }
    //}

    private IEnumerator DelaySetIsLogWindowOff()
    {
        logforSave.logWindow.SetActive(false);
        logforSave.isLogWindowOn = false;
        yield return new WaitForSeconds(3f); // 0.5초 딜레이
        Debug.Log("Log window state set to false after delay.");
    }

    void LoadDialogues()
    {
        jsonFileName = selectStatManager.resultDiaFileName;
        TextAsset jsonFile = Resources.Load<TextAsset>(selectStatManager.resultDiaFilePath);
        Debug.Log("File Path is " + selectStatManager.resultDiaFilePath);
        Debug.Log("JSON file name is " + jsonFileName);

        if (jsonFile == null)
        {
            Debug.LogError($"JSON 파일을 찾을 수 없습니다: {jsonFileName}");
            return;
        }

        string json = jsonFile.text;
        currentSelectDetail = JsonUtility.FromJson<ResultDialogueClass>(json);

        if (currentSelectDetail == null || currentSelectDetail.ResultDialogue == null || currentSelectDetail.ResultDialogue.Count == 0)
        {
            Debug.LogError("JSON 데이터가 비어 있거나 잘못되었습니다.");
            return;
        }

        // 성공 실패에 따라 대화 데이터를 설정
        SetSuccessFail();
    }

    //public void OnNextButtonPressed()
    //{
    //    logforSave.SavetoLog();
    //    if (selectData == null || textCount == 0)
    //    {
    //        Debug.LogError("DialogueSystem이 초기화되지 않았거나 대화 데이터가 없습니다.");
    //        return;
    //    }
    //    if (currentDialogueIndex >= textCount - 1)
    //    {
    //        EndOfScript();
    //    }
    //    // 경계 체크 후 인덱스 증가
    //    if (currentDialogueIndex < textCount - 1)
    //    {
    //        currentDialogueIndex++;
    //    }
    //    // 인덱스에 맞는 대화 출력
    //    ShowDialogueByIndex();
    //}

    void EndOfScript()
    {
        // 새로운 대화가 시작될 때 이전 대화 상태 초기화
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        if (selectStatManager.currentStatDetail == "턴종료")
        {
            moveCount.movecountPublic = 0;
        }
        if (moveCount.movecountPublic > 0)
        {
            circle.SetActive(true);
            this.gameObject.SetActive(false);
        }
        else if (moveCount.movecountPublic == 0 )
        {
            Debug.Log("EndOfScript: RollDialogueSystem");
            selectStatManager.CloseRollDiceWindowBTN();
            circle.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }

    public void ShowDialogueByIndex()
    {
        if (textCount == 0 || currentDialogueIndex >= textCount)
        {
            Debug.LogError("dialogues가 초기화되지 않았거나 인덱스 범위를 초과했습니다.");
            return;
        }

        if (currentDialogueIndex < textCount - 2)
        {
            DialogueEntry dialogueEntry = selectData[currentDialogueIndex];
            logforSave.Logname = dialogueEntry.CharName;
            logforSave.Logdialogue = dialogueEntry.Dialogue;
            StartCoroutine(HandleDialogueEntry(dialogueEntry));
        }
        else if (currentDialogueIndex == textCount - 2)
        {
            Debug.Log("textCount-2 " + textCount);
            DialogueEntry dialogueEntry = selectData[currentDialogueIndex];
            dialogueEntry.Dialogue += selectStatManager.diceResultExplainTxt;
            logforSave.Logdialogue = dialogueEntry.Dialogue;
            StartCoroutine(HandleDialogueEntry(dialogueEntry));
        }
        else if (currentDialogueIndex == textCount - 1)
        {
            Debug.Log("textCount-1 " + textCount);
            if (selectStatManager.diceResultEffectTxt == "") { EndOfScript(); }
            else
            {
                DialogueEntry dialogueEntry = selectData[currentDialogueIndex - 1];
                dialogueEntry.Dialogue = selectStatManager.diceResultEffectTxt; //+할지말지
                logforSave.Logname = dialogueEntry.CharName;
                logforSave.Logdialogue = dialogueEntry.Dialogue;
                StartCoroutine(HandleDialogueEntry(dialogueEntry));
            }
        }
        //Debug.Log("!!!!logforSave.Logname " + logforSave.Logname);
        //Debug.Log("!!!!logforSave.Logdialogue " + logforSave.Logdialogue);
    }

    private Coroutine dialogueCoroutine;

    IEnumerator HandleDialogueEntry(DialogueEntry entry)
    {
        // 기존 코루틴이 실행 중이라면 중지
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }

        // 새로운 코루틴 실행
        dialogueCoroutine = StartCoroutine(RunDialogue(entry));
        yield return dialogueCoroutine;
    }

    IEnumerator RunDialogue(DialogueEntry entry)
    {
        // Start coroutines for setting images and typing dialogue simultaneously
        Coroutine imageCoroutine = StartCoroutine(dialogueImageSetting.SetCharacterImages(entry));
        Coroutine typingCoroutine = StartCoroutine(TypeDialogue(entry.Dialogue));

        // Wait for both coroutines to complete
        yield return imageCoroutine;
        yield return typingCoroutine;

        // Start sfxCoroutine and wait for it to complete
        yield return StartCoroutine(SetSFXAudio(entry));

        // Start voiceCoroutine and wait for it to complete
        yield return StartCoroutine(SetVoiceAudio(entry));
    }

    IEnumerator SetSFXAudio(DialogueEntry entry)
    {
        if (entry.SFX != "")
        {
            if(DialogueAudioSource == null)
            {
                DialogueAudioSource = gameObject.AddComponent<AudioSource>();
            }
            AudioClip sfxClip = Resources.Load<AudioClip>($"Sound/SFX/{entry.SFX}");
            // AudioClip을 AudioSource에 할당
            DialogueAudioSource.clip = sfxClip;
            // 오디오 재생
            DialogueAudioSource.Play();
        }
        yield return null;
    }

    IEnumerator SetVoiceAudio(DialogueEntry entry)
    {
        if (entry.Voice != "")
        {
            if (DialogueAudioSource == null)
            {
                DialogueAudioSource = gameObject.AddComponent<AudioSource>();
            }
            AudioClip voiceClip = Resources.Load<AudioClip>($"Sound/Voice/Select/{SelectNumber}/v_{entry.Voice}");
            // AudioClip을 AudioSource에 할당
            DialogueAudioSource.clip = voiceClip;
            // 오디오 재생
            DialogueAudioSource.Play();
        }
        yield return null;
    }

    IEnumerator TypeDialogue(string dialogue)
    {
        isTyping = true;
        dialogueImageSetting.dialogueText.text = "";

        foreach (char letter in dialogue)
        {
            dialogueImageSetting.dialogueText.text += letter;
            yield return null;
            //yield return new WaitForSeconds(typingSpeed); // Adjust typing speed here
        }
        isTyping = false;
    }
}