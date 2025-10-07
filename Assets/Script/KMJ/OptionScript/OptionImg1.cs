using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionImg1 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isPointerOver = false; // 상태를 관리하는 변수

    // 이벤트를 자식들에게 전달할 델리게이트
    public delegate void PointerStateChanged(bool isOver);
    public event PointerStateChanged OnPointerStateChanged;

    // PointerEnter 이벤트 처리
    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        Debug.Log("Pointer Entered on Parent: " + isPointerOver);

        // 상태 변경 알림
        OnPointerStateChanged?.Invoke(isPointerOver);
    }

    // PointerExit 이벤트 처리
    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        Debug.Log("Pointer Exited on Parent: " + isPointerOver);

        // 상태 변경 알림
        OnPointerStateChanged?.Invoke(isPointerOver);
    }
}