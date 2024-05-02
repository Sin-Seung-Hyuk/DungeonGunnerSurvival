using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class ShopKeeperDisplay : MonoBehaviour
{
    [SerializeField] private ShopKeeper shopKeeper;
    [SerializeField] private ShopSlotUI _shopSlotPrefab;
    [SerializeField] private ShoppingCartItemUI _shoppingCartItemPrefab;
    [SerializeField] private Button btnSellTab;
    [SerializeField] private Button btnBuyTab;

    [Header("Shopping Cart")] // 쇼핑카트 구역
    [SerializeField] private TextMeshProUGUI basketTotalText;
    [SerializeField] private TextMeshProUGUI playerGoldText;
    [SerializeField] private TextMeshProUGUI buyButtonText;
    [SerializeField] private Button BtnBuy;
    [SerializeField] private TextMeshProUGUI RerollGoldText;

    [Header("Item Preview")] // 아이템 프리뷰 구역
    [SerializeField] private Image itemPreviewSprite;
    [SerializeField] private TextMeshProUGUI itemPreviewName;
    [SerializeField] private TextMeshProUGUI itemPreviewDescription;

    [SerializeField] private GameObject itemListContentPanel;
    [SerializeField] private GameObject shoppingCartContentPanel;

    private int basketTotal;
    private bool isSelling;
    private int rerollGold = Settings.rerollGold;

    private ShopSystem _shopSystem; // 상점시스템
    private PlayerInventoryHolder _playerInventory; // 플레이어 인벤토리
    // 쇼핑카트 (아이템데이터, 개수)
    private Dictionary<InventoryItemData, int> _shoppingCart = new Dictionary<InventoryItemData, int>();
    // 쇼핑카트UI (아이템데이터, 쇼핑카트UI)
    private Dictionary<InventoryItemData, ShoppingCartItemUI> _shoppingCartUI = new Dictionary<InventoryItemData, ShoppingCartItemUI>();


    // UIController 에서 실행하여 상점시스템과 플레이어 인벤토리를 넘겨줌
    public void DisplayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInventory)
    {               
        _shopSystem = shopSystem;
        _playerInventory = playerInventory;

        if (playerInventory == null) Debug.Log("플레이어인벤 없음");

        RefreshDisplay();
    }

    private void RefreshDisplay() // 상점UI 새로고침
    {
        if (BtnBuy != null)
        {
            buyButtonText.text = isSelling ? "Sell Items" : "Buy Items";
            BtnBuy.onClick.RemoveAllListeners();
            if (isSelling) BtnBuy.onClick.AddListener(SellItems); // 판매or구입 함수를 갖는 버튼
            else BtnBuy.onClick.AddListener(BuyItems);
        }

        ClearSlots();
        ClearItemPreview();

        basketTotalText.enabled = false;
        basketTotal = 0;
        playerGoldText.text = _playerInventory.PrimaryInventorySystem.Gold.ToString() + " G";

        if (isSelling) DisplayPlayerInventory(); // 판매모드 : 플레이어 인벤토리
        else DisplayShopInventory();            // 구입모드 : 상점 인벤토리
    }


    // ============= Buy / Sell Items ======================================================================================================================================================================
    private void BuyItems() // 아이템 구입
    {
        // 돈 부족 or 가방 자리부족
        if (_playerInventory.PrimaryInventorySystem.Gold < basketTotal) return;
        if (!_playerInventory.PrimaryInventorySystem.CheckInventoryRemaining(_shoppingCart)) return;

        foreach (var pair in _shoppingCart)
        {
            _shopSystem.PurchaseItem(pair.Key, pair.Value); // 구입한 아이템 상점에서 스택제거

            for (int i=0;i <pair.Value; i++)
            {                                           // 플레이어 인벤토리에 구입한 아이템 추가
                _playerInventory.PrimaryInventorySystem.AddToInventory(pair.Key, 1);
            }
        }

        _playerInventory.PrimaryInventorySystem.SpendGold(basketTotal); // 플레이어는 돈 소비
        StatisticsManager.Instance.TotalSpentGold += basketTotal; // 총 소모한 골드 증가

        RefreshDisplay();
    }

    private void SellItems()    // 아이템 판매
    {
        foreach (var pair in _shoppingCart)
        {                                // data      amount
            int price = GetModifiedPrice(pair.Key, pair.Value, _shopSystem.SellMarkUp);

            _shopSystem.SellItem(pair.Key, pair.Value, price);

            _playerInventory.PrimaryInventorySystem.GainGold(price);
            _playerInventory.PrimaryInventorySystem.RemoveItemsFromInventory(pair.Key, pair.Value);
        }

        RefreshDisplay();
    }

    // ============= Display Inventory ====================================================================================================================================================================================================================================
    private void DisplayShopInventory()
    {
        foreach (ShopSlot item in _shopSystem.ShopInventory)
        {
            if (item.ItemData == null) continue;

            // 상점 슬롯 생성,초기화
            ShopSlotUI shopSlot = Instantiate(_shopSlotPrefab, itemListContentPanel.transform);
            shopSlot.Init(item, _shopSystem.BuyMarkUp);
        }
    }

    private void DisplayPlayerInventory() // 판매모드 플레이어 인벤토리 
    {                                                   // 플레이어 인벤의 모든템 가져오기
        foreach (var item in _playerInventory.PrimaryInventorySystem.GetAllItem()) 
        {
            ShopSlot tempSlot = new ShopSlot();
            tempSlot.AssignItem(item.Key, item.Value);

            ShopSlotUI shopSlot = Instantiate(_shopSlotPrefab, itemListContentPanel.transform);
            shopSlot.Init(tempSlot, _shopSystem.SellMarkUp);
        }
    }

    // ============= Reroll Shop ====================================================================================================================================================================================================================================
    public void RerollShop()
    {
        // 1. 구매모드일때만 동작, 플레이어는 새로고침 비용을 지불해야함
        // 2. 기존의 상점리스트 모두 삭제
        // 3. 새로고침한 상점으로 디스플레이 업데이트
        if (!isSelling && _playerInventory.PrimaryInventorySystem.Gold >= rerollGold)
        {
            _playerInventory.PrimaryInventorySystem.SpendGold(rerollGold);
            rerollGold = Utilities.IncreaseByPercent(rerollGold, 10); // 리롤비용 10%씩 증가
            RerollGoldText.text = rerollGold.ToString();

            ClearShopItemList();
            shopKeeper.SetShopItemList();
            _shopSystem = shopKeeper._shopSystem;

            RefreshDisplay();
        }
    }


    // ======== Remove / Add Item to Cart ====================================================================================================================================================================================================================================
    public void RemoveItemFromCart(ShopSlotUI shopSlotUI) // 해당 슬롯UI 카트에서 제거
    {
        InventoryItemData data = shopSlotUI.AssignedItemSlot.ItemData;
        int price = GetModifiedPrice(data, 1, shopSlotUI.MarkUp);

        if (_shoppingCart.ContainsKey(data)) // 쇼핑카트에 담긴 아이템 검사
        {
            _shoppingCart[data]--; // 카트에 담긴 해당아이템 개수 감소
            string newString = $"{data.DisplayName} ({price}G) x{_shoppingCart[data]}";
            _shoppingCartUI[data].SetItemText(newString);

            if (_shoppingCart[data] <= 0) // 카트에서 모두 제거
            {
                _shoppingCart.Remove(data);
                GameObject tempObj = _shoppingCartUI[data].gameObject;
                _shoppingCartUI.Remove(data);
                Destroy(tempObj);
            }
        }

        basketTotal -= price;
        basketTotalText.text = "Total : " + basketTotal;

        if (basketTotal <=0 && basketTotalText.IsActive()) // 카트에서 모두 제거
        {
            basketTotalText.enabled = false;
            BtnBuy.gameObject.SetActive(false);
            ClearItemPreview();
            
            return;
        }

        CheckCartAvailableGold();
    }

    public void AddItemToCart(ShopSlotUI shopSlotUI) // 카트에 아이템 담기
    {
        InventoryItemData data = shopSlotUI.AssignedItemSlot.ItemData;

        UpdateItemPreview(shopSlotUI); // 아이템 프리뷰화면 업데이트

        int price = GetModifiedPrice(data, 1, shopSlotUI.MarkUp);


        if (_shoppingCart.ContainsKey(data)) // 이미 담은 아이템
        {
            _shoppingCart[data]++; // 이미 쇼핑카트에 올려놓은 템이면 개수증가
            string newString = $"{data.DisplayName} ({price}G) x{_shoppingCart[data]}";
            _shoppingCartUI[data].SetItemText(newString);
        }
        else
        {
            _shoppingCart.Add(data, 1); // 카트에 새로 담은 아이템 딕셔너리에 추가
            ShoppingCartItemUI shoppingCartTextObj = Instantiate(_shoppingCartItemPrefab, shoppingCartContentPanel.transform);
            string newString = $"{data.DisplayName} ({price}G) x1";
            shoppingCartTextObj.SetItemText(newString);
            _shoppingCartUI.Add(data, shoppingCartTextObj);
        }

        basketTotal += price;
        basketTotalText.text = "Total : " + basketTotal;

        if (basketTotal > 0 && !basketTotalText.IsActive()) // 카트에 아이템 담김
        {
            basketTotalText.enabled = true;
            BtnBuy.gameObject.SetActive(true);
        }

        CheckCartAvailableGold(); // 골드,가방공간 충분한지 검사
    }

    private void CheckCartAvailableGold()
    {
        if (isSelling || _playerInventory.PrimaryInventorySystem.CheckInventoryRemaining(_shoppingCart)) return;

        basketTotalText.text = "Not Enough to room in inventory"; // 가방에 공간부족
        basketTotalText.color = Settings.red;
    }

    public static int GetModifiedPrice(InventoryItemData data, int amount, float markUp)
    {
        int baseValue = data.GoldValue * amount;

        return Mathf.RoundToInt(baseValue + baseValue * markUp); // 가격계산
    }

    private void UpdateItemPreview(ShopSlotUI shopSlotUI) // 프리뷰 아이템 화면 업데이트
    {
        InventoryItemData data = shopSlotUI.AssignedItemSlot.ItemData;

        itemPreviewSprite.sprite = data.ItemSprite;
        itemPreviewSprite.color = Color.white;
        itemPreviewName.text = data.DisplayName;
        itemPreviewName.color = data.gradeColor;
        itemPreviewDescription.text = data.Description;
    }


    // ============= Clear UI,Slot ======================================================================================================================================================================
    private void ClearSlots() // 쇼핑카트,쇼핑카트UI 모두 없애기
    {
        _shoppingCart = new Dictionary<InventoryItemData, int>();
        _shoppingCartUI = new Dictionary<InventoryItemData, ShoppingCartItemUI>();

        foreach (var item in itemListContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
        foreach (var item in shoppingCartContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
    }
    private void ClearItemPreview() // 프리뷰화면 없애기
    {
        itemPreviewSprite.sprite = null;
        itemPreviewSprite.color = Color.clear;
        itemPreviewName.text = "";
        itemPreviewName.color = Color.clear;
        itemPreviewDescription.text = "";
    }
    private void ClearShopItemList() // 상점 아이템 모두 없애기 (상점 새로고침)
    {
        ShopSlotUI[] slotUI = itemListContentPanel.GetComponentsInChildren<ShopSlotUI>();

        foreach (var item in slotUI)
        {
            item.ClearSlotUI();
        }
    }


    // ============= Buy / Sell Mode ======================================================================================================================================================================
    public void OnBuyTabPressed() // 구매 모드
    {
        isSelling = false;
        RefreshDisplay();
    }
    public void OnSellTabPressed() // 판매 모드
    {
        isSelling = true;
        RefreshDisplay();
    }
}
