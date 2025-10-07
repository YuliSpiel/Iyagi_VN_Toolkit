using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Sprite quickOnImage;
    [SerializeField] Sprite quickOffImage;
    Image image;

    // Start is called before the first frame update
    // 마우스가 버튼 위에 올라갔을 때 호출됩니다.
    private void Awake()
    {
        image = this.gameObject.GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = quickOnImage;
    }

    // 마우스가 버튼에서 벗어났을 때 호출됩니다.
    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = quickOffImage;
    }

}
