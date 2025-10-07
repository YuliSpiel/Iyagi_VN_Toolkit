using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonKeyboardHandler : MonoBehaviour
{
    public Button targetButton;

    void Update()
    {
        // 스페이스바 또는 Enter 키 입력 감지
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (targetButton != null)
            {
                // 버튼의 OnClick 이벤트 수동 호출
                targetButton.onClick.Invoke();
            }
        }
    }
}
