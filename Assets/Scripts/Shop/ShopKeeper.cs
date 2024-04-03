
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))] // 상인마다 고유 ID 존재
public class ShopKeeper : MonoBehaviour, IInteractable // 상호작용 대상
{
    [SerializeField] private List<ShopItemList> _shopItemsHeld; // 상인이 가진 아이템
    [SerializeField] private ShopSystem _shopSystem; // 상인이 가진 상점시스템(인벤토리 포함)

    private string id;
    private ShopSaveData shopSaveData;

    public static UnityAction<ShopSystem, PlayerInventoryHolder> OnShopWindowRequested;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }


    private void Awake()
    {   // 이 상인의 상점시스템 생성
        int idx = Random.Range(0, _shopItemsHeld.Count);

        _shopSystem = new ShopSystem(
            _shopItemsHeld[idx].Items.Count,
            _shopItemsHeld[idx].MaxAllowedGold,
            _shopItemsHeld[idx].BuyMarkUp,
            _shopItemsHeld[idx].SellMarkUp
            );

        foreach (var item in _shopItemsHeld[idx].Items)
        {   // 상인이 가진 아이템목록 받아와 상점 인벤토리에 추가
            _shopSystem.AddToShop(item.ItemData,item.Amount);
        }

        id = GetComponent<UniqueID>().ID;
        shopSaveData = new ShopSaveData(_shopSystem);

    }

    private void Start()
    {
        if (!SaveGameManager.data.shopKeeperDictionary.ContainsKey(id))
            SaveGameManager.data.shopKeeperDictionary.Add(id, shopSaveData);
    }

    private void OnEnable()
    {
        SaveLoad.OnLoadGame += LoadInventory;
    }
    private void OnDisable()
    {
        SaveLoad.OnLoadGame -= LoadInventory;
    }

    private void LoadInventory(SaveData data)
    {
        if (!data.shopKeeperDictionary.TryGetValue(id, out ShopSaveData saveData)) return;

        shopSaveData = saveData;
        _shopSystem = saveData.shopSystem;
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

[System.Serializable]
public struct ShopSaveData
{
    public ShopSystem shopSystem;

    public ShopSaveData(ShopSystem system)
    {
        shopSystem = system;
    }
}