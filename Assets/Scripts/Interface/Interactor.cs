using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    // ��ȣ�ۿ��� ������ ��ü. �÷��̾� ĳ����

    public Transform InteractionPoint; // ��ȣ�ۿ� ��ġ
    public LayerMask InteractionLayer; // ��ȣ�ۿ� ��� ���̾�
    public float InteractionPointRadius = 1f;
    public bool IsInteracting { get; private set; }

    private Vector3 interactPosition;
    private IInteractable interactable;

    private void Update()
    {
        // ��ȣ�ۿ� ��ġ ���� �浹üũ
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            InteractionPoint.position, InteractionPointRadius, InteractionLayer);

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            for (int i=0; i< colliders.Length; ++i)
            {
                interactable = colliders[i].GetComponent<IInteractable>();

                // ��ȣ�ۿ� ������ ��ü�� ������ ��ȣ�ۿ� ����
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
