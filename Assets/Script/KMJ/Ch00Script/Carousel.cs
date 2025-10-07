using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlotCarousel : MonoBehaviour
{
    public RectTransform[] slots; // 슬롯 6개
    public Button leftButton, rightButton;
    public float moveDuration = 0.5f; // 이동 속도

    private int centerIndex = 1; // 현재 중앙 슬롯 인덱스 (1부터 시작)
    private bool isMoving = false;
    private CanvasGroup[] canvasGroups; // 투명도 조절

    void Start()
    {
        canvasGroups = new CanvasGroup[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            canvasGroups[i] = slots[i].GetComponent<CanvasGroup>();
        }

        ArrangeSlots(); // 초기 배치
        leftButton.onClick.AddListener(() => Move(-1));
        rightButton.onClick.AddListener(() => Move(1));
    }

    void Move(int direction)
    {
        if (isMoving) return;

        centerIndex = (centerIndex + direction + slots.Length) % slots.Length;
        StartCoroutine(MoveSlots());
    }

    IEnumerator MoveSlots()
    {
        isMoving = true;
        float elapsed = 0f;

        Vector3[] startPositions = new Vector3[slots.Length];
        Vector3[] startScales = new Vector3[slots.Length];
        float[] startAlphas = new float[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            startPositions[i] = slots[i].anchoredPosition;
            startScales[i] = slots[i].localScale;
            startAlphas[i] = canvasGroups[i].alpha;
        }

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            for (int i = 0; i < slots.Length; i++)
            {
                int relativeIndex = GetRelativeIndex(i);
                slots[i].anchoredPosition = Vector3.Lerp(startPositions[i], GetTargetPosition(relativeIndex), t);
                slots[i].localScale = Vector3.Lerp(startScales[i], Vector3.one * GetScale(relativeIndex), t);
                canvasGroups[i].alpha = Mathf.Lerp(startAlphas[i], GetAlpha(relativeIndex), t);
            }
            yield return null;
        }

        // ✅ 중앙 슬롯을 최상위(앞쪽)로 배치
        for (int i = 0; i < slots.Length; i++)
        {
            int relativeIndex = GetRelativeIndex(i);
            if (relativeIndex == 0) // 중앙 슬롯
            {
                slots[i].SetAsLastSibling(); // 가장 앞쪽으로 이동
            }
        }

        isMoving = false;
    }


    void ArrangeSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            int relativeIndex = GetRelativeIndex(i);
            slots[i].anchoredPosition = GetTargetPosition(relativeIndex);
            slots[i].localScale = Vector3.one * GetScale(relativeIndex);
            canvasGroups[i].alpha = GetAlpha(relativeIndex);

            if (relativeIndex == 0) // 중앙 슬롯
            {
                slots[i].SetAsLastSibling(); // 가장 앞쪽으로 이동
            }
        }

    }

    int GetRelativeIndex(int slotIndex)
    {
        int left = (centerIndex - 1 + slots.Length) % slots.Length;
        int right = (centerIndex + 1) % slots.Length;

        if (slotIndex == centerIndex) return 0;  // 중앙 슬롯
        if (slotIndex == left) return -1;       // 왼쪽 슬롯
        if (slotIndex == right) return 1;       // 오른쪽 슬롯
        return 99;  // 숨길 슬롯
    }

    Vector3 GetTargetPosition(int relativeIndex)
    {
        if (relativeIndex == 0) return new Vector3(0, 0, 0);    // 중앙 슬롯
        if (relativeIndex == -1) return new Vector3(-300, 0, 0); // 왼쪽 슬롯
        if (relativeIndex == 1) return new Vector3(300, 0, 0);  // 오른쪽 슬롯
        return new Vector3(600 * relativeIndex, 0, 0); // ✅ 원래 위치를 유지하면서 멀리 배치
    }

    float GetScale(int relativeIndex)
    {
        if (relativeIndex == 0) return 1.2f;  // 중앙 슬롯
        if (relativeIndex == -1 || relativeIndex == 1) return 0.8f;  // 좌우 슬롯
        return 0f; // 숨긴 슬롯은 크기 0
    }

    float GetAlpha(int relativeIndex)
    {
        if (relativeIndex == 0) return 1f;  // 중앙 슬롯 (완전 선명)
        if (relativeIndex == -1 || relativeIndex == 1) return 0.5f;  // 양옆 슬롯 (약간 흐림)
        return 0f; // 나머지 슬롯 (완전히 숨기지 않지만 거의 안 보이게)
    }

}
