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
        if (args.isPooling) // 오브젝트 풀에 반환해야 하는 객체일 경우
        {
            gameObject.SetActive(false);
        }
        else Destroy(gameObject); // 이 스크립트를 가진 자기자신을 파괴
    }
}
