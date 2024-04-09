using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �������� ���ٰ����� ���� ��� static Ŭ����
public static class Utilities
{
    public static Camera mainCam;

    // ���콺 Ŀ�� ��ġ ���ϱ�  ===========================================================
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

    // �ۼ�Ʈ % ����ϱ�  ===========================================================
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

    // Ȯ�� ����ϱ�  ===========================================================
    public static bool isSuccess(int percent)
    {
        int chance = Random.Range(1, 101);
        return percent >= chance; // �����ϸ� true
    }

    // ���ͷκ��� ���� ���ϱ�  ===========================================================
    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x); // Atan2(y,x) ���� ���ϱ�
        float degrees = radians * Mathf.Rad2Deg;         // ���� ��׸��� ��ȯ

        return degrees;
    }

    // �����κ��� ���⺤�� ���ϱ�  ===========================================================
    public static Vector3 GetDirectionVectorFromAngle(float angle)
    {
        Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
        return direction;
    }

    // ���� ���콺 ������ ���� AimDirection ����  ===========================================================
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

    // ���� ���� �������� ���ú��� ��ȯ ====================================================================
    public static float LinearToDecibels(int linear)
    {
        float linearScaleRange = 20f;

        return Mathf.Log10((float)linear / linearScaleRange) * 20f;
    }
}
