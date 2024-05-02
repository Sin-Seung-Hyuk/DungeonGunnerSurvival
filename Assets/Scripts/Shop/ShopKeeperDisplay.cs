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

    [Header("Shopping Cart")] // ����īƮ ����
    [SerializeField] private TextMeshProUGUI basketTotalText;
    [SerializeField] private TextMeshProUGUI playerGoldText;
    [SerializeField] private TextMeshProUGUI buyButtonText;
    [SerializeField] private Button BtnBuy;
    [SerializeField] private TextMeshProUGUI RerollGoldText;

    [Header("Item Preview")] // ������ ������ ����
    [SerializeField] private Image itemPreviewSprite;
    [SerializeField] private TextMeshProUGUI itemPreviewName;
    [SerializeField] private TextMeshProUGUI itemPreviewDescription;

    [SerializeField] private GameObject itemListContentPanel;
    [SerializeField] private GameObject shoppingCartContentPanel;

    private int basketTotal;
    private bool isSelling;
    private int rerollGold = Settings.rerollGold;

    private ShopSystem _shopSystem; // �����ý���
    private PlayerInventoryHolder _playerInventory; // �÷��̾� �κ��丮
    // ����īƮ (�����۵�����, ����)
    private Dictionary<InventoryItemData, int> _shoppingCart = new Dictionary<InventoryItemData, int>();
    // ����īƮUI (�����۵�����, ����īƮUI)
    private Dictionary<InventoryItemData, ShoppingCartItemUI> _shoppingCartUI = new Dictionary<InventoryItemData, ShoppingCartItemUI>();


    // UIController ���� �����Ͽ� �����ý��۰� �÷��̾� �κ��丮�� �Ѱ���
    public void DisplayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInventory)
    {               
        _shopSystem = shopSystem;
        _playerInventory = playerInventory;

        if (playerInventory == null) Debug.Log("�÷��̾��κ� ����");

        RefreshDisplay();
    }

    private void RefreshDisplay() // ����UI ���ΰ�ħ
    {
        if (BtnBuy != null)
        {
            buyButtonText.text = isSelling ? "Sell Items" : "Buy Items";
            BtnBuy.onClick.RemoveAllListeners();
            if (isSelling) BtnBuy.onClick.AddListener(SellItems); // �Ǹ�or���� �Լ��� ���� ��ư
            else BtnBuy.onClick.AddListener(BuyItems);
        }

        ClearSlots();
        ClearItemPreview();

        basketTotalText.enabled = false;
        basketTotal = 0;
        playerGoldText.text = _playerInventory.PrimaryInventorySystem.Gold.ToString() + " G";

        if (isSelling) DisplayPlayerInventory(); // �ǸŸ�� : �÷��̾� �κ��丮
        else DisplayShopInventory();            // ���Ը�� : ���� �κ��丮
    }


    // ============= Buy / Sell Items ======================================================================================================================================================================
    private void BuyItems() // ������ ����
    {
        // �� ���� or ���� �ڸ�����
        if (_playerInventory.PrimaryInventorySystem.Gold < basketTotal) return;
        if (!_playerInventory.PrimaryInventorySystem.CheckInventoryRemaining(_shoppingCart)) return;

        foreach (var pair in _shoppingCart)
        {
            _shopSystem.PurchaseItem(pair.Key, pair.Value); // ������ ������ �������� ��������

            for (int i=0;i <pair.Value; i++)
            {                                           // �÷��̾� �κ��丮�� ������ ������ �߰�
                _playerInventory.PrimaryInventorySystem.AddToInventory(pair.Key, 1);
            }
        }

        _playerInventory.PrimaryInventorySystem.SpendGold(basketTotal); // �÷��̾�� �� �Һ�
        StatisticsManager.Instance.TotalSpentGold += basketTotal; // �� �Ҹ��� ��� ����

        RefreshDisplay();
    }

    private void SellItems()    // ������ �Ǹ�
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

            // ���� ���� ����,�ʱ�ȭ
            ShopSlotUI shopSlot = Instantiate(_shopSlotPrefab, itemListContentPanel.transform);
            shopSlot.Init(item, _shopSystem.BuyMarkUp);
        }
    }

    private void DisplayPlayerInventory() // �ǸŸ�� �÷��̾� �κ��丮 
    {                                                   // �÷��̾� �κ��� ����� ��������
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
        // 1. ���Ÿ���϶��� ����, �÷��̾�� ���ΰ�ħ ����� �����ؾ���
        // 2. ������ ��������Ʈ ��� ����
        // 3. ���ΰ�ħ�� �������� ���÷��� ������Ʈ
        if (!isSelling && _playerInventory.PrimaryInventorySystem.Gold >= rerollGold)
        {
            _playerInventory.PrimaryInventorySystem.SpendGold(rerollGold);
            rerollGold = Utilities.IncreaseByPercent(rerollGold, 10); // ���Ѻ�� 10%�� ����
            RerollGoldText.text = rerollGold.ToString();

            ClearShopItemList();
            shopKeeper.SetShopItemList();
            _shopSystem = shopKeeper._shopSystem;

            RefreshDisplay();
        }
    }


    // ======== Remove / Add Item to Cart ====================================================================================================================================================================================================================================
    public void RemoveItemFromCart(ShopSlotUI shopSlotUI) // �ش� ����UI īƮ���� ����
    {
        InventoryItemData data = shopSlotUI.AssignedItemSlot.ItemData;
        int price = GetModifiedPrice(data, 1, shopSlotUI.MarkUp);

        if (_shoppingCart.ContainsKey(data)) // ����īƮ�� ��� ������ �˻�
        {
            _shoppingCart[data]--; // īƮ�� ��� �ش������ ���� ����
            string newString = $"{data.DisplayName} ({price}G) x{_shoppingCart[data]}";
            _shoppingCartUI[data].SetItemText(newString);

            if (_shoppingCart[data] <= 0) // īƮ���� ��� ����
            {
                _shoppingCart.Remove(data);
                GameObject tempObj = _shoppingCartUI[data].gameObject;
                _shoppingCartUI.Remove(data);
                Destroy(tempObj);
            }
        }

        basketTotal -= price;
        basketTotalText.text = "Total : " + basketTotal;

        if (basketTotal <=0 && basketTotalText.IsActive()) // īƮ���� ��� ����
        {
            basketTotalText.enabled = false;
            BtnBuy.gameObject.SetActive(false);
            ClearItemPreview();
            
            return;
        }

        CheckCartAvailableGold();
    }

    public void AddItemToCart(ShopSlotUI shopSlotUI) // īƮ�� ������ ���
    {
        InventoryItemData data = shopSlotUI.AssignedItemSlot.ItemData;

        UpdateItemPreview(shopSlotUI); // ������ ������ȭ�� ������Ʈ

        int price = GetModifiedPrice(data, 1, shopSlotUI.MarkUp);


        if (_shoppingCart.ContainsKey(data)) // �̹� ���� ������
        {
            _shoppingCart[data]++; // �̹� ����īƮ�� �÷����� ���̸� ��������
            string newString = $"{data.DisplayName} ({price}G) x{_shoppingCart[data]}";
            _shoppingCartUI[data].SetItemText(newString);
        }
        else
        {
            _shoppingCart.Add(data, 1); // īƮ�� ���� ���� ������ ��ųʸ��� �߰�
            ShoppingCartItemUI shoppingCartTextObj = Instantiate(_shoppingCartItemPrefab, shoppingCartContentPanel.transform);
            string newString = $"{data.DisplayName} ({price}G) x1";
            shoppingCartTextObj.SetItemText(newString);
            _shoppingCartUI.Add(data, shoppingCartTextObj);
        }

        basketTotal += price;
        basketTotalText.text = "Total : " + basketTotal;

        if (basketTotal > 0 && !basketTotalText.IsActive()) // īƮ�� ������ ���
        {
            basketTotalText.enabled = true;
            BtnBuy.gameObject.SetActive(true);
        }

        CheckCartAvailableGold(); // ���,������� ������� �˻�
    }

    private void CheckCartAvailableGold()
    {
        if (isSelling || _playerInventory.PrimaryInventorySystem.CheckInventoryRemaining(_shoppingCart)) return;

        basketTotalText.text = "Not Enough to room in inventory"; // ���濡 ��������
        basketTotalText.color = Settings.red;
    }

    public static int GetModifiedPrice(InventoryItemData data, int amount, float markUp)
    {
        int baseValue = data.GoldValue * amount;

        return Mathf.RoundToInt(baseValue + baseValue * markUp); // ���ݰ��
    }

    private void UpdateItemPreview(ShopSlotUI shopSlotUI) // ������ ������ ȭ�� ������Ʈ
    {
        InventoryItemData data = shopSlotUI.AssignedItemSlot.ItemData;

        itemPreviewSprite.sprite = data.ItemSprite;
        itemPreviewSprite.color = Color.white;
        itemPreviewName.text = data.DisplayName;
        itemPreviewName.color = data.gradeColor;
        itemPreviewDescription.text = data.Description;
    }


    // ============= Clear UI,Slot ======================================================================================================================================================================
    private void ClearSlots() // ����īƮ,����īƮUI ��� ���ֱ�
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
    private void ClearItemPreview() // ������ȭ�� ���ֱ�
    {
        itemPreviewSprite.sprite = null;
        itemPreviewSprite.color = Color.clear;
        itemPreviewName.text = "";
        itemPreviewName.color = Color.clear;
        itemPreviewDescription.text = "";
    }
    private void ClearShopItemList() // ���� ������ ��� ���ֱ� (���� ���ΰ�ħ)
    {
        ShopSlotUI[] slotUI = itemListContentPanel.GetComponentsInChildren<ShopSlotUI>();

        foreach (var item in slotUI)
        {
            item.ClearSlotUI();
        }
    }


    // ============= Buy / Sell Mode ======================================================================================================================================================================
    public void OnBuyTabPressed() // ���� ���
    {
        isSelling = false;
        RefreshDisplay();
    }
    public void OnSellTabPressed() // �Ǹ� ���
    {
        isSelling = true;
        RefreshDisplay();
    }
}
