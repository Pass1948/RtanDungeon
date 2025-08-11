using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPannel;
    public Transform dropPosition;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDeseription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;

    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipeButton;
    public GameObject dropButton;

    ItemData selecteditem;
    int selectedItemIndex = 0;


    private PlayerControl control;
    private PlayerCondition condition;
    int curEquipIndex = 0;

    private void Start()
    {
        control = GameManager.Character.Player.controller;
        condition = GameManager.Character.Player.condition;
        dropPosition = GameManager.Character.Player.dropPosition;

        control.inventory += Toggle;
        GameManager.Character.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPannel.childCount];

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPannel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].Inventory = this;
        }
        ClearSelectedItemWindow();

    }
    void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDeseription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipeButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);

        }
        else
        {
            inventoryWindow.SetActive(true);
        }

    }
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    void AddItem()
    {
        ItemData data = GameManager.Character.Player.ItemData;

        // 아이템 중복 확인
        if (data.canStack)
        {
            ItemSlot slot = GetItemSlot(data);
            if (slot != null)
            {
                slot.quantity++;
                UIUpdate();
                GameManager.Character.Player.ItemData = null;
                return;
            }
        }
        // 비어있는 슬롯 가저오기
        ItemSlot emptySlot = GetEmptySlot();
        if (emptySlot != null)
        {
            emptySlot.Item = data;
            emptySlot.quantity = 1;
            UIUpdate();
            GameManager.Character.Player.ItemData = null ;
            return;
        }
        ThrowItem(data);
        GameManager.Character.Player.ItemData = null;
    }

    void UIUpdate()
    {
        for(int i = 0; i< slots.Length; i++)
        {
            if (slots[i].Item != null)
                slots[i].Set();
            else
                slots[i].Clear();
        }
    }


    ItemSlot GetItemSlot(ItemData data)
    {
        for(int i = 0; i< slots.Length; i++)
        {
            if(slots[i].Item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for(int i = 0; i< slots.Length; i++)
        {
            if (slots[i].Item == null) 
            {
                return slots[i];
            }
        }
        return null;
    }

    void ThrowItem(ItemData data)
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }
        UIUpdate();
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int index)
    {
        if (slots[index].Item == null) return;
        selecteditem = slots[index].Item;
        selectedItemIndex = index;

        selectedItemName.text = selecteditem.displayName;
        selectedItemDeseription.text = selecteditem.description;


        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for(int i =0; i< selecteditem.consumables.Length;  i++)
        {
            selectedStatName.text += selecteditem.consumables[i].type.ToString()+"\n";
            selectedStatValue.text += selecteditem.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selecteditem.type == ItemType.Consumable);
        equipButton.SetActive(selecteditem.type == ItemType.Equipable && !slots[index].equipped);
        unequipeButton.SetActive(selecteditem.type == ItemType.Equipable&& slots[index].equipped);
        dropButton.SetActive(true);

    }

    public void OnUseButton()
    {
        if(selecteditem.type == ItemType.Consumable)
        {
            for(int i =0; i< selecteditem.consumables.Length; i++)
            {
                switch (selecteditem.consumables[i].type)
                {
                    case CounsumableType.Health:
                        condition.Heal(selecteditem.consumables[i].value);
                        break;
                    case CounsumableType.Hunger:
                        condition.Eat(selecteditem.consumables[i].value);
                        break;
                }
            }
            RemoveSeletedItem();
        }
    }
    public void OnDropButton()
    {
        ThrowItem(selecteditem);
        RemoveSeletedItem();
    }

    void RemoveSeletedItem()
    {
        slots[selectedItemIndex].quantity --;

        if(slots[selectedItemIndex].quantity <= 0)
        {
            selecteditem = null;
            slots[selectedItemIndex].Item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }
        UIUpdate();
    }

    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }

        slots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        GameManager.Character.Player.equipment.EquipNew(selecteditem);
        UIUpdate();

        SelectItem(selectedItemIndex);

    }

    void UnEquip(int index)
    {
        slots[index].equipped = false;
        GameManager.Character.Player.equipment.UnEquip();
        UIUpdate();

        if(selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }

    public bool HasItem(ItemData item, int quantity)
    {
        return false;
    }
}
