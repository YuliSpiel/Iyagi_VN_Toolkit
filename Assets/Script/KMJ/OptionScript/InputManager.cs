using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private float inputCooldown = 0.5f; // 입력 쿨다운 시간
    private float lastInputTime = 0f;  // 마지막 입력 시간

    // 이벤트 (다른 시스템이 구독할 수 있도록)
    public event Action OnNextDialogue;
    public event Action OnSkipDialogue;
    public event Action<int> OnScroll;
    public event Action OnToggleUI;
    public event Action OnCloseLogWindow;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Time.time - lastInputTime < inputCooldown) return;

        if (!DialogueSystem0.Instance.isDiceWindowOn && !LogforSave.Instance.isLogWindowOn && !DialogueSystem0.Instance.isTyping)
        {
            if (!DialogueSystem0.Instance.isSelectChoiced)
            {
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                {
                    OnSkipDialogue?.Invoke();
                }

                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    OnNextDialogue?.Invoke();
                }

                if (scrollInput < 0f)
                {
                    lastInputTime = Time.time;
                    OnScroll?.Invoke(1);
                }
                else if (scrollInput > 0f)
                {
                    OnScroll?.Invoke(-1);
                }

                if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.H))
                {
                    OnToggleUI?.Invoke();
                }
            }
        }
        else if (LogforSave.Instance.isLogWindowOn)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnCloseLogWindow?.Invoke();
            }
        }
    }
}
