using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    private GameObject soundTestPanel;

    void Awake()
    {
        soundTestPanel = GameObject.Find("SoundTestPanel");
        Debug.Log(soundTestPanel);
        soundTestPanel.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            soundTestPanel.SetActive(!soundTestPanel.activeSelf);
        }
    }

    // 적용사례 : 아래와 같이 사운드가 필요한 상황에서 그때그때 사운드매니저의 인스턴스를 호출해 사용하시면 됩니다.
    public void OnClickBGM1()
    {
        SoundManager.Instance.PlayBGM("BGM_Sample1");
    }
    
    public void OnClickBGM2()
    {
        SoundManager.Instance.PlayBGM("BGM_Sample2");
    }
    
    // SFX도 같은 방식입니다.
    public void OnClickSFX1()
    {
        SoundManager.Instance.PlaySFX("SFX_Knock");
    }
    
    public void OnClickSFX2()
    {
        SoundManager.Instance.PlaySFX("SFX_WakeUp");
    }
}
