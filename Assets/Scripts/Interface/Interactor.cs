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

    private void Update()
    {
        // ��ȣ�ۿ� ��ġ ���� �浹üũ
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            InteractionPoint.position, InteractionPointRadius, InteractionLayer);

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            for (int i=0; i< colliders.Length; ++i)
            {
                var interactable = colliders[i].GetComponent<IInteractable>();

                // ��ȣ�ۿ� ������ ��ü�� ������ ��ȣ�ۿ� ����
                if (interactable != null)
                    StartInteractable(interactable);
            }
        }
    }

    private void StartInteractable(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccessful);
        IsInteracting = true;
    }

    private void EndInteracting()
    {
        IsInteracting = false;
    }
}
