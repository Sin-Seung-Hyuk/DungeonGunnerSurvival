using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDebuff // 디버프를 받을 수 있는 클래스에서 상속 (몹,플레이어)
{
    public void Debuff_Slow();
    public void Debuff_Burn(int ammoDamage);
}
