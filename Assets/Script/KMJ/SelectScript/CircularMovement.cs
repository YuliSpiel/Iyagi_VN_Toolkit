using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    public RectTransform[] buttons; // 6개의 버튼 위치
    private bool isInButtonZone = false; // 버튼 영역 안에 있는지 여부
    private bool canRotate = true; // 회전 가능 여부를 결정하는 플래그

    void Start()
    {
        // 버튼 위치 초기화
        buttons = new RectTransform[6];
        buttons[0] = GameObject.Find("piece (1)").GetComponent<RectTransform>();
        buttons[1] = GameObject.Find("piece (2)").GetComponent<RectTransform>();
        buttons[2] = GameObject.Find("piece (3)").GetComponent<RectTransform>();
        buttons[3] = GameObject.Find("piece (4)").GetComponent<RectTransform>();
        buttons[4] = GameObject.Find("piece (5)").GetComponent<RectTransform>();
        buttons[5] = GameObject.Find("piece (6)").GetComponent<RectTransform>();
    }
    private void Update()
    {
        // 버튼 영역 여부 확인
        bool wasInButtonZone = isInButtonZone;
        isInButtonZone = false;

        foreach (RectTransform button in buttons)
        {
            // 현재 RectTransform이 버튼 영역 안에 있는지 확인
            if (RectTransformUtility.RectangleContainsScreenPoint(button, this.gameObject.GetComponent<RectTransform>().position, Camera.main))
            {
                StopRotate(button); // 버튼 영역 안에 있으면 회전 중지
                canRotate = false;
                isInButtonZone = true;
                this.gameObject.GetComponent<RectTransform>().position = button.position; // 버튼 위치로 이동
                Debug.Log("Entered button zone!");
                break;
            }
        }

        // 버튼 영역을 벗어나면 회전 시작
        if (!isInButtonZone && wasInButtonZone)
        {
            Debug.Log("Exited button zone, starting rotation!");
            canRotate = true; // 다시 회전 허용
        }

        // 회전 가능 시 Rotate 호출
        if (canRotate && !isInButtonZone)
        {
            Rotate();
        }
    }

    private void Rotate()
    {
        //Debug.Log("#### This is Rotate Function ####");

        // 마우스 위치를 월드 좌표로 변환
        Vector3 mPosition = Input.mousePosition;
        mPosition.z = Camera.main.nearClipPlane; // 카메라의 near clip plane 설정
        Vector3 target = Camera.main.ScreenToWorldPoint(mPosition);

        // 원의 중심 위치
        Vector3 oPosition = transform.position;

        // 마우스와 중심 사이의 벡터 계산
        float dx = target.x - oPosition.x;
        float dy = target.y - oPosition.y;

        // 마우스 각도 계산
        float mouseAngle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        // 11시 방향 기준 보정
        float rotateDegree = mouseAngle; //-140

        // RectTransform의 로컬 회전을 설정
        this.gameObject.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, rotateDegree);

        //Debug.Log($"Mouse Angle: {mouseAngle}, Rotate Degree: {rotateDegree}");
    }

    private void StopRotate(RectTransform button)
    {
        Debug.Log("#### StopRotate Function Called ####");
        canRotate = false; // 회전 비활성화
    }

}
