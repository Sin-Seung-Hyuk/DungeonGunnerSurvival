using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDebuff // ������� ���� �� �ִ� Ŭ�������� ��� (��,�÷��̾�)
{
    public void Debuff_Slow();
    public void Debuff_Burn(int ammoDamage);
}
