
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System;
                                                // ����UI�� ���콺 �ø���,����
public class InventorySlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemSprite; // ����UI �̹���
    [SerializeField] private TextMeshProUGUI itemCount; // ����UI ����ǥ��
    [SerializeField] public TextMeshProUGUI itemNumPad; // ����UI �ѹ��е�

    [SerializeField] private Image MouseOverUI; // ����UI ���콺���� UI
    [SerializeField] private TextMeshProUGUI MouseOverDisplayName;
    [SerializeField] private TextMeshProUGUI MouseOverDescription;

    // �� ����UI�� �Ҵ�� �κ��丮 ���� ������ (����UI�� �� �κ��丮 ������ ������ '���'�ϴ� ��ġ)
    [SerializeField] private InventorySlot assignedInventorySlot;

    private Button btn;
    public InventorySlot AssignedInventorySlot => assignedInventorySlot;
    public InventoryDisplay ParentDisplay { get; private set;}

    private void Awake()
    {
        ClearSlot(); // �� ����UI�� ����

        btn = GetComponent<Button>();
        btn?.onClick.AddListener(OnUIISlotClick);

        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (assignedInventorySlot.ItemData == null) return;

        MouseOverUI.gameObject.SetActive(true);
        MouseOverDisplayName.text = assignedInventorySlot.ItemData.DisplayName;
        MouseOverDisplayName.color = assignedInventorySlot.ItemData.gradeColor;
        MouseOverDescription.text = assignedInventorySlot.ItemData.Description;
        MouseOverUI.transform.position = new Vector3(eventData.position.x, eventData.position.y + 50f,0);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        MouseOverUI.gameObject.SetActive(false);
    }

    public void ClearSlot() // �� ����UI�� �������� ���� ���� �ʱ�ȭ
    {
        assignedInventorySlot?.ClearSlot();
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = "";
    }

    public void Init(InventorySlot slot)
    {
        assignedInventorySlot = slot; // �κ��丮 ���� �޾ƿ���
        UpdateUISlot(slot); // �޾ƿ� ���� �����ͷ� ����UI ������Ʈ
    }

    public void UpdateUISlot(InventorySlot slot) // ����UI ������Ʈ. ȭ��� ���� ������ ������ֱ�
    {
        if (slot.ItemData != null)
        {
            itemSprite.sprite = slot.ItemData.ItemSprite;
            itemSprite.color = Color.white;

            if (slot.StackSize > 1) itemCount.text = slot.StackSize.ToString();
            else itemCount.text = "";
        } else
            ClearSlot();

        if (slot.StackSize > 1) itemCount.text = slot.StackSize.ToString();
        else itemCount.text = "";
    }

    public void UpdateUISlot() // ���ο� �����ʹ� �ƴϰ� ���ø� �ٲ� ȣ���
    {
        if (assignedInventorySlot != null)
            UpdateUISlot(assignedInventorySlot);
    }

    public void OnUIISlotClick()
    {
        // �θ������Ʈ�� Ŭ���̺�Ʈ�� �� ������ �Ѱ���
        ParentDisplay?.SlotClicked(this);
    }
}
