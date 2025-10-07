using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private DialogueSystem0 _dialogueSystem0;
    public int num;

    void Start()
    {
        // ButtonHoverManager 스크립트를 찾아서 참조합니다.
        _dialogueSystem0 = FindObjectOfType<DialogueSystem0>();
    }

    // 마우스가 버튼 위에 올라갔을 때 호출됩니다.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_dialogueSystem0 != null)
        {
            _dialogueSystem0.SelctionHover(true, num);
        }
    }

    // 마우스가 버튼에서 벗어났을 때 호출됩니다.
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_dialogueSystem0 != null)
        {
            _dialogueSystem0.SelctionHover(false, num);
        }
    }
}
