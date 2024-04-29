
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopKeeper : MonoBehaviour, IInteractable // ��ȣ�ۿ� ���
{
    [SerializeField] private List<ShopItemList> _shopItemsHeld; // ������ ���� ������
    public ShopSystem _shopSystem { get; private set; } // ������ ���� �����ý���(�κ��丮 ����)

    public static UnityAction<ShopSystem, PlayerInventoryHolder> OnShopWindowRequested;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }


    private void Start()
    {
        SetShopItemList();
    }

    public void SetShopItemList()
    {
        // �� ������ �����ý��� ����
        int idx = Random.Range(0, _shopItemsHeld.Count);

        _shopSystem = new ShopSystem(
            _shopItemsHeld[idx].Items.Count,
            _shopItemsHeld[idx].BuyMarkUp,
            _shopItemsHeld[idx].SellMarkUp
            );

        foreach (var item in _shopItemsHeld[idx].Items)
        {   // ������ ���� �����۸�� �޾ƿ� ���� �κ��丮�� �߰�
            _shopSystem.AddToShop(item.ItemData, item.Amount);
        }
    }

    public void Interact(Interactor interator, out bool interactSuccessful)
    {
        var playerInv = interator.GetComponent<PlayerInventoryHolder>();

        if (playerInv != null)
        {
            OnShopWindowRequested?.Invoke(_shopSystem, playerInv);
            interactSuccessful = true;
        }
        else
        {
            interactSuccessful = false;
            Debug.Log("player inventory null");
        }
    }

    public void EndInteraction()
    {
    }

}