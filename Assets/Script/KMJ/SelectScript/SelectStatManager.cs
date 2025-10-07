using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.IO;
using UnityEngine.Serialization;

public class SelectStatManager : MonoBehaviour
{
    // 필드
    public int chapter, selectNum, statNum, tempNum, successPercentage;
    private TextMeshProUGUI select1Text, moveCountNum, successPercentNum, successResult;
    public StatDetailManager statDetailManager;
    public StatManager statManager;

    public GameObject selectBTN;
    private GameObject selectStat;
    public RectTransform[] buttons = new RectTransform[6];
    public ButtonHoverManager hoverManager;

    // Hover Window 관련 필드
    public TextMeshProUGUI StatTextNum_HoverWindow, TargetNum_HoverWindow;
    public RectTransform transform_Hover;
    public TextMeshProUGUI diceResultTxt;
    public TextMeshProUGUI dice3344Text; //주사위 3,3 4,4 나왔을 때
    public String diceResultEffectTxt, diceResultExplainTxt;
    public String targetNum1;
    public MoveCount moveCount;

    public String statname;
    public String statState; //주사위 결과 저장용

    public String currentStatDetail = null;

    public String resultDiaFilePath;
    public String resultDiaFileName;

    private bool isHovering = false; // 마우스가 게임 오브젝트 위에 있는지 상태를 저장

    [FormerlySerializedAs("dialogueSystem")] public DialogueSystem0 dialogueSystem0;

    public GameObject CircleWindow;

    public GameObject dialSystemWindow;

    public GameObject rollDialogueWindow;

    //로그 관련
    [SerializeField] LogforSave logforSave;

    public void CloseRollDiceWindowBTN()
    {
        Debug.Log("CloseRollDiceWindowBTN: RollDialogueSystem");
        RollWindowClose();
        ResultDia();
        if (moveCount.movecountPublic > 0)
        {
            moveCount.movecountPublic -= 1;
        }
        else if (moveCount.movecountPublic == 0)
        {
            Debug.Log("moveCount.movecountPublic == 0: CloseRollDiceWindowBTN: RollDialogueSystem");
            //nextDialogue실행하는 코드 실행하는 거 추가
            dialSystemWindow.SetActive(true);
            
            dialogueSystem0.isDiceWindowOn = false;
            foreach (Transform child in dialSystemWindow.transform)
            {
                GameObject childObject = child.gameObject;
                if (childObject.name == "DialogueBox" || childObject.name == "NextBTN") childObject.SetActive(true);
            }
            //Debug.Log("##CloseRollDiceWindowBTN currentDialogueIndex: " + dialogueSystem.currentDialogueIndex);
            dialogueSystem0.ShowDialogueByIndex(dialogueSystem0.currentDialogueIndex);
            this.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        InitializeUIElements();
        AssignButtonEvents();
        Init_Cursor();
    }

    private void Update()
    {
        moveCount = GameObject.Find("MoveCountManager").GetComponent<MoveCount>();
        Update_MousePosition();
    }

    private void OnEnable()
    {
        statDetailManager = GameObject.Find("StatDetailManager").GetComponent<StatDetailManager>();
        statManager = GameObject.Find("StatDataManager").GetComponent<StatManager>();
        hoverManager = FindObjectOfType<ButtonHoverManager>();
        chapter = int.Parse(dialogueSystem0.nextDialogue[2]);
        selectNum = int.Parse(dialogueSystem0.nextDialogue[3]);
    }

    private void Init_Cursor()
    {
        transform_Hover.pivot = Vector2.up;

        if (transform_Hover.GetComponent<Graphic>())
            transform_Hover.GetComponent<Graphic>().raycastTarget = false;
    }

    //CodeFinder 코드파인더
    //From https://codefinder.janndk.com/ 
    private void Update_MousePosition()
    {
        if (!isHovering) return;  // Hover 상태가 아닐 경우 마우스 위치 업데이트를 중단

        Vector2 mousePos = Input.mousePosition + new Vector3(20, -50, 5);  // 마우스 위치 보정
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            transform_Hover.parent as RectTransform,  // 부모 RectTransform에 맞춰서 마우스 위치 계산
            mousePos,
            Camera.main,
            out Vector3 worldPos
        );
        transform_Hover.position = worldPos;  // transform_Hover의 위치를 마우스 위치로 업데이트
    }


