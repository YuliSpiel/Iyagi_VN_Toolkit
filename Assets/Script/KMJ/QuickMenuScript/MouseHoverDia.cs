using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseHoverDia : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject highlightPrefab; // 하이라이트 이미지 프리팹
    private GameObject leftHighlight, rightHighlight; // 생성된 하이라이트 오브젝트
    [SerializeField] TextMeshProUGUI tmpText; // 버튼 내 TMP 텍스트 참조
    private RectTransform tmpRectTransform; // TMP 위치 정보
    private FontStyles originalFontStyle; // 원래 폰트 스타일 저장

    void Start()
    {
        // 버튼 내부에서 TextMeshPro 찾기
        tmpText = GetComponentInChildren<TextMeshProUGUI>();
        if (tmpText)
        {
            tmpRectTransform = tmpText.GetComponent<RectTransform>();
            originalFontStyle = tmpText.fontStyle; // 기본 폰트 스타일 저장
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightPrefab == null || tmpRectTransform == null) return;

        // 좌측 하이라이트 생성
        leftHighlight = Instantiate(highlightPrefab, transform);
        PositionHighlight(leftHighlight, -1);

        // 우측 하이라이트 생성
        rightHighlight = Instantiate(highlightPrefab, transform);
        PositionHighlight(rightHighlight, 1);
        // TMP 언더라인 활성화
        tmpText.fontStyle |= FontStyles.Underline;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (leftHighlight) Destroy(leftHighlight);
        if (rightHighlight) Destroy(rightHighlight);
        // TMP 언더라인 제거 (원래 스타일로 복원)
        if (tmpText) tmpText.fontStyle = originalFontStyle;
    }

    private void PositionHighlight(GameObject highlight, int direction)
    {
        RectTransform highlightRT = highlight.GetComponent<RectTransform>();

        // 버튼 내부 텍스트 위치를 기준으로 상대적 위치 설정
        highlightRT.SetParent(tmpRectTransform.parent);
        highlightRT.SetAsFirstSibling(); // 버튼 뒤에 배치 가능

        // TMP 텍스트의 너비를 가져와서 위치 설정
        float textWidth = tmpRectTransform.rect.width;
        Debug.Log("***textwidth is " + textWidth);

        // 5px(또는 원하는 간격)만큼 떨어뜨려 배치
        highlightRT.anchoredPosition = new Vector2(
            tmpRectTransform.anchoredPosition.x + (direction * (textWidth / 2 + 30)), // 간격
            tmpRectTransform.anchoredPosition.y
        );
        Debug.Log("DiaPosition is " + highlightRT.anchoredPosition);

        highlightRT.localScale = Vector3.one;
    }
}