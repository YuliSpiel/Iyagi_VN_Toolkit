using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StatOKBTN : MonoBehaviour
{
    private TextMeshProUGUI yeomText;
    private TextMeshProUGUI sinText;
    private TextMeshProUGUI jineungText;
    private TextMeshProUGUI insungText;
    private TextMeshProUGUI jikkamText;
    private TextMeshProUGUI luckText;

    private int yeomNum;
    private int sinNum;
    private int jineungNum;
    private int insungNum;
    private int jikkamNum;
    private int luckNum;

    public GameObject alertBox;
    public TextMeshProUGUI alertText;

    private int sum;

    public StatManager statManager;
    private CanvasGroup alertCanvasGroup;

    public string nextSceneName;

    // Start is called before the first frame update
    void Start()
    {
        alertText = alertBox.GetComponentInChildren<TextMeshProUGUI>();
        alertCanvasGroup = alertBox.GetComponent<CanvasGroup>();
        if (alertCanvasGroup == null)
        {
            alertCanvasGroup = alertBox.AddComponent<CanvasGroup>();
        }
        alertBox.SetActive(false);
    }

    public void SumCheck()
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

        sum = yeomNum + sinNum + jineungNum + insungNum + jikkamNum + luckNum;
        Debug.Log("Sum is " + sum);

        if (sum == 18)
        {
            foreach (StatData stat in statManager.statJsonList.Items)
            {
                stat.Stats.Yeom = yeomNum;
                stat.Stats.Sinche = sinNum;
                stat.Stats.Jineung = jineungNum;
                stat.Stats.Insung = insungNum;
                stat.Stats.Jikkam = jikkamNum;
                stat.Stats.Luck = luckNum;
                stat.Sanity = 34 - (yeomNum + luckNum + jikkamNum) + jineungNum + insungNum + sinNum;
            }
            statManager.StatSave();
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            StartCoroutine(FadeOutAlertText());
        }
    }
    private IEnumerator FadeOutAlertText()
    {

        alertBox.SetActive(true);
        alertCanvasGroup.alpha = 0f;

        // 0.5초 동안 페이드 인
        for (float t = 0.0f; t < 0.5f; t += Time.deltaTime)
        {
            alertCanvasGroup.alpha = Mathf.Lerp(0, 1, t / 0.5f);
            yield return null;
        }
        alertCanvasGroup.alpha = 1f;

        // 2초간 유지
        yield return new WaitForSeconds(2);

        // 0.5초 동안 페이드 아웃
        for (float t = 0.0f; t < 0.5f; t += Time.deltaTime)
        {
            alertCanvasGroup.alpha = Mathf.Lerp(1, 0, t / 0.5f);
            yield return null;
        }
        alertCanvasGroup.alpha = 0f;
        alertBox.SetActive(false);
    }
}