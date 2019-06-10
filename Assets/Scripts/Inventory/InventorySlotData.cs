using UnityEngine;
using UnityEngine.UI;

public class InventorySlotData : MonoBehaviour {

	[Header("Weapon")]
	public int currentAmmo;
	public float itemHealth;

	private Inventory inventory;
	private InventorySlot inventorySlot;
	private InventoryUIManager inventoryUI;
	private Item slotItem;
	private int inventoryLength;


	void Start() {
		Empty ();
		inventory = GameObject.Find ("GameManager").GetComponent<Inventory> ();
		inventoryUI = GameObject.Find("GameManager").GetComponent<InventoryUIManager> ();

		Image damageImage = transform.Find ("Damage").transform.Find ("DamageIndicator").GetComponent<Image> ();
		damageImage.fillAmount = 0;

		inventorySlot = GetComponent<InventorySlot> ();
		inventorySlot.slotChangedEvent += OnSlotChanged;
		inventoryLength = inventory.InventoryList.Count;
	}

	void OnSlotChanged() {
		Item inventoryItem = inventory.InventoryList [inventorySlot.slotID];
		if (inventoryItem.itemName == "") {
			slotItem = null;
		} else if (inventoryItem.itemName != "" && inventoryUI.GetIndexAtMouse () == -1 && inventory.InventoryList.Count != inventoryLength) {
			inventoryLength = GetInventoryCount();
			if (inventoryItem != slotItem) {
				slotItem = inventoryItem;
				if (inventoryItem.itemType == Item.Types.Weapon) {
					Gun gun = inventoryItem.itemPrefab.GetComponent<Gun> ();
					currentAmmo = gun.clipSize;
					itemHealth = gun.startingHealth;
					SetItemHealth ();
				}
			}
		}
	}

	public void SetItem(Item item) {
		slotItem = item;
	}

	public void Empty() {
		slotItem = null;
		currentAmmo = 0;
		itemHealth = 0;
		SetItemHealth ();
	}

	int GetInventoryCount() {
		int count = 0;

		for (int i = 0; i < inventory.InventoryList.Count; i++) {
			if (inventory.InventoryList [i].itemName != "") {
				count++;
			}
		}

		return count;
	}

	public void SetItemHealth() {
		Image damageImage = transform.Find ("Damage").transform.Find ("DamageIndicator").GetComponent<Image> ();
		damageImage.fillAmount = 0;
		if (slotItem != null) {
			Item inventoryItem = inventory.InventoryList [inventorySlot.slotID];
			if (inventoryItem.itemName == "") {
				slotItem = null;
			} else if (inventoryItem.itemType == Item.Types.Weapon) {
				Gun gun = inventoryItem.itemPrefab.GetComponent<Gun> ();

				if (gun.takesDamage) {
					damageImage.fillAmount = itemHealth / gun.startingHealth;
				}
			}
		}
	}
}
