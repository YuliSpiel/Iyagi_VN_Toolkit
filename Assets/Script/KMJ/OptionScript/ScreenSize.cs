using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSize : MonoBehaviour
{
    private bool isFullScreen = true; // 초기 전체화면 상태 (true = 전체화면, false = 창모드)

    // 화면 전환 함수
    public void ToggleFullScreen()
    {
        if (isFullScreen)
        {
            // 창모드로 전환, 해상도 1920x1080
            Screen.SetResolution(1920, 1080, false);
        }
        else
        {
            // 전체화면으로 전환
            Screen.SetResolution(1920, 1080, true);
        }

        // 전체화면 상태 토글
        isFullScreen = !isFullScreen;
    }
}
