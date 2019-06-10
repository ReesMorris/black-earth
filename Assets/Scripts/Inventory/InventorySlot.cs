using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public int slotID;

	private bool selected = false;
	public bool Selected {
		get {
			return selected;
		}
		set {
			selected = value;
		}
	}

	private bool mouseHovering = false;
	private Image image;
	private Inventory inventory;
	private MouseManager mouseManager;
	private InventorySelector inventorySelector;
	private InventoryUIManager inventoryUI;
	private Image icon;
	private Sprite defaultIcon;
	private Tooltip tooltip;
	private Item item;
	private bool _active;

	public delegate void OnSlotChanged ();
	public OnSlotChanged slotChangedEvent;

	void Start() {
		_active = gameObject.activeSelf;
		tooltip = GameObject.Find ("UI").transform.Find ("Tooltip").gameObject.GetComponent<Tooltip>();
		inventoryUI = GameObject.Find("GameManager").GetComponent<InventoryUIManager> ();
		inventorySelector = GameObject.Find ("GameManager").GetComponent<InventorySelector> ();
		image = GetComponent<Image> ();
		icon = transform.Find ("Icon").GetComponent<Image> ();
		defaultIcon = icon.sprite;
		mouseManager = GameObject.Find("GameManager").GetComponent<MouseManager> ();

		inventory = GameObject.Find ("GameManager").GetComponent<Inventory> ();
		inventory.inventoryChangedEvent += OnInventoryChanged;
		item = inventory.InventoryList [slotID];

		OnInventoryChanged ();
	}

	public void OnPointerEnter(PointerEventData dataName) {
		mouseHovering = true;
		mouseManager.ChangeMouse (false, true);
		ChangeUI ();
	}

	public void OnPointerExit(PointerEventData dataName) {
		mouseHovering = false;
		mouseManager.ChangeMouse (true, false);
		ChangeUI ();

		tooltip.Hide ();
	}

	void Update() {
		if (_active != gameObject.activeSelf) {
			if (!_active) {
				OnInventoryChanged ();
			}

			_active = gameObject.activeSelf;
		}

		// Shift click
		if (Input.GetKey (KeyCode.LeftShift)) {
			if (Input.GetMouseButton (0)) {
				if (mouseHovering && item.itemName != "") {
					ShiftClick ();
				}
			}
		}

		// Left click
		if (Input.GetMouseButtonDown (0)) {
			if (mouseHovering) {
				if (item.itemName != "" || inventoryUI.GetItemAtMouse () != null) {
					MoveToMouse ();
				}
			}
		}

		if (mouseHovering) {
			if (item != null) {
				if (item.itemName != "") {
					string desc = item.itemDescription;
					if (item.Amount > 1) {
						desc += "\nYou have " + item.Amount + " of these.";
					}
					tooltip.ChangeText (item.itemName, desc);
					tooltip.SetPosition (new Vector2(Input.mousePosition.x, Input.mousePosition.y - 100f + tooltip.BodyHeight()));
				}
			}
		}
	}

	public void ChangeUI() {
		image = GetComponent<Image> ();
		if (mouseHovering) {
			image.color = new Color (image.color.r, image.color.g, image.color.b, 0.85f);
		} else if(selected) {
			image.color = new Color (255, 255, 50, 1);
		} else {
			image.color = new Color (image.color.r, image.color.g, image.color.b, 0.75f);
		}
	}

	void OnInventoryChanged() {
		Item newItem = inventory.InventoryList [slotID];
		if (item != newItem && selected) {
			if (newItem.itemName != "") {
				inventorySelector.ShowItemName (newItem.itemName);
				inventorySelector.ChangeWeapon (newItem.itemPrefab.GetComponent<Gun> ());
			} else {
				inventorySelector.ChangeWeapon (null);
			}
		}
		item = inventory.InventoryList [slotID];
		Text text = transform.Find ("Amount").GetComponent<Text> ();

		if (item.itemName == "") {
			icon.sprite = defaultIcon;
			text.text = "";
		} else {
			icon.sprite = item.itemSprite;

			// Show amount
			if (item.Amount > 1) {
				text.text = item.Amount.ToString();
			} else {
				text.text = "";
			}
		}

		if (slotChangedEvent != null) {
			slotChangedEvent ();
		}
	}

	void ShiftClick() {
		// From Hotbar
		if (this.slotID < 7) {
			for (int i = 7; i < inventory.InventoryList.Count; i++) {
				Item item = inventory.InventoryList [i];
				// Empty space
				if (item.itemName == "") {
					inventory.MoveItemToSlot (slotID, i);
					tooltip.Hide ();

					GameObject[] allSlots = GameObject.FindGameObjectsWithTag ("InventorySlot");
					for(int j = 0; j < allSlots.Length; j++) {
						if (allSlots [j].GetComponent<InventorySlot> ().slotID == i) {
							InventorySlotData slotData = allSlots [j].GetComponent<InventorySlotData> ();
							InventorySlotData thisSlot = GetComponent<InventorySlotData> ();

							slotData.SetItem (inventory.InventoryList [slotID]);
							slotData.currentAmmo = thisSlot.currentAmmo;
							slotData.itemHealth = thisSlot.itemHealth;
							slotData.SetItemHealth();
							break;
						}
					}
					break;
				}
			}
		}

		// From Inventory
		if (this.slotID >= 7) {
			for (int i = 0; i < 7; i++) {
				Item item = inventory.InventoryList [i];
				// Empty space
				if (item.itemName == "") {
					inventory.MoveItemToSlot (slotID, i);
					tooltip.Hide ();

					GameObject[] allSlots = GameObject.FindGameObjectsWithTag ("InventorySlot");
					for(int j = 0; j < allSlots.Length; j++) {
						if (allSlots [j].GetComponent<InventorySlot> ().slotID == i) {
							InventorySlotData slotData = allSlots [j].GetComponent<InventorySlotData> ();
							InventorySlotData thisSlot = GetComponent<InventorySlotData> ();

							slotData.SetItem (inventory.InventoryList [slotID]);
							slotData.currentAmmo = thisSlot.currentAmmo;
							slotData.itemHealth = thisSlot.itemHealth;
							slotData.SetItemHealth();
							break;
						}
					}
					break;
				}
			}
		}
	}

	void MoveToMouse() {
		Item item = inventory.InventoryList [slotID];

		// Taking from slot
		if (inventoryUI.GetItemAtMouse ().itemName == "" && item.itemName != "") {
			inventoryUI.SetAtMouse (item, slotID, gameObject, GetComponent<InventorySlotData>());
			inventory.RemoveAtIndex (slotID);
			inventoryUI.GetSlotAtMouse ().GetComponent<InventorySlotData> ().Empty();

			if (slotChangedEvent != null) {
				slotChangedEvent ();
			}
		} 

		// Putting in slot
		else {

			// Slot is empty
			if (inventory.InventoryList [slotID].itemName == "") {
				inventory.AddItemToInventory (inventoryUI.GetItemAtMouse(), slotID);
				InventorySlotData slotData = GetComponent<InventorySlotData> ();
				slotData.SetItem (inventory.InventoryList [slotID]);
				slotData.currentAmmo = inventoryUI.GetWeaponAmmoAtMouse ();
				slotData.itemHealth = inventoryUI.GetHealthAtMouse ();
				slotData.SetItemHealth();
			}

			// Swap places
			else {
				InventorySlotData thisSlot = GetComponent<InventorySlotData> ();
				InventorySlotData otherSlot = inventoryUI.GetSlotAtMouse ().GetComponent<InventorySlotData> ();

				int heldAmmo = inventoryUI.GetWeaponAmmoAtMouse (); // the ammo of the item being moved
				int newAmmo = thisSlot.currentAmmo; // the ammo of this slot

				float heldHealth = inventoryUI.GetHealthAtMouse (); // the health of the item being moved
				float newHealth = thisSlot.itemHealth; // the health of this slot

				// Swap the slots
				inventory.AddItemToInventory (item, inventoryUI.GetIndexAtMouse ());
				inventory.AddItemToInventory (inventoryUI.GetItemAtMouse (), slotID);

				// Replace the values
				thisSlot.currentAmmo = heldAmmo;
				thisSlot.itemHealth = heldHealth;
				otherSlot.currentAmmo = newAmmo;
				otherSlot.itemHealth = newHealth;

			}

			inventoryUI.EmptyAtMouse ();
		}
	}
}