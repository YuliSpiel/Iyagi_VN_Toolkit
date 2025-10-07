using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RandomDiceBTN : MonoBehaviour
{
    public TextMeshProUGUI yeomText;
    public TextMeshProUGUI sinText;
    public TextMeshProUGUI jineungText;
    public TextMeshProUGUI insungText;
    public TextMeshProUGUI jikkamText;
    public TextMeshProUGUI luckText;

    private int yeomNum;
    private int sinNum;
    private int jineungNum;
    private int insungNum;
    private int jikkamNum;
    private int luckNum;

    private int sum;

    private int min = 2;  // 주사위 눈금의 최솟값
    private int max = 4;  // 주사위 눈금의 최댓값

    // Start is called before the first frame update
    void Start()
    {
        GameObject tempObject = GameObject.Find("YeomNum");
        yeomText = tempObject.GetComponent<TextMeshProUGUI>();
        yeomNum = int.Parse(yeomText.text);

        tempObject = GameObject.Find("SinNum");
        sinText = tempObject.GetComponent<TextMeshProUGUI>();
        sinNum = int.Parse(sinText.text);

        tempObject = GameObject.Find("JiNum");
        jineungText = tempObject.GetComponent<TextMeshProUGUI>();
        jineungNum = int.Parse(jineungText.text);

        tempObject = GameObject.Find("InNum");
        insungText = tempObject.GetComponent<TextMeshProUGUI>();
        insungNum = int.Parse(insungText.text);

        tempObject = GameObject.Find("JikNum");
        jikkamText = tempObject.GetComponent<TextMeshProUGUI>();
        jikkamNum = int.Parse(jikkamText.text);

        tempObject = GameObject.Find("LuckNum");
        luckText = tempObject.GetComponent<TextMeshProUGUI>();
        luckNum = int.Parse(luckText.text);
    }

    public void RandomDice()
    {
        int total = 0;

        yeomNum = Random.Range(min, max + 1);
        sinNum = Random.Range(min, max + 1); ;
        jineungNum = Random.Range(min, max + 1);
        insungNum = Random.Range(min, max + 1);
        jikkamNum = Random.Range(min, max + 1);
        luckNum = Random.Range(min, max + 1);

        total = yeomNum + sinNum + jineungNum + insungNum + jikkamNum + luckNum;

        if (total == 18)
        {
            yeomText.text = yeomNum.ToString();
            sinText.text = sinNum.ToString();
            jineungText.text = jineungNum.ToString();
            insungText.text = insungNum.ToString();
            jikkamText.text = jikkamNum.ToString();
            luckText.text = luckNum.ToString();
        }
        else
            RandomDice();
    }
}
