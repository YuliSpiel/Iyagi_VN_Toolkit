using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class DialogueImageSetting : MonoBehaviour
{
    [SerializeField] OptionManager optionManager;

    public Image[] charImage1_s;
    public Image[] charImage2_s;
    public Image[] charImage3_s;

    public Image[] charImage1_m;
    public Image[] charImage2_m;
    public Image[] charImage3_m;

    public Image[] charImage1_l;

    //backgroundIm
    public Image backgroundImg;

    //UI
    public TextMeshProUGUI dialogueText;
    public GameObject nextButton;
    public GameObject dialBox;
    public Image nameBox;

    public float fadeDuration = 0.13f;

    // Character Image
    public Sprite charNameSprite;

    public string prevClothes, prevEyes, prevMouth;
    public string prevClothes1, prevEyes1, prevMouth1;

    public class CharacterImageState
    {
        public string Clothes { get; set; }
        public string Eyes { get; set; }
        public string Mouth { get; set; }
    }

    public CharacterImageState state1 = new CharacterImageState();
    public CharacterImageState state2 = new CharacterImageState();
    public CharacterImageState state3 = new CharacterImageState();
    public CharacterImageState state4 = new CharacterImageState();
    public CharacterImageState state5 = new CharacterImageState();
    public CharacterImageState state6 = new CharacterImageState();
    public CharacterImageState state7 = new CharacterImageState();

    public Transform charImg1Transform_s;
    public Transform charImg2Transform_s;
    public Transform charImg3Transform_s;

    public Transform charImg1Transform_m;
    public Transform charImg2Transform_m;
    public Transform charImg3Transform_m;

    public Transform charImg1Transform_l;

    private string backImg;
    private string previousBackgroundImg = ""; // 이전 배경 이미지를 저장할 변수

    public void DialogueGraphicSetting()
    {
        charImage1_s = new Image[5];
        charImage2_s = new Image[5];
        charImage3_s = new Image[5];

        charImage1_m = new Image[5];
        charImage2_m = new Image[5];
        charImage3_m = new Image[5];

        charImage1_l = new Image[5];

        for (int i = 0; i < 5; i++)
        {
            charImage1_s[i] = charImg1Transform_s.GetChild(i).GetComponent<Image>();
            charImage2_s[i] = charImg2Transform_s.GetChild(i).GetComponent<Image>();
            charImage3_s[i] = charImg3Transform_s.GetChild(i).GetComponent<Image>();

            charImage1_m[i] = charImg1Transform_m.GetChild(i).GetComponent<Image>();
            charImage2_m[i] = charImg2Transform_m.GetChild(i).GetComponent<Image>();
            charImage3_m[i] = charImg3Transform_m.GetChild(i).GetComponent<Image>();

            charImage1_l[i] = charImg1Transform_l.GetChild(i).GetComponent<Image>();
        }
    }

    public IEnumerator SetCharacterImages(DialogueEntry entry)
    {
        // BackgroundImg가 null이면 이전 배경을 사용
        if (entry.BackgroundImg != "")
        {
            Debug.Log("Background is not null");
            backImg = entry.BackgroundImg;
            previousBackgroundImg = backImg; // 새로운 배경을 설정하면서 이전 배경을 업데이트
            Debug.Log("BackIMg is " + backImg);
            Debug.Log("previousBackgroundImg " + previousBackgroundImg);
        }
        else if (entry.BackgroundImg == "")
        {
            Debug.Log("Background is null");
            Debug.Log("previousBackgroundImg " + previousBackgroundImg);
            backImg = previousBackgroundImg; // 이전 배경 사용
        }

        // 배경 이미지 불러오기
        backgroundImg.sprite = Resources.Load<Sprite>($"Images/Background/{backImg}");

        Image dialImg = dialBox.GetComponent<Image>();
        // 현재 이미지 색상을 가져옴
        Color currentColor = dialImg.color;
        // TypingAlpha 값을 0~100 범위에서 0~1 범위로 변환
        float alphaValue = optionManager.TypingAlpha / 100f;
        // 기존 색상에서 alpha만 변경
        dialImg.color = new Color(currentColor.r, currentColor.g, currentColor.b, alphaValue);

        // Handle name box
        if (entry.CharName != null)
        {
            charNameSprite = Resources.Load<Sprite>($"Images/NameTag/{entry.CharName}");
            if (charNameSprite != null)
            {
                nameBox.sprite = charNameSprite;
                nameBox.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"CharName 이미지 찾을 수 없음: {entry.CharName}");
                nameBox.gameObject.SetActive(false);
            }
        }
        else
        {
            nameBox.gameObject.SetActive(false);
        }

        // Set image visibility based on number of characters
        if (entry.PeopleNum == 1)
        {
            // Show only one set of images and hide the other
            //Debug.Log("PeopleNum is 1");
            if (entry.ImgName.Size == "s")
            {
                SetImagesVisibility(charImage1_s, true);
                SetImagesVisibility(charImage2_s, false);
                SetImagesVisibility(charImage3_s, false);

                SetImagesVisibility(charImage1_m, false);
                SetImagesVisibility(charImage2_m, false);
                SetImagesVisibility(charImage3_m, false);

                SetImagesVisibility(charImage1_l, false);

                //이미지 위치 조정
                CharPositionSetting(charImg1Transform_s, entry.PeopleNum, entry.ImgName.Char, "s", "center");

                // Load and fade images for the single character
                yield return StartCoroutine(LoadCharacterImagesWithFade(charImage1_s, entry.ImgName, state1));
            }
            else if (entry.ImgName.Size == "m")
            {
                SetImagesVisibility(charImage1_s, false);
                SetImagesVisibility(charImage2_s, false);
                SetImagesVisibility(charImage3_s, false);

                SetImagesVisibility(charImage1_m, true);
                SetImagesVisibility(charImage2_m, false);
                SetImagesVisibility(charImage3_m, false);

                SetImagesVisibility(charImage1_l, false);

                //이미지 위치 조정
                CharPositionSetting(charImg1Transform_m, entry.PeopleNum, entry.ImgName.Char, "m", "center");

                // Load and fade images for the single character
                yield return StartCoroutine(LoadCharacterImagesWithFade(charImage1_m, entry.ImgName, state4));
            }
            else if (entry.ImgName.Size == "l")
            {
                SetImagesVisibility(charImage1_s, false);
                SetImagesVisibility(charImage2_s, false);
                SetImagesVisibility(charImage3_s, false);

                SetImagesVisibility(charImage1_m, false);
                SetImagesVisibility(charImage2_m, false);
                SetImagesVisibility(charImage3_m, false);

                SetImagesVisibility(charImage1_l, true);

                //이미지 위치 조정
                CharPositionSetting(charImg1Transform_l, entry.PeopleNum, entry.ImgName.Char, "l", "center");

                // Load and fade images for the single character
                yield return StartCoroutine(LoadCharacterImagesWithFade(charImage1_l, entry.ImgName, state7));
            }
        }
        else if (entry.PeopleNum == 2)
        {
            //Debug.Log("PeopleNum is 2");
            // Show both sets of images
            SetImagesVisibility(charImage1_l, false);
            if (entry.ImgName.Size == "s")
            {
                SetImagesVisibility(charImage1_s, false);
                SetImagesVisibility(charImage2_s, true);
                SetImagesVisibility(charImage3_s, true);

                SetImagesVisibility(charImage1_m, false);
                SetImagesVisibility(charImage2_m, false);
                SetImagesVisibility(charImage3_m, false);

                //이미지 위치 조정
                CharPositionSetting(charImg2Transform_s, entry.PeopleNum, entry.ImgName.Char, "s", "left");
                CharPositionSetting(charImg3Transform_s, entry.PeopleNum, entry.ImgName2.Char, "s", "right");

                Coroutine fadeCoroutine1 = StartCoroutine(LoadCharacterImagesWithFade(charImage2_s, entry.ImgName, state2));
                Coroutine fadeCoroutine2 = StartCoroutine(LoadCharacterImagesWithFade(charImage3_s, entry.ImgName2, state3));

                // Wait for both fading coroutines to complete
                yield return fadeCoroutine1;
                yield return fadeCoroutine2;
            }
            else if (entry.ImgName.Size == "m")
            {
                SetImagesVisibility(charImage1_s, false);
                SetImagesVisibility(charImage2_s, false);
                SetImagesVisibility(charImage3_s, false);

                SetImagesVisibility(charImage1_m, false);
                SetImagesVisibility(charImage2_m, true);
                SetImagesVisibility(charImage3_m, true);

                //이미지 위치 조정
                CharPositionSetting(charImg2Transform_m, entry.PeopleNum, entry.ImgName.Char, "m", "left");
                CharPositionSetting(charImg3Transform_m, entry.PeopleNum, entry.ImgName2.Char, "m", "right");

                Coroutine fadeCoroutine1 = StartCoroutine(LoadCharacterImagesWithFade(charImage2_m, entry.ImgName, state5));
                Coroutine fadeCoroutine2 = StartCoroutine(LoadCharacterImagesWithFade(charImage3_m, entry.ImgName2, state6));

                // Wait for both fading coroutines to complete
                yield return fadeCoroutine1;
                yield return fadeCoroutine2;
            }
        }
        else
        {
            //Debug.Log("PeopleNum is 0");
            // Hide all images if no characters are present
            SetImagesVisibility(charImage1_s, false);
            SetImagesVisibility(charImage2_s, false);
            SetImagesVisibility(charImage3_s, false);

            SetImagesVisibility(charImage1_m, false);
            SetImagesVisibility(charImage2_m, false);
            SetImagesVisibility(charImage3_m, false);

            SetImagesVisibility(charImage1_l, false);
        }
    }

    public void CharPositionSetting(Transform charImgTransform, int peopleNum, string imgName, string size, string position)
    {
        RectTransform rectCharImgTransform = charImgTransform as RectTransform;
        //Debug.Log("This function is CharPositionSetting");
        //Debug.Log(imgName.Char + " " + size + " " + position);
        //large size
        if (size == "l")
        {
            //캐릭터 위치 조정
            if (imgName == "Sasha")
            {
                rectCharImgTransform.anchoredPosition = new Vector2(-74, -190); //(-74, -190)
                //Debug.Log("Sasha x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
            }
            else if (imgName == "Edan")
            {
                rectCharImgTransform.anchoredPosition = new Vector2(0, -426); //(0, -426) 
                //Debug.Log("Edan x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
            }
            else if (imgName == "Sion")
            {
                rectCharImgTransform.anchoredPosition = new Vector2(0, -460); //(0, -460)
                //Debug.Log("Sion x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
            }
        }
        //medium size
        else if (size == "m")
        {
            if (peopleNum == 1)
            {
                //캐릭터 위치 조정
                if (imgName == "Sasha")
                {
                    rectCharImgTransform.anchoredPosition = new Vector2(-51, -17); //(-51, -17)
                    //Debug.Log("Sasha x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
                }
                else if (imgName == "Edan")
                {
                    rectCharImgTransform.anchoredPosition = new Vector2(-18, -187); //(0, -187)
                    //Debug.Log("Edan x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
                }
                else if (imgName == "Sion")
                {
                    float sionPosY = (float)-214.5;
                    rectCharImgTransform.anchoredPosition = new Vector2(0, sionPosY); //(0, -214.5)
                    //Debug.Log("Sion x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
                }
            }
            else if (peopleNum == 2)
            {
                float PosY = (float)-214.5;
                if (position == "left")
                {
                    //캐릭터 위치 조정
                    if (imgName == "Sasha")
                    {
                        rectCharImgTransform.anchoredPosition = new Vector2(-451, PosY); //(-451, -214.5)
                        //Debug.Log("Sasha x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
                    }
                    else if (imgName == "Edan")
                    {
                        rectCharImgTransform.anchoredPosition = new Vector2(-414, PosY); //(-424, -214.5)
                        //Debug.Log("Edan x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
                    }
                    else if (imgName == "Sion")
                    {
                        rectCharImgTransform.anchoredPosition = new Vector2(-397, PosY); //(-397, -214.5)
                        //Debug.Log("Sion x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
                    }
                }
                else if (position == "right")
                {
                    //캐릭터 위치 조정
                    if (imgName == "Sasha")
                    {
                        rectCharImgTransform.anchoredPosition = new Vector2(361, PosY); //(361, -214.5)
                        //Debug.Log("Sasha x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
                    }
                    else if (imgName == "Edan")
                    {
                        rectCharImgTransform.anchoredPosition = new Vector2(378, PosY); //(378, -214.5)
                        //Debug.Log("Edan x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
                    }
                    else if (imgName == "Sion")
                    {
                        rectCharImgTransform.anchoredPosition = new Vector2(426, PosY); //(426, -214.5)
                        //Debug.Log("Sion x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
                    }
                }
            }
        }
        //small size
        else if (size == "s")
        {
            if (peopleNum == 1)
            {
                //캐릭터 위치 조정
                if (imgName == "Sasha")
                {
                    rectCharImgTransform.anchoredPosition = new Vector2(-41, -17); //(-41, -14)
                    //Debug.Log("Sasha x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
                }
                else if (imgName == "Edan")
                {
                    rectCharImgTransform.anchoredPosition = new Vector2(-41, -17); //(-41, -17)
                    //Debug.Log("Edan x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
                }
                else if (imgName == "Sion")
                {
                    rectCharImgTransform.anchoredPosition = new Vector2(-17, -14); //(-17, -14)
                    //Debug.Log("Sion x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
                }
            }
            else if (peopleNum == 2)
            {
                float PosY = (float)-14;
                if (position == "left")
                {
                    //캐릭터 위치 조정
                    if (imgName == "Sasha")
                    {
                        rectCharImgTransform.anchoredPosition = new Vector2(-381, PosY); //(-381, -14)
                        //Debug.Log("Sasha x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
                    }
                    else if (imgName == "Edan")
                    {
                        rectCharImgTransform.anchoredPosition = new Vector2(-373, PosY); //(-373, -14)
                        //Debug.Log("Edan x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
                    }
                    else if (imgName == "Sion")
                    {
                        rectCharImgTransform.anchoredPosition = new Vector2(-376, PosY); //(-376, -14)
                        //Debug.Log("Sion x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
                    }
                }
                else if (position == "right")
                {
                    //캐릭터 위치 조정
                    if (imgName == "Sasha")
                    {
                        rectCharImgTransform.anchoredPosition = new Vector2(301, PosY); //(301, -14)
                        //Debug.Log("Sasha x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
                    }
                    else if (imgName == "Edan")
                    {
                        rectCharImgTransform.anchoredPosition = new Vector2(327, PosY); //(327, -14)
                        //Debug.Log("Edan x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
                    }
                    else if (imgName == "Sion")
                    {
                        rectCharImgTransform.anchoredPosition = new Vector2(354, PosY); //(354, -14)
                        //Debug.Log("Sion x: " + rectCharImgTransform.anchoredPosition.x + " y: " + rectCharImgTransform.anchoredPosition.y);
                    }
                }
            }
        }
    }

    public IEnumerator LoadCharacterImagesWithFade(Image[] imageArray, CharacterImage imgName, CharacterImageState state)
    {
        string newClothes = $"{imgName.Char}_{imgName.Clothes}_{imgName.Pose}";
        string newEyes = $"{imgName.Char}_{imgName.Eyes}";
        string newMouth = $"{imgName.Char}_{imgName.Mouth}";

        //Debug.Log("imgName is " + imgName);
        //Debug.Log("newClothes is " + newClothes);
        //Debug.Log("newEyes is " + newEyes);
        //Debug.Log("newMouth is " + newMouth);

        List<Coroutine> fadeCoroutines = new List<Coroutine>();

        // Start coroutines for images that need to be updated
        if (state.Clothes != newClothes)
        {
            Sprite newClothesSprite = Resources.Load<Sprite>($"Images/Standing/{imgName.Char}/Clothes/" + newClothes);
            if (newClothesSprite == null)
            {
                Debug.LogError("Clothes sprite not found: " + newClothes);
            }
            fadeCoroutines.Add(StartCoroutine(FadeImage(imageArray[0], newClothesSprite)));
        }
        if (state.Eyes != newEyes)
        {
            Sprite newEyesSprite = Resources.Load<Sprite>($"Images/Standing/{imgName.Char}/Eyes/" + newEyes);
            if (newEyesSprite == null)
            {
                Debug.LogError("Eyes sprite not found: " + newEyes);
            }
            fadeCoroutines.Add(StartCoroutine(FadeImage(imageArray[1], newEyesSprite)));
        }
        if (state.Mouth != newMouth)
        {
            Sprite newMouthSprite = Resources.Load<Sprite>($"Images/Standing/{imgName.Char}/Mouth/" + newMouth);
            if (newMouthSprite == null)
            {
                Debug.LogError("Mouth sprite not found: " + newMouth);
            }
            fadeCoroutines.Add(StartCoroutine(FadeImage(imageArray[2], newMouthSprite)));
        }

        // Wait for all image fading coroutines to complete
        foreach (var fadeCoroutine in fadeCoroutines)
        {
            yield return fadeCoroutine;
        }

        // Update the state after processing images
        UpdateState(state, newClothes, newEyes, newMouth);

        // Handle Blush
        if (imgName.Blush != null)
        {
            string newBlush = $"{imgName.Char}_{imgName.Blush}";
            Sprite blushSprite = Resources.Load<Sprite>($"Images/Standing/{imgName.Char}/Blush/" + newBlush);
            if (blushSprite != null)
            {
                imageArray[3].gameObject.SetActive(true);
                yield return StartCoroutine(FadeImage(imageArray[4], blushSprite));
            }
        }
        else
        {
            imageArray[3].gameObject.SetActive(false);
        }

        // Handle Scar
        if (imgName.Scar != null)
        {
            string newScar = $"{imgName.Char}_{imgName.Scar}";
            Debug.Log("newScar" + newScar);
            Sprite scarSprite = Resources.Load<Sprite>($"Images/Standing/{imgName.Char}/Scar/" + newScar);
            if (scarSprite != null)
            {
                imageArray[4].gameObject.SetActive(true);
                yield return StartCoroutine(FadeImage(imageArray[4], scarSprite));
            }
        }
        else
        {
            imageArray[4].gameObject.SetActive(false);
        }
    }

    IEnumerator FadeImage(Image image, Sprite newSprite)
    {
        if (image == null || newSprite == null)
            yield break;

        // 현재 이미지와 새 이미지가 같으면 페이드 효과를 생략합니다.
        if (image.sprite == newSprite)
        {
            yield break;
        }

        // 페이드 아웃
        Color startColor = image.color;
        Color fadeOutEndColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // 투명 색상
        float halfFadeDuration = fadeDuration / 2f;
        float elapsedTime = 0f;

        while (elapsedTime < halfFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0.5f, 1f, elapsedTime / halfFadeDuration);
            image.color = Color.Lerp(startColor, fadeOutEndColor, t);
            yield return null;
        }

        // 스프라이트 변경
        image.sprite = newSprite;
        Color fadeInStartColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // 투명 색상
        Color fadeInEndColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // 불투명 색상

        // 페이드 아웃과 페이드 인을 동시에 처리하는 루프
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float tOut = Mathf.SmoothStep(0.5f, 0f, (elapsedTime - halfFadeDuration) / halfFadeDuration);
            float tIn = Mathf.SmoothStep(0.5f, 1f, (elapsedTime - halfFadeDuration) / halfFadeDuration);

            image.color = Color.Lerp(fadeInStartColor, fadeInEndColor, tIn);

            yield return null;
        }
        image.color = fadeInEndColor; // 최종 불투명 상태 설정
    }

    void UpdateState(CharacterImageState state, string newClothes, string newEyes, string newMouth)
    {
        state.Clothes = newClothes;
        state.Eyes = newEyes;
        state.Mouth = newMouth;
    }

    void SetImagesVisibility(Image[] images, bool isVisible)
    {
        for (int i = 0; i < 3; i++)
        {
            images[i].gameObject.SetActive(isVisible);
        }
        images[3].gameObject.SetActive(false);
        images[4].gameObject.SetActive(false);
    }

}