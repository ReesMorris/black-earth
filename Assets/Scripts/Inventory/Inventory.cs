using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	private List<Item> inventory = new List<Item>();
	public List<Item> InventoryList {
		get {
			return inventory;
		}
	}

	public int totalSlots = 35;
	public int[] startingItemIds;

	public delegate void InventoryChanged ();
	public InventoryChanged inventoryChangedEvent; // [6]

	private ItemDatabase itemDatabase;
	private InventorySelector inventorySelector;

	void Start() {
		itemDatabase = GameObject.Find ("Databases").transform.Find ("ItemDatabase").GetComponent<ItemDatabase> ();
		inventorySelector = GetComponent<InventorySelector> ();

		for (int i = 0; i < totalSlots; i++) {
			if (startingItemIds.Length > i) {
				Item newItem = itemDatabase.items [startingItemIds [i]];
				inventory.Add (newItem);
				if (i == inventorySelector.CurrentSlot) {
					inventorySelector.ShowItemName (newItem.itemName);
					if (newItem.itemType == Item.Types.Weapon) {
						GunManager gunManager = GameObject.Find ("GameManager").GetComponent<GunManager> ();
						gunManager.ammoUI.SetMaxAmmo (newItem.itemPrefab.GetComponent<Gun> ().clipSize);
						gunManager.ammoUI.SetCurrentAmmo (0);
					}
				}
			} else {
				inventory.Add (new Item ()); // placeholder for future items
			}
		}
	}

	// Inventory items should be added one at a time, using a for loop for multiple
	public bool AddItemToInventory(Item item, int index = -1) {
		bool inInventory = false;

		if (index == -1) {
			// Is it already in the inventory?
			if (item.canStack) {
				for (int i = 0; i < inventory.Count; i++) {
					// Is it here?
					if (item.itemName == inventory [i].itemName) {
						inInventory = true;
						inventory [i].Amount++;
						break;
					}
				}
			}

			// It's not in there already
			if (!inInventory) {
				for (int i = 0; i < inventory.Count; i++) {
					// Check if each slot is empty
					if (inventory [i].itemName == "") {
						// Make sure that there's at least 1 item
						item.Amount = 1;
						inventory [i] = item;
						inInventory = true;
						break;
					}
				}
			}
		} else {
			inventory [index] = item;
			inInventory = true;
		}

		if (inInventory) {
			// Toggle the event saying that the inventory is changed
			if (inventoryChangedEvent != null) {
				inventoryChangedEvent ();
			}
			return true;
		}

		// Sorry, there's no room in this inn (inventory full).
		return false;
	}

	public bool MoveItemToSlot(int oldIndex, int newIndex) {
		if (inventory [newIndex].itemName == "") {
			inventory [newIndex] = inventory [oldIndex];
			inventory [oldIndex] = new Item ();

			if (inventoryChangedEvent != null) {
				inventoryChangedEvent ();
			}
			return true;
		}

		return false;
	}

	public void RemoveAtIndex(int index) {
		inventory [index] = new Item ();
		if (inventoryChangedEvent != null) {
			inventoryChangedEvent ();
		}
	}
}