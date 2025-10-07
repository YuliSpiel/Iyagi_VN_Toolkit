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
using static UnityEngine.GraphicsBuffer;

public class DialogueSystem0 : MonoBehaviour
{
    public static DialogueSystem0 Instance;

    public string ChapterNum;
    private string jsonFileName; // JSON 파일 이름 설정 (확장자 없이)

    public List<DialogueEntry> dialogues;
    public int currentDialogueIndex = 0;
    public bool isTyping = false;
    public Coroutine typingCoroutine;

    [SerializeField] OptionManager optionaManager;

    public float typingSpeed;

    [SerializeField] AudioSource SFXAudioSource;
    [SerializeField] AudioSource VoiceAudioSource;

    public StatManager statManager;

    ////테스트용
    //public TextMeshProUGUI keyPressedText;

    //선택지 관련
    public GameObject selectBox;

    public Button two_Selct1;
    public Button two_Selct2;

    public Button three_Selct1;
    public Button three_Selct2;
    public Button three_Selct3;

    //주사위 관련
    public GameObject rollDice;
    private SelectStatManager selectStatManager;

    //퀘스트 관련 //수정해야함
    public GameObject optionWindow;
    private ScreenOptionManager screenOptionManager;

    public string[] nextDialogue;

    public bool isDiceWindowOn = false;
    public bool isSelectChoiced = false;

    //그래픽 관련
    [SerializeField] DialogueImageSetting dialogueImageSetting;

    //로그 관련
    [SerializeField] LogforSave logforSave;

    //H key pressed
    private bool isHkeyOn = true;
    public GameObject DialogueGameGroupObject;

    private void Awake()
    {
        Instance = this;
        dialogueImageSetting.DialogueGraphicSetting();
        selectBox.SetActive(false);
        screenOptionManager = optionWindow.GetComponentInChildren<ScreenOptionManager>();
        statManager = GameObject.Find("StatDataManager").GetComponent<StatManager>();
        logforSave.LogSetting();
    }

    private void Start()
    {
        LoadDialogues();
        ShowDialogueByIndex(0); // 첫 번째 대화 출력
        rollDice.SetActive(false);
        InputManager.Instance.OnNextDialogue += HandleNextDialogue;
        InputManager.Instance.OnSkipDialogue += HandleSkipDialogue;
        InputManager.Instance.OnToggleUI += ToggleUI;
    }

    private void HandleNextDialogue()
    {
        OnNextButtonPressed(dialogues[0]);
    }

    private void HandleSkipDialogue()
    {
        typingSpeed = optionaManager.TypingSkipSpeed / 100 ;
        if (!isTyping)
        {
            OnNextButtonPressed(dialogues[0]);
        }
    }

    private void ToggleUI()
    {
        DialogueGameGroupObject.SetActive(isHkeyOn);
        dialogueImageSetting.backgroundImg.gameObject.SetActive(!isHkeyOn);
        isHkeyOn = !isHkeyOn;
    }

    //private float inputCooldown = 0.5f; // 입력 쿨다운 시간
    //private float lastInputTime = 0f;  // 마지막 입력 시간

    //void Update()
    //{
    //    float scrollInput = Input.GetAxis("Mouse ScrollWheel");
    //    // 현재 시간과 마지막 입력 시간 비교
    //    if (Time.time - lastInputTime < inputCooldown) return; // 쿨다운 중이면 입력 무시

    //    if (!isDiceWindowOn && !logforSave.isLogWindowOn && !isTyping)
    //    {

    //        if (!isSelectChoiced)
    //        {
    //            // Ctrl 키가 눌렸을 때 대사를 빠르게 스킵
    //            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
    //            {
    //                if (isTyping) // 타이핑 중일 때 빠르게 대사 표시
    //                {
    //                    typingSpeed = 0.05f; // 빠른 스킵 속도
    //                }
    //                else
    //                {
    //                    typingSpeed = 0.05f; // 타이핑 중이 아니더라도 빠르게 넘어가도록 설정
    //                    OnNextButtonPressed(dialogues[0]); // 대사 넘어가게 호출
    //                }
    //            }
    //            else
    //            {
    //                typingSpeed = 0.75f; // 기본 속도로 되돌림
    //            }

    //            // Check if the Enter or Space key is pressed
    //            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
    //            {
    //                OnNextButtonPressed(dialogues[0]);
    //            }

    //            if (scrollInput < 0f)
    //            {
    //                lastInputTime = Time.time; // 입력 시간 업데이트
    //                logforSave.HandleScrollInput(1);
    //                OnNextButtonPressed(dialogues[0]);
    //            }

