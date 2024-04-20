using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MaterializeEffect : MonoBehaviour
{
    private float dissolveAmount; // ���� ��
    private float targetAmount;   // ��ǥ ��

    public IEnumerator MaterializeRoutine(Shader materializeShader, Color materializeColor, 
        float materializeTime, SpriteRenderer sprite, Material normalMaterial, bool isFadeIn)
    {
        Material materializeMaterial = new Material(materializeShader); // �Ű����� ���̴��� ���׸��� ����

        materializeMaterial.SetColor("_EmissionColor", materializeColor); // ���׸��� �� ����

        sprite.material = materializeMaterial; // �Ű����� ��ü�� ��������Ʈ�� ���׸��� ������

        if (isFadeIn)
        {
            dissolveAmount = 0f;
            targetAmount = 1f;
        } else
        {
            dissolveAmount = 1f;
            targetAmount = 0f;
        }

        // DOTween.To(..) ���ٽ��� ���� ������ n�ʿ� ���� x������ ����
        var tween = DOTween.To(() => dissolveAmount, x => dissolveAmount = x, targetAmount, materializeTime)
            .OnUpdate(() => { materializeMaterial.SetFloat("_DissolveAmount", dissolveAmount); }); 
        // OnUpdate() : Ʈ���� ����Ǵ� ���� ���ٽ����� ���׸��� �Ӽ� ����

        yield return tween.WaitForCompletion(); // �ڷ�ƾó�� Ʈ���� ���������� ��ٸ���


        sprite.material = normalMaterial;
    }
}

//while (dissolveAmount > 0f)
//{
//    dissolveAmount -= Time.deltaTime / materializeTime;

//    materializeMaterial.SetFloat("_DissolveAmount", dissolveAmount);

//    yield return null;
//}