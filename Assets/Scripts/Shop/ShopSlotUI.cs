using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlotUI : MonoBehaviour // 상점슬롯 UI
{
    [SerializeField] private Image _itemSprite;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemCount;
    [SerializeField] private ShopSlot _assignedItemSlot;
    public ShopSlot AssignedItemSlot => _assignedItemSlot;

    [SerializeField] private Button BtnAddItemToCart; // + 버튼
    [SerializeField] private Button BtnRemoveItemFromCart; // - 버튼

    private int tempAmount;

    public ShopKeeperDisplay ParentDisplay { get; private set; }
    public float MarkUp { get; private set; }


    private void Awake()
    {
        // 상점슬롯 Ui를 비워두기
        _itemSprite.sprite = null;
        _itemSprite.preserveAspect = true;
        _itemSprite.color = Color.clear;

        _itemName.text = "";
        _itemCount.text = "";

        BtnAddItemToCart?.onClick.AddListener(AddItemToCart);
        BtnRemoveItemFromCart?.onClick.AddListener(RemoveItemFromCart);
        ParentDisplay = transform.parent.GetComponentInParent<ShopKeeperDisplay>();
    }

    public void Init(ShopSlot slot, float markUp)
    {
        // 들어온 slot의 정보로 슬롯UI 업데이트
        _assignedItemSlot = slot;
        MarkUp = markUp;
        tempAmount = slot.StackSize;

        UpdateUISlot();
    }

    private void UpdateUISlot() // 슬롯UI 업데이트
    {
        if (_assignedItemSlot.ItemData != null) // 슬롯UI에 데이터가 있다면
        {
            _itemSprite.sprite = _assignedItemSlot.ItemData.ItemSprite;
            _itemSprite.color = Color.white;
            _itemCount.text = _assignedItemSlot.StackSize.ToString();
            _itemName.color = AssignedItemSlot.ItemData.gradeColor;
            int price = ShopKeeperDisplay.GetModifiedPrice(_assignedItemSlot.ItemData, 1, MarkUp);
            _itemName.text = _assignedItemSlot.ItemData.DisplayName + " (" + price + "G)";
        } else
        {
            _itemSprite.sprite = null;
            _itemSprite.color = Color.clear;
            _itemName.text = "";
            _itemCount.text = "";
        }
    }


    private void AddItemToCart()
    {
        if (tempAmount <= 0) return;

         tempAmount--;
         ParentDisplay.AddItemToCart(this);
         _itemCount.text = tempAmount.ToString();   
    }

    private void RemoveItemFromCart()
    {
        if (tempAmount == _assignedItemSlot.StackSize) return;

        tempAmount++;
        ParentDisplay.RemoveItemFromCart(this);
        _itemCount.text = tempAmount.ToString();
    }
}
