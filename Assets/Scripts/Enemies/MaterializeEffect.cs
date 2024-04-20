using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MaterializeEffect : MonoBehaviour
{
    private float dissolveAmount; // 시작 값
    private float targetAmount;   // 목표 값

    public IEnumerator MaterializeRoutine(Shader materializeShader, Color materializeColor, 
        float materializeTime, SpriteRenderer sprite, Material normalMaterial, bool isFadeIn)
    {
        Material materializeMaterial = new Material(materializeShader); // 매개변수 쉐이더로 머테리얼 생성

        materializeMaterial.SetColor("_EmissionColor", materializeColor); // 머테리얼 색 설정

        sprite.material = materializeMaterial; // 매개변수 객체의 스프라이트에 머테리얼 입히기

        if (isFadeIn)
        {
            dissolveAmount = 0f;
            targetAmount = 1f;
        } else
        {
            dissolveAmount = 1f;
            targetAmount = 0f;
        }

        // DOTween.To(..) 람다식을 통해 변수를 n초에 걸쳐 x값으로 보간
        var tween = DOTween.To(() => dissolveAmount, x => dissolveAmount = x, targetAmount, materializeTime)
            .OnUpdate(() => { materializeMaterial.SetFloat("_DissolveAmount", dissolveAmount); }); 
        // OnUpdate() : 트윈이 실행되는 동안 람다식으로 머테리얼 속성 변경

        yield return tween.WaitForCompletion(); // 코루틴처럼 트윈이 끝날때까지 기다리기


        sprite.material = normalMaterial;
    }
}

//while (dissolveAmount > 0f)
//{
//    dissolveAmount -= Time.deltaTime / materializeTime;

//    materializeMaterial.SetFloat("_DissolveAmount", dissolveAmount);

//    yield return null;
//}