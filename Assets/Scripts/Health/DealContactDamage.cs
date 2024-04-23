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
        if (isColliding) return; // �̹� �ε���

        ContactDamage(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isColliding) return;

        ContactDamage(collision);
    }

    private void ContactDamage(Collider2D collision)
    {
        // �ε��� ������Ʈ�� ���̾��ũ ��Ʈ�� �˱� ���� �� ������Ʈ�� ���̾ ������ ��ȯ
        int collisionObjLayerMask = (1 << collision.gameObject.layer);

        if ((layerMask.value & collisionObjLayerMask) == 0) // ***************** �߿� ********
            return; // ���� ������ ���ϴ� ���̾��ũ�� ������Ʈ�� ���̾� & ����
                    // 0 �� ���� ���ϴ� ���̾ �ƴ϶�� ��. (��ġ�� ��Ʈ�� �����Ƿ�)

        ReciveContactDamage reciveContactDamage = collision.gameObject.GetComponent<ReciveContactDamage>();

        if (reciveContactDamage != null)
        {
            isColliding = true;
            // contactsDamageDelay �� ���Ŀ� �浹������ �ٽ� Ȱ��ȭ
            Invoke("ResetContactCollision", Settings.contactsDamageDelay);

            reciveContactDamage.TakeContactDamage(contactDamageAmount);
        }
    }

    private void ResetContactCollision()
    {
        isColliding = false;
    }
}
