using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ButtonHoverManager hoverManager;
    public int num;

    void Start()
    {
        // ButtonHoverManager 스크립트를 찾아서 참조합니다.
        hoverManager = FindObjectOfType<ButtonHoverManager>();
    }

    // 마우스가 버튼 위에 올라갔을 때 호출됩니다.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverManager != null)
        {
            hoverManager.SetHoverState(true, num);
        }
    }

    // 마우스가 버튼에서 벗어났을 때 호출됩니다.
    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverManager != null)
        {
            hoverManager.SetHoverState(false, num);
        }
    }
}