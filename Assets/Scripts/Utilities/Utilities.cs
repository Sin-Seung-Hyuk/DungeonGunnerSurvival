using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 전역으로 접근가능한 각종 수식 계산 static 클래스
public static class Utilities
{
    public static Camera mainCam;

    // 마우스 커서 위치 구하기
    public static Vector3 GetMouseCursorPos()
    {
        if (mainCam == null) mainCam = Camera.main;

        Vector3 pos = Input.mousePosition;

        pos.x = Mathf.Clamp(pos.x, 0f, Screen.width);
        pos.y = Mathf.Clamp(pos.y, 0f, Screen.height);

        Vector3 worldPos = mainCam.ScreenToWorldPoint(pos);
        worldPos.z = 0f;

        return worldPos;
    }

    // 퍼센트 % 계산하기
    public static int IncreaseByPercent(int value, float percent)
    {
        float increase = value * (percent / 100);
        return Mathf.RoundToInt(value + increase);
    }
    public static float IncreaseByPercent(float value, float percent)
    {
        float increase = value * (percent / 100);
        return value + increase;
    }
}
