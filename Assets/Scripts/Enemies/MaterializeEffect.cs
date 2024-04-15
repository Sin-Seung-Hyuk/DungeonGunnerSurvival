using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MaterializeEffect : MonoBehaviour
{
    public IEnumerator MaterializeRoutine(Shader materializeShader, Color materializeColor, 
        float materializeTime, SpriteRenderer sprite, Material normalMaterial)
    {
        Material materializeMaterial = new Material(materializeShader); // �Ű����� ���̴��� ���׸��� ����

        materializeMaterial.SetColor("_EmissionColor", materializeColor); // ���׸��� �� ����

        sprite.material = materializeMaterial; // �Ű����� ��ü�� ��������Ʈ�� ���׸��� ������

        float dissolveAmount = 1f;

        // DOTween.To(..) ���ٽ��� ���� ������ n�ʿ� ���� x������ ����
        var tween = DOTween.To(() => dissolveAmount, x => dissolveAmount = x, 0f, materializeTime)
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