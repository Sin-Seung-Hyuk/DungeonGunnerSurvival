using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �������� ���ٰ����� ���� ���� ��� static Ŭ����
public static class Utilities
{
    public static Camera mainCam;

    // ���콺 Ŀ�� ��ġ ���ϱ�
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

    // �ۼ�Ʈ % ����ϱ�
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
