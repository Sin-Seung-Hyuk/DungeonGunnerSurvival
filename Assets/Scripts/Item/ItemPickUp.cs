using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour  // 아이템에 연결할 클래스
{
    [SerializeField] private InventoryItemData itemData; // 아이템이 가지고 있는 아이템데이터
    [SerializeField] private SoundEffectSO itemPickUpSound;

    private SpriteRenderer spriteRenderer;
    private ParticleSystem particle;
    private int gainExp;

    private string id;


    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;
    }
    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedArgs obj)
    {
        gameObject.SetActive(false); // 방 변경시 아이템 소멸
    }

    public void InitializeItem(InventoryItemData data)
    {
        if (data == null) return; // 아무런 데이터가 들어오지 않았을 경우 (EnemySpawn 클래스 참고)

        itemData = data;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemData.ItemSprite;

        particle = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particle.main; // 파티클 시스템의 MainModule로 색상변경 가능
        main.startColor = itemData.gradeColor;

        gainExp = (int)itemData.itemGrade; // 해당 등급에 맞는 경험치 획득 (등급마다 경험치 정해져있음)

        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player inventoryHolder = collision.transform.GetComponentInParent<Player>();

        if (!inventoryHolder) return; // 인벤토리 보유여부

        if (inventoryHolder.playerInventory.AddToInventory(itemData,1))
        {
            gainExp += inventoryHolder.stat.expGain;
            inventoryHolder.playerExp.TakeExp(gainExp);

            SoundEffectManager.Instance.PlaySoundEffect(itemPickUpSound);
            gameObject.SetActive(false);
        }
    }
}
