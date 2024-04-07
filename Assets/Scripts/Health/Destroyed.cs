using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(DestroyedEvent))]
[DisallowMultipleComponent]
public class Destroyed : MonoBehaviour
{
    private DestroyedEvent destroyedEvent;

    private void Awake()
    {
        destroyedEvent = GetComponent<DestroyedEvent>();
    }
    private void OnEnable()
    {
        destroyedEvent.OnDestroyed += DestroyedEvent_OnDestroyed;
    }
    private void OnDisable()
    {
        destroyedEvent.OnDestroyed -= DestroyedEvent_OnDestroyed;
    }

    private void DestroyedEvent_OnDestroyed(DestroyedEvent obj, DestroyedEventArgs args)
    {
        if (args.isPlayerDie)
        {
            gameObject.SetActive(false);
        }
        else Destroy(gameObject); // �� ��ũ��Ʈ�� ���� �ڱ��ڽ��� �ı�
    }
}
