using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;

public class ButtonHoverManager : MonoBehaviour
{
    private bool isAnyButtonHovered = false;
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI numText;
    private StatManager statManager;
    private StatDetailManager statDetail;
    private MoveCount moveCount;

    private void Awake()
    {
        titleText = GameObject.Find("TitleText").GetComponent<TextMeshProUGUI>();
        numText = GameObject.Find("NumText").GetComponent<TextMeshProUGUI>();
        statManager = GameObject.Find("StatDataManager").GetComponent<StatManager>();
        statDetail = GameObject.Find("StatDetailManager").GetComponent<StatDetailManager>();
        moveCount = GameObject.Find("MoveCountManager").GetComponent<MoveCount>();
        numText.text = moveCount.movecountPublic.ToString();
    }

    public void SetHoverState(bool isHovered, int num)
    {
        if (isHovered)
        {
            isAnyButtonHovered = true;
            Debug.Log("At least one button is hovered");
            switch (num)
            {
                case 1:
                    titleText.text = "지능";
                    numText.text = statManager.statJsonList.Items[0].Stats.Jineung.ToString();
                    //Debug.Log("지능: " + numText.text);
                    break;
                case 2:
                    titleText.text = "인성";
                    numText.text = statManager.statJsonList.Items[0].Stats.Insung.ToString();
                    //Debug.Log("인성: " + numText.text);
                    break;
                case 3:
                    titleText.text = "신체";
                    numText.text = statManager.statJsonList.Items[0].Stats.Sinche.ToString();
                    //Debug.Log("신체: " + numText.text);
                    break;
                case 4:
                    titleText.text = "직감";
                    numText.text = statManager.statJsonList.Items[0].Stats.Jikkam.ToString();
                    //Debug.Log("직감: " + numText.text);
                    break;
                case 5:
                    titleText.text = "행운";
                    numText.text = statManager.statJsonList.Items[0].Stats.Luck.ToString();
                    //Debug.Log("행운: " + numText.text);
                    break;
                case 6:
                    titleText.text = "염력";
                    numText.text = statManager.statJsonList.Items[0].Stats.Yeom.ToString();
                    //Debug.Log("염력: " + numText.text);
                    break;
            }
        }
        else
        {
            // 모든 버튼이 hover 상태가 아닌지 확인하기 위해 검사
            titleText.text = "행동력";
            numText.text = moveCount.movecountPublic.ToString();
        }
    }
}