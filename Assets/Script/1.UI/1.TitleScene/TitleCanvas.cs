using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// MVP패턴의 View에 해당
public class TitleCanvas : MonoBehaviour
{
    public Button startButton;
    public Button loadButton;
    public Button ectButton;
    public Button settingsButton;
    public Button exitButton;
    
    public GameObject saveSlotsPanel; 
    public GameObject ectPanel;
    public GameObject settingsPanel;
    
    void Awake()
    {
        CloseAllPanel();
    }

    void OnEnable()
    {
        CloseAllPanel(); // 모든 패널 강제 비활성화
    }

    public void OpenOnlyPanel(GameObject panel)
    {
        CloseAllPanel();
        panel.SetActive(true);
    }

    public void OpenSaveSlotsPanel()
    {
        CloseAllPanel();
        saveSlotsPanel.SetActive(true);
        saveSlotsPanel.GetComponent<SaveSlotCanvas>().ShowSlots(1);
    }

    // 임시방편
    // public void OpenOnlyPanel(GameObject panel)
    // {
    //     CloseAllPanel();
    //     panel.SetActive(true);
    //
    //     if (panel.TryGetComponent<SaveSlotCanvas>(out var saveSlotCanvas))
    //     {
    //         saveSlotCanvas.ShowSlots(1);
    //     }
    // }


    // private IEnumerator FadeAndOpenCoroutine(GameObject panel, GameObject fadePanel)
    // {
    //     yield return UIManager.Instance.FadeInCoroutine(fadePanel, 1f);
    //     panel.SetActive(true);
    //     yield return UIManager.Instance.FadeOutCoroutine(fadePanel, 1f);
    // }

    public void CloseAllPanel()
    {
        saveSlotsPanel.SetActive(false);
        ectPanel.SetActive(false);
        settingsPanel.SetActive(false);    
    }

    public void EndGame()
    {
        GameManager.Instance.ExitGame();
    }
}
