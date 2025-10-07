using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ScreenOptionManager : MonoBehaviour
{
    public TextMeshProUGUI SashaJob;
    public TextMeshProUGUI Sanity;
    public TextMeshProUGUI Stat;
    public StatManager statManager;

    public Image backgroundImage; // 텍스트의 부모 배경 이미지

    public Coroutine glitchCoroutine;
    public Coroutine fadeInCoroutine;
    public Coroutine fadeOutCoroutine;

    public CanvasGroup stateCanvasGroup;

    private bool isVisible = true; // 현재 오브젝트의 가시 상태
    private bool isFading = false; // 페이드 동작 상태
    private bool isOnSpace;

    public GameObject onSpaceGameObject; //이벤트 트리거 연결된 오브젝트
    private OptionImg1 optionImg1;

    private void OnEnable()
    {
        //Debug.Log("OnEnable");
        // StatManager 초기화
        statManager = FindObjectOfType<StatManager>();

        // CanvasGroup 초기화
        stateCanvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        optionImg1 = onSpaceGameObject.GetComponent<OptionImg1>();
        //isOnSpace = optionImg1.isPointerOver;
        //Debug.Log("isOnSpace: " + isOnSpace);
    }

    private void UpdateTextAndStat()
    {
        //Debug.Log("UpdateTextAndStat");
        SashaJob.text = $"<color=#FFFFFF>(염력 능력 보유, 특별 관리 대상)</color>";

        int sanity = statManager.statJsonList.Items[0].Sanity;

        if (sanity > 0 && sanity < 30)
        {
            Sanity.text = $"<color=#FF0000>Sanity {sanity}, 광기 상태</color>";
        }
        else if (sanity >= 30 && sanity < 38)
        {
            Sanity.text = $"<color=#FFFF00>Sanity {sanity}, 보통 상태</color>";
        }
        else if (sanity >= 38 && sanity < 67)
        {
            Sanity.text = $"<color=#0000FF>Sanity {sanity}, 이성 상태</color>";
        }

        Stat.text = $"염력 {statManager.statJsonList.Items[0].Stats.Yeom}  " +
                    $"지능 {statManager.statJsonList.Items[0].Stats.Jineung}  " +
                    $"직감 {statManager.statJsonList.Items[0].Stats.Jikkam}  " +
                    $"신체 {statManager.statJsonList.Items[0].Stats.Sinche}  " +
                    $"인성 {statManager.statJsonList.Items[0].Stats.Insung}  " +
                    $"행운 {statManager.statJsonList.Items[0].Stats.Luck}";
    }

    public void GlitchOn()
    {
        Debug.Log("Glitch On");
        if (!isVisible)
        {
            //isOnSpace = true;
            if (isFading) return; // 코루틴 실행 중이면 무시
            this.gameObject.SetActive(true);

            if (fadeInCoroutine != null) StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = StartCoroutine(FadeIn(0.1f));
            isVisible = true;
        }
    }

    public void GlitchOff()
    {
        if (isVisible)
        {
            //isOnSpace = false;
            if (isFading) return; // 코루틴 실행 중이면 무시

            if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = StartCoroutine(FadeOut(0.5f));
            isVisible = false;
        }
    }

    void Update()
    {
        UpdateTextAndStat();
        isOnSpace = optionImg1.isPointerOver;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q is clicked and isOnSpace: " + isOnSpace);
            if (!isOnSpace)
            {
                // 페이드가 진행 중이면 중단
                if (isFading)
                {
                    if (fadeInCoroutine != null) StopCoroutine(fadeInCoroutine);
                    if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);
                    isFading = false;
                }

                // 현재 상태 반전
                if (!isVisible)
                {
                    fadeInCoroutine = StartCoroutine(FadeIn(0.1f));
                    isVisible = !isVisible; // 상태 반전
                }
                else
                {
                    fadeOutCoroutine = StartCoroutine(FadeOut(0.5f));
                    isVisible = !isVisible; // 상태 반전
                }
            }
            else return;
        }
    }

    private IEnumerator FadeIn(float duration)
    {
        isFading = true;
        float startAlpha = stateCanvasGroup.alpha;
        float endAlpha = 1.0f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            stateCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            yield return null;
        }
        stateCanvasGroup.alpha = endAlpha;
        isFading = false;
    }

    private IEnumerator FadeOut(float duration)
    {
        isFading = true;
        float startAlpha = stateCanvasGroup.alpha;
        float endAlpha = 0.0f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            stateCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            yield return null;
        }
        stateCanvasGroup.alpha = endAlpha;
        isFading = false;
    }
}