    //            //왼쪽 상단에 뜨는 사샤 스탯창도 안 뜨게 수정 필요
    //            if ((Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.H)) && isHkeyOn)
    //            {
    //                Debug.Log("H key is pressed, SetActive false");

    //                DialogueGameGroupObject.SetActive(false);
    //                dialogueImageSetting.backgroundImg.gameObject.SetActive(true); // 목표 오브젝트 활성화
    //                isHkeyOn = !isHkeyOn;
    //            }
    //            else if((Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.H)) && !isHkeyOn)
    //            {
    //                Debug.Log("H key is pressed SetActive True");
    //                DialogueGameGroupObject.SetActive(true);
    //                isHkeyOn = !isHkeyOn;
    //            }
    //        }
    //        if (scrollInput > 0f)
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
    //            //lastInputTime = Time.time; // 입력 시간 업데이트
    //            //logforSave.HandleScrollInput(1);
    //            ////OnNextButtonPressed(dialogues[0]);
    //            logforSave.logWindow.SetActive(false);
    //            logforSave.logBackground.SetActive(false);
    //            logforSave.isLogWindowOn = false;
    //        }
    //    }
    //}

    public void OnNextButtonPressed(DialogueEntry entry)
    {
        DialogueEntry currentDialogue = null;

        foreach (var dialogue in dialogues)
        {
            //Debug.Log("****dialogue index is "+dialogue.Index);
            if (dialogue.Index == currentDialogueIndex)
            {
                currentDialogue = dialogue;
                break;  // 항목을 찾았으면 반복문 종료
            }
        }
        if (currentDialogue != null)
        {
            Debug.Log("현재 대화 항목: " + currentDialogue);
        }
        else
        {
            Debug.LogError("해당 Index에 해당하는 대화 항목을 찾을 수 없습니다.");
        }
        //LogVFX = entry.Voice;
        nextDialogue = currentDialogue.AfterDialogue.Split('_');

        if (currentDialogue.CharName == "None") logforSave.Logname = "";
        else logforSave.Logname = currentDialogue.CharName;
        // Dialogue 처리
        if (nextDialogue[0].Equals("Dialogue", StringComparison.OrdinalIgnoreCase))
        {
            logforSave.Logdialogue = currentDialogue.Dialogue;
            if (nextDialogue.Length > 1 && int.TryParse(nextDialogue[1], out int nextIndex))
            {
                // 다음 대사 인덱스를 갱신
                currentDialogueIndex = nextIndex;
                Debug.Log($"Next Dialogue Index: {currentDialogueIndex}");
                ShowDialogueByIndex(currentDialogueIndex);  // 대사 갱신
            }
            else
            {
                Debug.LogError("Invalid Dialogue Index");
            }
            logforSave.SavetoLog();
        }
        // Sanity 처리
        else if (nextDialogue[0].Equals("Sanity", StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("Sanity 처리 시작");
            ShowSanityDice(nextDialogue);  // Sanity 처리 후 대화 갱신
            logforSave.SavetoLog();
        }
        // Select 처리
        else if (nextDialogue[0].Equals("Select", StringComparison.OrdinalIgnoreCase))
        {
            logforSave.Logname = "System";
            dialogueImageSetting.dialBox.SetActive(false);
            //selectBox.SetActive(true);
            //isSelectChoiced = true;
            ShowSelectWindow(entry, nextDialogue);  // 선택지 처리
        }
        // Quest 처리
        else if (nextDialogue[0].Equals("Quest", StringComparison.OrdinalIgnoreCase))
        {
            logforSave.Logdialogue = currentDialogue.Dialogue;
            ShowQuest(nextDialogue);  // 퀘스트 처리
            logforSave.SavetoLog();
        }
        // Dice 처리
        else if (nextDialogue[0].Equals("Dice", StringComparison.OrdinalIgnoreCase))
        {
            logforSave.Logdialogue = currentDialogue.Dialogue;
            //Debug.Log("DiceSystem Function");
            currentDialogueIndex = int.Parse(nextDialogue[1]);
            Debug.Log("####Dialogue System currentDialogueIndex: " + currentDialogueIndex);
            ShowDiceWindow(nextDialogue);
            logforSave.SavetoLog();
        }
        //흐려지기, 흔들리기, 지지직, fade in&out, 파티클, 카메라 이동,텍스트 효과(글자 지지직/폰트 크기 변화), 화면 깜빡깜빡 추가해야 함
        // 그 외의 타입 처리
        else
        {
            Debug.LogError($"Unknown AfterDialogue type: {nextDialogue[0]}");
        }

        //SavetoLog(entry);
    }

    public void OnNextButtonPressed()
    {
        // 적절한 DialogueEntry를 찾아서 호출
        if (dialogues.Count > 0)
        {
            OnNextButtonPressed(dialogues[0]);  // 첫 번째 대화 항목을 기본값으로 사용
        }
        else
        {
            Debug.LogError("대화 목록이 비어 있습니다.");
        }
    }

    void LoadDialogues()
    {
        Debug.Log("LoadDialogues Function");
        Debug.Log("current index: " + currentDialogueIndex);
        jsonFileName = "Ch" + ChapterNum;
        //Debug.Log("JSON file name is " + jsonFileName);
        TextAsset jsonFile = Resources.Load<TextAsset>($"JSON/Dialogue/{jsonFileName}");
        if (jsonFile == null)
        {
            Debug.LogError($"JSON 파일을 찾을 수 없습니다: {jsonFileName}");
            return;
        }

        string json = jsonFile.text;
        DialogueList dialogueList = JsonUtility.FromJson<DialogueList>(json);
        //dialogues = dialogueList.dialogues.FindAll(d => d.SceneNum == sceneNum.ToString());

        // SceneNum 필터링 제거, 모든 대화를 가져옵니다.
        dialogues = dialogueList.dialogues;
    }


    public void ShowDialogueByIndex(int indexNum)
    {
        if (dialogues == null || dialogues.Count == 0)
        {
            Debug.LogError("대화 데이터가 없습니다.");
            return;
        }
        dialogueImageSetting.dialBox.SetActive(true);

        // 현재 대화 가져오기
        DialogueEntry dialogueEntry = dialogues.FirstOrDefault(d => d.Index == indexNum);

        if (dialogueEntry == null)
        {
            Debug.LogError($"Index {indexNum}에 해당하는 대화가 없습니다.");
            return;
        }

        // sanityChangeText가 있을 경우 대사에 추가
        if (!string.IsNullOrEmpty(sanityChangeText))
        {
            dialogueEntry.Dialogue += " " + sanityChangeText;
            sanityChangeText = ""; // 한 번 반영 후 초기화
        }

        Debug.Log($"ShowDialogueByIndex: Current Dialogue Index = {indexNum}, Dialogue = {dialogueEntry.Dialogue}");

        // 대화 표시 처리
        StartCoroutine(HandleDialogueEntry(dialogueEntry));
    }


    void ShowDiceWindow(String[] nextDialogue)
    {
        rollDice.SetActive(true);
        foreach (Transform child in this.gameObject.transform)
        {
            GameObject childObject = child.gameObject;
            //Debug.Log("******GameObject is " + childObject);
            if (childObject.name == "Background") childObject.SetActive(true);
            else childObject.SetActive(false); //수정해야함
        }
        logforSave.SaveDialogue();
        isDiceWindowOn = true;
    }

    void ShowSelectWindow(DialogueEntry entry, String[] nextDialogue)
    {
        selectBox.SetActive(true);
        isSelectChoiced = true;
        Debug.Log("***ShowSelectWindow***");
        foreach (String i in nextDialogue) Debug.Log(i);
        if (int.Parse(nextDialogue[1]) == 2)
        {
            two_Selct1.gameObject.SetActive(true);
            two_Selct2.gameObject.SetActive(true);
            two_Selct1.onClick.AddListener(() => OnSelectButtonPressed(1, nextDialogue));
            two_Selct2.onClick.AddListener(() => OnSelectButtonPressed(2, nextDialogue));


            three_Selct1.gameObject.SetActive(false);
            three_Selct2.gameObject.SetActive(false);
            three_Selct3.gameObject.SetActive(false);

            two_Selct1.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = nextDialogue[2];
            two_Selct2.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = nextDialogue[4];

        }
        else if (int.Parse(nextDialogue[1]) == 3)
        {
            two_Selct1.gameObject.SetActive(false);
            two_Selct2.gameObject.SetActive(false);

            three_Selct1.gameObject.SetActive(true);
            three_Selct2.gameObject.SetActive(true);
            three_Selct3.gameObject.SetActive(true);

            three_Selct1.onClick.AddListener(() => OnSelectButtonPressed(1, nextDialogue));
            three_Selct2.onClick.AddListener(() => OnSelectButtonPressed(2, nextDialogue));
            three_Selct3.onClick.AddListener(() => OnSelectButtonPressed(3, nextDialogue));

            three_Selct1.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = nextDialogue[2];
            three_Selct2.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = nextDialogue[4];
            three_Selct3.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = nextDialogue[6];
        }
        // 현재 대화가 끝나면 다음 대화로 이동
        ShowDialogueByIndex(currentDialogueIndex);
    }

    private bool isAnyButtonHovered = false;
    public void SelctionHover(bool isHovered, int num)
    {
        Image two_one = two_Selct1.GetComponent<Image>();
        Image two_two = two_Selct2.GetComponent<Image>();
        Image three_one = three_Selct1.GetComponent<Image>();
        Image three_two = three_Selct2.GetComponent<Image>();
        Image three_three = three_Selct3.GetComponent<Image>();
        if (isHovered)
        {
            isAnyButtonHovered = true;
            switch (num)
            {
                case 1:
                    two_one.sprite = Resources.Load<Sprite>($"Images/UI/Select_Selected");
                    two_two.sprite = Resources.Load<Sprite>($"Images/UI/Select_NotSelected");
                    break;
                case 2:
                    two_one.sprite = Resources.Load<Sprite>($"Images/UI/Select_NotSelected");
                    two_two.sprite = Resources.Load<Sprite>($"Images/UI/Select_Selected");
                    break;
                case 3:
                    three_one.sprite = Resources.Load<Sprite>($"Images/UI/Select_Selected");
                    three_two.sprite = Resources.Load<Sprite>($"Images/UI/Select_NotSelected");
                    three_three.sprite = Resources.Load<Sprite>($"Images/UI/Select_NotSelected");
                    break;
                case 4:
                    three_one.sprite = Resources.Load<Sprite>($"Images/UI/Select_NotSelected");
                    three_two.sprite = Resources.Load<Sprite>($"Images/UI/Select_Selected");
                    three_three.sprite = Resources.Load<Sprite>($"Images/UI/Select_NotSelected");
                    break;
                case 5:
                    three_one.sprite = Resources.Load<Sprite>($"Images/UI/Select_NotSelected");
                    three_two.sprite = Resources.Load<Sprite>($"Images/UI/Select_NotSelected");
                    three_three.sprite = Resources.Load<Sprite>($"Images/UI/Select_Selected");
                    break;
            }
        }
        else
        {
            two_one.sprite = Resources.Load<Sprite>($"Images/UI/Select_Basic");
            two_two.sprite = Resources.Load<Sprite>($"Images/UI/Select_Basic");
            three_one.sprite = Resources.Load<Sprite>($"Images/UI/Select_Basic");
            three_two.sprite = Resources.Load<Sprite>($"Images/UI/Select_Basic");
            three_three.sprite = Resources.Load<Sprite>($"Images/UI/Select_Basic");
        }
    }

    void OnSelectButtonPressed(int buttonIndex, string[] nextDialogue)
    {
        //Debug.Log("OnSelectButtonPressed");
        //Debug.Log($"Button {buttonIndex} clicked");
        if (buttonIndex == 1)
        {
            logforSave.Logdialogue += "\n\n" + nextDialogue[2];
            currentDialogueIndex = int.Parse(nextDialogue[3]);
        }
        else if (buttonIndex == 2)
        {
            logforSave.Logdialogue += "\n\n" + nextDialogue[4];
            currentDialogueIndex = int.Parse(nextDialogue[5]);
        }
        else if (buttonIndex == 3)
        {
            logforSave.Logdialogue += "\n\n" + nextDialogue[6];
            currentDialogueIndex = int.Parse(nextDialogue[7]);
        }
        //Debug.Log("**********OnSelectButtonPressed Logdialogue" + Logdialogue);
        logforSave.SavetoLog();
        logforSave.SaveDialogue();
        selectBox.SetActive(false); // 선택창 닫기
        isSelectChoiced = false;
        ShowDialogueByIndex(currentDialogueIndex); // 다음 대화 표시
    }

    public string sanityChangeText;

    void ShowSanityDice(String[] nextDialogue)
    {
        Debug.Log("ShowSanityDice called with: " + string.Join(", ", nextDialogue));

        int sanity = UnityEngine.Random.Range(1, 4);
        int beforeSanity = statManager.statJsonList.Items[0].Sanity;

        sanityChangeText = "";  // 대사에 추가할 Sanity 변화 텍스트

        if (nextDialogue[1] == "P")  // Sanity 증가
        {
            Debug.Log("Sanity Plus " + sanity);
            statManager.statJsonList.Items[0].Sanity += sanity;
            sanityChangeText = $"<color=#FFFF00><b>(sanity +1D4)</b></color><color=#FFFFFF><i>sanity({beforeSanity}) + {sanity} = {statManager.statJsonList.Items[0].Sanity}</i></color>";
        }
        else if (nextDialogue[1] == "M")  // Sanity 감소
        {
            Debug.Log("Sanity Minus " + sanity);
            statManager.statJsonList.Items[0].Sanity -= sanity;
            sanityChangeText = $"<color=#FFFF00><b>(sanity -1D4)</b></color><color=#FFFFFF><i>sanity({beforeSanity}) - {sanity} = {statManager.statJsonList.Items[0].Sanity}</i></color>";
        }
        Debug.Log("Updated Sanity: " + statManager.statJsonList.Items[0].Sanity);

        // 대사 텍스트 업데이트
        dialogues[currentDialogueIndex].Dialogue += " " + sanityChangeText;
        logforSave.Logdialogue = dialogues[currentDialogueIndex].Dialogue;
        // UI 즉시 업데이트
        dialogueImageSetting.dialogueText.text = dialogues[currentDialogueIndex].Dialogue;
        // Sanity 텍스트 초기화
        sanityChangeText = "";

        statManager.StatSave();
        //SavetoLog("Sanity", nextDialogue);
        // 다음 대화로 이동
        if (int.TryParse(nextDialogue[2], out int nextIndex))
        {
            currentDialogueIndex = nextIndex;
        }
    }

    //수정
    void ShowQuest(String[] nextDialogue)
    {
        if (nextDialogue[2] == "On")
        {
            optionWindow.transform.GetChild(0).gameObject.SetActive(true);
            screenOptionManager.GlitchOn();
        }
        else if (nextDialogue[2] == "Stay")
        {
            optionWindow.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (nextDialogue[2] == "Off")
        {
            screenOptionManager.GlitchOff();
            optionWindow.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (int.TryParse(nextDialogue[1], out int nextIndex))
        {
            currentDialogueIndex = nextIndex;  // Sanity 처리 후 다음 대화로 이동
            ShowDialogueByIndex(currentDialogueIndex);
        }
        else
        {
            Debug.LogError("AfterDialogue에서 지정된 다음 대화 인덱스를 찾을 수 없습니다.");
        }
        //SavetoLog("Dialogue", nextDialogue);
        logforSave.SaveDialogue();
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
        if (!string.IsNullOrEmpty(entry.SFX))
        {
            if (SFXAudioSource == null)
            {
                SFXAudioSource = gameObject.AddComponent<AudioSource>();
            }

            AudioClip sfxClip = Resources.Load<AudioClip>($"Sound/SFX/{entry.SFX}");
            SFXAudioSource.clip = sfxClip;

            // 다이얼로그 박스 활성화/비활성화
            dialogueImageSetting.dialBox.SetActive(!entry.PeopleNum.Equals(0) || !entry.Dialogue.Equals(""));
            // 오디오 재생
            SFXAudioSource.Play();

            // 오디오가 끝날 때까지 대기
            while (SFXAudioSource.isPlaying)
            {
                yield return null;
            }
        }
    }

    IEnumerator SetVoiceAudio(DialogueEntry entry)
    {
        if (!string.IsNullOrEmpty(entry.Voice))
        {
            if (VoiceAudioSource == null)
            {
                VoiceAudioSource = gameObject.AddComponent<AudioSource>();
            }

            AudioClip voiceClip = Resources.Load<AudioClip>($"Sound/Voice/{ChapterNum}/{entry.Voice}");

            //Debug.Log("VoiceClip is " + entry.Voice);
            logforSave.LogVFX = entry.Voice;

            VoiceAudioSource.clip = voiceClip;
            // 오디오 재생
            VoiceAudioSource.Play();

            // 오디오가 끝날 때까지 대기
            while (VoiceAudioSource.isPlaying)
            {
                yield return null;
            }
        }
    }

    void DisplayDialogue(DialogueEntry entry)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        dialogueImageSetting.SetCharacterImages(entry);
        typingCoroutine = StartCoroutine(TypeDialogue(entry.Dialogue));
    }

    IEnumerator TypeDialogue(string dialogue)
    {
        isTyping = true;
        dialogueImageSetting.nextButton.SetActive(false);
        dialogueImageSetting.dialogueText.text = "";

        foreach (char letter in dialogue)
        {
            dialogueImageSetting.dialogueText.text += letter;
            yield return new WaitForSeconds(1 - optionaManager.TypingSpeed/100);
        }
        logforSave.Logdialogue = dialogue.ToString();
        isTyping = false;
        dialogueImageSetting.nextButton.SetActive(true);
    }
}