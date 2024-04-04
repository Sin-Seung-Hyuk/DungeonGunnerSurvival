using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 전역으로 접근가능한 각종 계산 static 클래스
public static class Utilities
{
    public static Camera mainCam;

    // 마우스 커서 위치 구하기  ===========================================================
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

    // 퍼센트 % 계산하기  ===========================================================
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

    // 벡터로부터 각도 구하기  ===========================================================
    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x); // Atan2(y,x) 라디안 구하기
        float degrees = radians * Mathf.Rad2Deg;         // 라디안 디그리로 변환

        return degrees;
    }

    // 현재 마우스 각도에 따라 AimDirection 설정  ===========================================================
    public static AimDirection GetAimDirectionFromAngle(float angle)
    {
        AimDirection aimDirection;

        // UpRight
        if (angle >= 22f && angle <= 67f) aimDirection = AimDirection.UpRight;
        // Up
        else if (angle > 67f && angle <= 112f) aimDirection = AimDirection.Up;
        // UpLeft
        else if (angle > 112f && angle <= 158f) aimDirection = AimDirection.UpLeft;
        // Left
        else if ((angle <= 180f && angle > 158f) || (angle > -180 && angle <= -135f))
            aimDirection = AimDirection.Left;
        // Down
        else if (angle > -135f && angle <= -45f) aimDirection = AimDirection.Down;
        // Right
        else if ((angle > -45f && angle <= 0f) || (angle > 0f && angle < 22f))
            aimDirection = AimDirection.Right;
        else aimDirection = AimDirection.Right;

        return aimDirection;
    }
}
