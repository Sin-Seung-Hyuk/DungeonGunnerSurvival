using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlotUI : MonoBehaviour // �������� UI
{
    [SerializeField] private Image _itemSprite;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemCount;
    [SerializeField] private ShopSlot _assignedItemSlot;
    public ShopSlot AssignedItemSlot => _assignedItemSlot;

    [SerializeField] private Button BtnAddItemToCart; // + ��ư
    [SerializeField] private Button BtnRemoveItemFromCart; // - ��ư

    private int tempAmount;

    public ShopKeeperDisplay ParentDisplay { get; private set; }
    public float MarkUp { get; private set; }


    private void Awake()
    {
        // �������� Ui�� ����α�
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
        // ���� slot�� ������ ����UI ������Ʈ
        _assignedItemSlot = slot;
        MarkUp = markUp;
        tempAmount = slot.StackSize;

        UpdateUISlot();
    }

    private void UpdateUISlot() // ����UI ������Ʈ
    {
        if (_assignedItemSlot.ItemData != null) // ����UI�� �����Ͱ� �ִٸ�
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
