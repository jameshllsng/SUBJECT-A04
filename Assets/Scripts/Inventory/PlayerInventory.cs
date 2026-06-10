using System;
using System.Collections.Generic;
using UnityEngine;

namespace SubjectA04.InventorySystem
{
    [Serializable]
    public class InventoryItemStack
    {
        [SerializeField] private string itemId;
        [SerializeField] private string displayName;
        [SerializeField] private int quantity;

        public string ItemId => itemId;
        public string DisplayName => displayName;
        public int Quantity => quantity;

        public InventoryItemStack(string itemId, string displayName, int quantity)
        {
            this.itemId = itemId;
            this.displayName = displayName;
            this.quantity = quantity;
        }

        public void Add(int amount)
        {
            quantity += amount;
        }

        public bool Remove(int amount)
        {
            if (amount <= 0 || quantity < amount)
            {
                return false;
            }

            quantity -= amount;
            return true;
        }
    }

    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private List<InventoryItemStack> items = new List<InventoryItemStack>();

        public IReadOnlyList<InventoryItemStack> Items => items;

        public event Action Changed;

        public void AddItem(string itemId, string displayName, int amount)
        {
            if (string.IsNullOrWhiteSpace(itemId) || amount <= 0)
            {
                return;
            }

            InventoryItemStack stack = FindStack(itemId);
            if (stack == null)
            {
                string resolvedName = string.IsNullOrWhiteSpace(displayName) ? itemId : displayName;
                items.Add(new InventoryItemStack(itemId, resolvedName, amount));
            }
            else
            {
                stack.Add(amount);
            }

            Changed?.Invoke();
            Debug.Log($"{name} received {amount}x {displayName}. Total: {GetQuantity(itemId)}.", this);
        }

        public bool RemoveItem(string itemId, int amount)
        {
            InventoryItemStack stack = FindStack(itemId);
            if (stack == null || !stack.Remove(amount))
            {
                return false;
            }

            if (stack.Quantity == 0)
            {
                items.Remove(stack);
            }

            Changed?.Invoke();
            return true;
        }

        public int GetQuantity(string itemId)
        {
            InventoryItemStack stack = FindStack(itemId);
            return stack?.Quantity ?? 0;
        }

        private InventoryItemStack FindStack(string itemId)
        {
            return items.Find(item => item.ItemId == itemId);
        }
    }
}