    private void InitializeUIElements()
    {
        select1Text = GameObject.Find("Select (1)").GetComponentInChildren<TextMeshProUGUI>();
        successPercentNum = GameObject.Find("SuccessNum").GetComponent<TextMeshProUGUI>();
        successResult = GameObject.Find("SuccessResult").GetComponent<TextMeshProUGUI>();

        moveCountNum = GameObject.Find("NumText").GetComponent<TextMeshProUGUI>();
        selectStat = GameObject.Find("SelectStat");
        selectStat.SetActive(false);

        buttons = new RectTransform[6];
        buttons[0] = GameObject.Find("piece (1)").GetComponent<RectTransform>();
        buttons[1] = GameObject.Find("piece (2)").GetComponent<RectTransform>();
        buttons[2] = GameObject.Find("piece (3)").GetComponent<RectTransform>();
        buttons[3] = GameObject.Find("piece (4)").GetComponent<RectTransform>();
        buttons[4] = GameObject.Find("piece (5)").GetComponent<RectTransform>();
        buttons[5] = GameObject.Find("piece (6)").GetComponent<RectTransform>();
    }

    private void AssignButtonEvents()
    {
        buttons[0].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(statDetailManager.selectData.Select[selectNum - 1].Stat.Jineung, 1));
        buttons[1].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(statDetailManager.selectData.Select[selectNum - 1].Stat.Insung, 2));
        buttons[2].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(statDetailManager.selectData.Select[selectNum - 1].Stat.Sinche, 3));
        buttons[3].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(statDetailManager.selectData.Select[selectNum - 1].Stat.Jikkam, 4));
        buttons[4].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(statDetailManager.selectData.Select[selectNum - 1].Stat.Luck, 5));
        buttons[5].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(statDetailManager.selectData.Select[selectNum - 1].Stat.Yeom, 6));
    }

    private void OnButtonClick(StatDetail statDetail, int num)
    {
        tempNum = num - 1;
        DisplayStatDetail(statDetail, num);
        buttons[tempNum].GetComponent<Button>().enabled = false;
        statNum = num;
        UpdateHoverWindow(statDetail, num);
    }

    private void DisplayStatDetail(StatDetail statDetail, int num)
    {
        if (statDetail == null)
        {
            Debug.LogError("statDetail is null in DisplayStatDetail");
            return;
        }
        else if (successPercentNum == null)
        {
            Debug.LogError("successPercentNum is null");
            return;
        }
        else if (successResult == null)
        {
            Debug.LogError("successResult is null");
            return;
        }
        select1Text.text = statDetail.Before;
        successPercentNum.text = CalculateSuccessPercent(statDetail, num).ToString();
        // "매혹-"의 경우 뒤쪽을 제거
        if (statDetail.Success.StartsWith("매혹-"))
        {
            successResult.text = "매혹"; // "매혹-" 앞부분만 표시
        }
        else
        {
            successResult.text = statDetail.Success.ToString(); // 그대로 표시
        }
        targetNum1 = statDetail.LevelNum.ToString();
        diceResultTxt.text = "";
        diceResultEffectTxt = "";
        diceResultExplainTxt = "";
        dice3344Text.text = "";
    }

    private void UpdateHoverWindow(StatDetail statDetail, int num)
    {   
        UpdateStatNum(num);
        TargetNum_HoverWindow.text = statDetail.LevelNum.ToString();
    }

    private void UpdateStatNum(int num)
    {
        var stats = statManager.statJsonList.Items[0].Stats;
        switch (num)
        {
            case 1:
                StatTextNum_HoverWindow.text = stats.Jineung.ToString();
                statNum = stats.Jineung;
                statname = "Jineung";
                break;
            case 2:
                StatTextNum_HoverWindow.text = stats.Insung.ToString();
                statNum = stats.Insung;
                statname = "Insung";
                break;
            case 3:
                StatTextNum_HoverWindow.text = stats.Sinche.ToString();
                statNum = stats.Sinche;
                statname = "Sinche";
                break;
            case 4:
                StatTextNum_HoverWindow.text = stats.Jikkam.ToString();
                statNum = stats.Jikkam;
                statname = "Jikkam";
                break;
            case 5:
                StatTextNum_HoverWindow.text = stats.Luck.ToString();
                statNum = stats.Luck;
                statname = "Luck";
                break;
            case 6:
                StatTextNum_HoverWindow.text = stats.Yeom.ToString();
                statNum = stats.Yeom;
                statname = "Yeom";
                break;
        }
    }

    public void BTNEnter()
    {
        isHovering = true;  // Update hover state
        transform_Hover.gameObject.SetActive(true);
        hoverManager.SetHoverState(true, tempNum); // Optional: manage hover effects for buttons
    }

    public void BTNExit()
    {
        isHovering = false;  // Update hover state
        hoverManager.SetHoverState(false, tempNum); // Optional: reset hover effects for buttons
        transform_Hover.gameObject.SetActive(false);
    }

    private int CalculateSuccessPercent(StatDetail statDetail, int num)
    {
        int targetNum = statDetail.LevelNum;
        int currentStat = GetStatValue(num);
        int requiredValue = targetNum - currentStat;

        if (requiredValue <= 0) return 100;

        int possibleOutcomes = CalculatePossibleOutcomes(requiredValue);
        return (int)Math.Ceiling(((float)possibleOutcomes / 36) * 100);
    }

    private int GetStatValue(int num)
    {
        var stats = statManager.statJsonList.Items[0].Stats;
        return num switch
        {
            1 => stats.Jineung,
            2 => stats.Insung,
            3 => stats.Sinche,
            4 => stats.Jikkam,
            5 => stats.Luck,
            6 => stats.Yeom,
            _ => 0
        };
    }

    private int CalculatePossibleOutcomes(int realTargetNum)
    {
        int totalOutcomes = 0;
        if (realTargetNum >= 7)
        {
            for (int i = 13 - realTargetNum; i > 0; i--)
                totalOutcomes += i;
        }
        else
        {
            totalOutcomes += 6 * 7 / 2; // sum of first 6 numbers
            totalOutcomes += realTargetNum switch
            {
                2 => 15,
                3 => 14,
                4 => 12,
                5 => 9,
                6 => 5,
                _ => 0
            };
        }
        return totalOutcomes;
    }

    public void RollDiceAndUpdateUI()
    {
        int diceRoll1 = UnityEngine.Random.Range(1, 7);
        int diceRoll2 = UnityEngine.Random.Range(1, 7);

        ////33 test
        //diceRoll1 = 3;
        //diceRoll2 = 3;

        ////44test
        //diceRoll1 = 4;
        //diceRoll2 = 4;

        int totalRoll = diceRoll1 + diceRoll2 + statNum;

        statState = totalRoll >= int.Parse(targetNum1) ? "성공" : "실패";
        string statState1 = totalRoll >= int.Parse(targetNum1) ? "<color=#FFFF00>성공</color>" : "<color=#FF0000>실패</color>"; // 리치 텍스트로 색상 추가

        logforSave.Logname = $"시스템";
        String tempstatName = "";

        switch (statname)
        {
            case "Yeom":
                diceResultTxt.text += $"염력";
                tempstatName = $"염력";
                logforSave.Logdialogue = $"염력";
                break;
            case "Jineung":
                diceResultTxt.text += $"지능";
                tempstatName = $"지능";
                logforSave.Logdialogue = $"지능";
                break;
            case "Insung":
                diceResultTxt.text += $"인성";
                tempstatName = $"인성";
                logforSave.Logdialogue = $"인성";
                break;
            case "Sinche":
                diceResultTxt.text += $"신체";
                tempstatName = $"신체";
                logforSave.Logdialogue = $"신체";
                break;
            case "Jikkam":
                diceResultTxt.text += $"직감";
                tempstatName = $"직감";
                logforSave.Logdialogue = $"직감";
                break;
            case "Luck":
                diceResultTxt.text += $"운";
                tempstatName = $"운";
                logforSave.Logdialogue = $"운";
                break;
            default:
                Debug.LogError("Invalid stat name: " + statname);
                break;
        }
        logforSave.Logdialogue += $"을(를) 선택했습니다.";
        diceResultTxt.text += $"({statNum}) + {diceRoll1} + {diceRoll2} = {totalRoll}  {statState1}";

        //Log
        logforSave.Logdialogue += diceResultTxt.text.ToString();

        diceResultTxt.gameObject.SetActive(true);
        if ((diceRoll1 == 3 && diceRoll2 == 3 && statNum <= 7) || (diceRoll1 == 4 && diceRoll2 == 4 && statNum >= 3))
        {
            GameObject.Find("Close3344BTN").gameObject.SetActive(true);
            GameObject.Find("CloseBTN").gameObject.SetActive(false);
            dice3344Text.gameObject.SetActive(false);
            //3 3 나오고 해당 스탯이 7 이하
            if (diceRoll1 == 3 && diceRoll2 == 3 && statNum <= 7)
            {
                CalStat(statname, 1);
                dice3344Text.text += $"<color=#FFFF00>(3,3): </color>{tempstatName} + 1";
                logforSave.Logdialogue += dice3344Text.text.ToString();
            }
            //4 4 나오고 해당 스탯이 3 이상
            else if (diceRoll1 == 4 && diceRoll2 == 4 && statNum >= 3)
            {
                CalStat(statname, -1);
                dice3344Text.text += $"<color=#FF0000>(4,4): </color>{tempstatName} - 1";
                logforSave.Logdialogue += dice3344Text.text.ToString();
            }
        }
        else
        {
            GameObject.Find("Close3344BTN").gameObject.SetActive(false);
            GameObject.Find("CloseBTN").gameObject.SetActive(true);
        }

        if (statname == "Yeom")
        {
            Debug.Log("Before Sanity: " + statManager.statJsonList.Items[0].Sanity);
            statManager.statJsonList.Items[0].Sanity -= 1;
            Debug.Log("After Sanity: " + statManager.statJsonList.Items[0].Sanity);
            //Log로 알려줄건지말지
            statManager.StatSave();
        }
        else if (statname == "Insung")
        {
            Debug.Log("Before Sanity: " + statManager.statJsonList.Items[0].Sanity);
            statManager.statJsonList.Items[0].Sanity += 1;
            Debug.Log("After Sanity: " + statManager.statJsonList.Items[0].Sanity);
            statManager.StatSave();
        }

        if (statState == "성공")
        {
            // Access the correct StatDetail based on statname
            switch (statname)
            {
                case "Yeom":
                    currentStatDetail = statDetailManager.selectData.Select[selectNum - 1].Stat.Yeom.Success;
                    break;
                case "Jineung":
                    currentStatDetail = statDetailManager.selectData.Select[selectNum - 1].Stat.Jineung.Success;
                    break;
                case "Insung":
                    currentStatDetail = statDetailManager.selectData.Select[selectNum - 1].Stat.Insung.Success;
                    break;
                case "Sinche":
                    currentStatDetail = statDetailManager.selectData.Select[selectNum - 1].Stat.Sinche.Success;
                    break;
                case "Jikkam":
                    currentStatDetail = statDetailManager.selectData.Select[selectNum - 1].Stat.Jikkam.Success;
                    break;
                case "Luck":
                    currentStatDetail = statDetailManager.selectData.Select[selectNum - 1].Stat.Luck.Success;
                    break;
                default:
                    Debug.LogError("Invalid stat name: " + statname);
                    break;
            }
        }
        else if (statState == "실패")
        {
            switch (statname)
            {
                case "Yeom":
                    currentStatDetail = statDetailManager.selectData.Select[selectNum - 1].Stat.Yeom.Fail;
                    break;
                case "Jineung":
                    currentStatDetail = statDetailManager.selectData.Select[selectNum - 1].Stat.Jineung.Fail;
                    break;
                case "Insung":
                    currentStatDetail = statDetailManager.selectData.Select[selectNum - 1].Stat.Insung.Fail;
                    break;
                case "Sinche":
                    currentStatDetail = statDetailManager.selectData.Select[selectNum - 1].Stat.Sinche.Fail;
                    break;
                case "Jikkam":
                    currentStatDetail = statDetailManager.selectData.Select[selectNum - 1].Stat.Jikkam.Fail;
                    break;
                case "Luck":
                    currentStatDetail = statDetailManager.selectData.Select[selectNum - 1].Stat.Luck.Fail;
                    break;
                default:
                    Debug.LogError("Invalid stat name: " + statname);
                    break;
            }
        }
        if (currentStatDetail.StartsWith("매혹-"))
        {
            currentStatDetail = currentStatDetail.Substring(3); // "매혹-" 이후의 문자열만 남김
        }
        int beforesanity = statManager.statJsonList.Items[0].Sanity;
        switch (currentStatDetail) //효과 작성해야됨
        {
            case "이성":
                beforesanity = statManager.statJsonList.Items[0].Sanity;
                int sanity = UnityEngine.Random.Range(1, 4);
                statManager.statJsonList.Items[0].Sanity += sanity;
                statManager.StatSave();
                diceResultExplainTxt = $"<color=#FFFF00><b>(sanity +1D4)</b></color>";
                diceResultEffectTxt += $"<color=#FFFFFF><i>sanity({beforesanity}) + {sanity} = {statManager.statJsonList.Items[0].Sanity}</i></color>";
                logforSave.Logdialogue += "\n"+diceResultExplainTxt + "\n"+diceResultEffectTxt;
                break;
            case "경험치":
                int exp = UnityEngine.Random.Range(1, 10) * 10;
                statManager.statJsonList.Items[0].EXP += exp;
                if (statManager.statJsonList.Items[0].EXP >= 100)
                    statManager.statJsonList.Items[0].EXP -= 100;
                statManager.StatSave();
                diceResultExplainTxt = $"<color=#FFFF00><b>(경험치 1D10 * 10)</b></color>";
                diceResultEffectTxt += $"<color=#FFFFFF><i>경험치 {exp} +</i></color>";
                logforSave.Logdialogue += "\n" + diceResultExplainTxt + "\n" + diceResultEffectTxt;
                break;
            case "역효과":
                beforesanity = statManager.statJsonList.Items[0].Sanity;
                sanity = UnityEngine.Random.Range(1, 4);
                statManager.statJsonList.Items[0].Sanity -= sanity;
                statManager.StatSave();
                diceResultExplainTxt = $"<color=#FFFF00><b>(sanity -1D4)</b></color>";
                diceResultEffectTxt += $"<color=#FFFFFF><i>sanity({beforesanity}) - {sanity} = {statManager.statJsonList.Items[0].Sanity}</i></color>";
                logforSave.Logdialogue += "\n" + diceResultExplainTxt + "\n" + diceResultEffectTxt;
                break;
            case "정보":
                break;
            case "무영향":
                break;
            case "턴종료":
                Debug.Log("턴종료: SelectStatSystem");
                diceResultExplainTxt += $"<color=#FFFF00><b>(턴종료)</b></color>";
                logforSave.Logdialogue += "\n" + diceResultExplainTxt;
                //rollDialogueWindow.SetActive(true);
                //moveCount.movecountPublic = 0;
                break;
            //case "매혹":
            //    diceResultTxt.text += $"다음 효과를 얻습니다";
            //    break;
        }
        logforSave.SavetoLog();
    }

    private void CalStat(String statname, int num)
    {
        Debug.Log("Before StatNum: " + statNum);
        switch (statname)
        {
            case "Yeom":
                statManager.statJsonList.Items[0].Stats.Yeom += num;
                UpdateStatNum(6);
                break;
            case "Jineung":
                statManager.statJsonList.Items[0].Stats.Jineung += num;
                UpdateStatNum(1);
                break;
            case "Insung":
                statManager.statJsonList.Items[0].Stats.Insung += num;
                UpdateStatNum(2);
                break;
            case "Sinche":
                statManager.statJsonList.Items[0].Stats.Sinche += num;
                UpdateStatNum(3);
                break;
            case "Jikkam":
                statManager.statJsonList.Items[0].Stats.Jikkam += num;
                UpdateStatNum(4);
                break;
            case "Luck":
                statManager.statJsonList.Items[0].Stats.Luck += num;
                UpdateStatNum(5);
                break;
            default:
                Debug.LogError("Invalid stat name: " + statname);
                break;
        }
        Debug.Log("After StatNum: " + statNum);
        statManager.StatSave();
    }

    public void BTNClose()
    {
        //Debug.Log("@@@@@@ tempNum is " + tempNum);
        buttons[tempNum].GetComponent<Button>().enabled = true;
        diceResultTxt.text = "";
    }

    public void RollWindowClose() => buttons[tempNum].GetComponent<Button>().enabled = false;

    public void ResultDia()
    {
        resultDiaFileName = $"{chapter}_{selectNum}_{statname}";
        resultDiaFilePath = Path.Combine("JSON/Select/Result", statname, resultDiaFileName); // 확장자 제외
    }
}