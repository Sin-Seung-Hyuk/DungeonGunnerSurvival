using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    // 상호작용을 수행할 주체. 플레이어 캐릭터

    public Transform InteractionPoint; // 상호작용 위치
    public LayerMask InteractionLayer; // 상호작용 대상 레이어
    public float InteractionPointRadius = 1f;
    public bool IsInteracting { get; private set; }

    private Vector3 interactPosition;
    private IInteractable interactable;

    private void Update()
    {
        // 상호작용 위치 주위 충돌체크
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            InteractionPoint.position, InteractionPointRadius, InteractionLayer);

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            for (int i=0; i< colliders.Length; ++i)
            {
                interactable = colliders[i].GetComponent<IInteractable>();

                // 상호작용 가능한 객체에 접근해 상호작용 시작
                if (interactable != null)
                {
                    StartInteractable(interactable);
                    interactPosition = InteractionPoint.position;
                }
            }
        }

        if (Vector2.Distance(this.transform.position, interactPosition) > 5.0f && IsInteracting)
        {
            EndInteracting(interactable);
        }
    }

    private void StartInteractable(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccessful);
    }

    private void EndInteracting(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccessful);
        interactPosition = Vector3.zero;
    }

    public void SetInteracting()
    {
        if (IsInteracting)
            IsInteracting = false;
        else IsInteracting = true;
    }
}
