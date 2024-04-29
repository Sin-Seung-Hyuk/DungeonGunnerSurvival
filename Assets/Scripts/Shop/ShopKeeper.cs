
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopKeeper : MonoBehaviour, IInteractable // 상호작용 대상
{
    [SerializeField] private List<ShopItemList> _shopItemsHeld; // 상인이 가진 아이템
    public ShopSystem _shopSystem { get; private set; } // 상인이 가진 상점시스템(인벤토리 포함)

    public static UnityAction<ShopSystem, PlayerInventoryHolder> OnShopWindowRequested;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }


    private void Start()
    {
        SetShopItemList();
    }

    public void SetShopItemList()
    {
        // 이 상인의 상점시스템 생성
        int idx = Random.Range(0, _shopItemsHeld.Count);

        _shopSystem = new ShopSystem(
            _shopItemsHeld[idx].Items.Count,
            _shopItemsHeld[idx].BuyMarkUp,
            _shopItemsHeld[idx].SellMarkUp
            );

        foreach (var item in _shopItemsHeld[idx].Items)
        {   // 상인이 가진 아이템목록 받아와 상점 인벤토리에 추가
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