using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealContactDamage : MonoBehaviour
{
    [Header("Deal Damage")]
    private int contactDamageAmount;
    [SerializeField] private LayerMask layerMask;

    private bool isColliding = false;


    public void InitializedContactDamage(int damageAmount)
    {
        contactDamageAmount = damageAmount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isColliding) return; // 이미 부딪힘

        ContactDamage(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isColliding) return;

        ContactDamage(collision);
    }

    private void ContactDamage(Collider2D collision)
    {
        // 부딪힌 오브젝트의 레이어마스크 비트를 알기 위해 그 오브젝트의 레이어를 정수로 변환
        int collisionObjLayerMask = (1 << collision.gameObject.layer);

        if ((layerMask.value & collisionObjLayerMask) == 0) // ***************** 중요 ********
            return; // 내가 설정한 원하는 레이어마스크와 오브젝트의 레이어 & 연산
                    // 0 은 내가 원하는 레이어가 아니라는 뜻. (겹치는 비트가 없으므로)

        ReciveContactDamage reciveContactDamage = collision.gameObject.GetComponent<ReciveContactDamage>();

        if (reciveContactDamage != null)
        {
            isColliding = true;
            // contactsDamageDelay 초 이후에 충돌데미지 다시 활성화
            Invoke("ResetContactCollision", Settings.contactsDamageDelay);

            reciveContactDamage.TakeContactDamage(contactDamageAmount);
        }
    }

    private void ResetContactCollision()
    {
        isColliding = false;
    }
}
