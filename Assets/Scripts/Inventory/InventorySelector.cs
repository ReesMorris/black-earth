using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventorySelector : MonoBehaviour {

	public Text itemName;

	public delegate void InventorySlotChange();
	public InventorySlotChange inventorySlotChanged;

	private int currentSlot = 0;
	public int CurrentSlot {
		get {
			return currentSlot;
		}
	}
	private GameObject currentSlotObject;
	public GameObject CurrentSlotObject {
		get {
			return currentSlotObject;
		}
	}

	private GameObject[] primarySlots;
	private Inventory inventory;
	private PauseManager pauseManager;
	private GameObject player;

	void Start() {
		inventory = GameObject.Find ("GameManager").GetComponent<Inventory> ();
		player = GameObject.Find ("Player").gameObject;
		pauseManager = GetComponent<PauseManager> ();
		primarySlots = GameObject.FindGameObjectsWithTag ("InventorySlot");
		ChangeSlot (0, true);
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			ChangeSlot (0);
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			ChangeSlot (1);
		} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			ChangeSlot (2);
		} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
			ChangeSlot (3);
		} else if (Input.GetKeyDown (KeyCode.Alpha5)) {
			ChangeSlot (4);
		} else if (Input.GetKeyDown (KeyCode.Alpha6)) {
			ChangeSlot (5);
		} else if (Input.GetKeyDown (KeyCode.Alpha7)) {
			ChangeSlot (6);
		}

		string OS = SystemInfo.operatingSystem.Split (' ') [0];
		if (OS == "Mac") {
			if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
				int newSlot = currentSlot + 1;
				if (newSlot > 6) {
					newSlot = 0;
				}
				ChangeSlot (newSlot);
			}

			if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
				int newSlot = currentSlot - 1;
				if (newSlot < 0) {
					newSlot = 6;
				}
				ChangeSlot (newSlot);
			}
		} else {
			if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
				int newSlot = currentSlot + 1;
				if (newSlot > 6) {
					newSlot = 0;
				}
				ChangeSlot (newSlot);
			}

			if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
				int newSlot = currentSlot - 1;
				if (newSlot < 0) {
					newSlot = 6;
				}
				ChangeSlot (newSlot);
			}
		}
	}

	void ChangeSlot(int index, bool overrideCurrent = false) {
		if (!pauseManager.gameIsPaused) {
			if (currentSlot != index || overrideCurrent) {
				for (int i = 0; i < primarySlots.Length; i++) {
					InventorySlot slot = primarySlots [i].GetComponent<InventorySlot> ();
					if (slot.slotID == index) {
						slot.Selected = true;
						currentSlotObject = primarySlots [i];
						slot.ChangeUI ();

						if (inventory.InventoryList.Count > slot.slotID) {
							Item slotItem = inventory.InventoryList [slot.slotID];
							if (slotItem.itemName != "") {
								ShowItemName (slotItem.itemName);

								if (slotItem.itemType == Item.Types.Weapon) {
									ChangeWeapon (slotItem.itemPrefab.GetComponent<Gun> ());
								}
							} else {
								ShowItemName ("");
								ChangeWeapon (null);
							}
						}

					} else if (slot.slotID == currentSlot) {
						slot.Selected = false;
						slot.ChangeUI ();
					}
				}

				currentSlot = index;

				if (inventorySlotChanged != null) {
					inventorySlotChanged ();
				}
			}
		}
	}

	public void ShowItemName(string itemName) {
		StopCoroutine ("ItemName");
		StartCoroutine("ItemName", itemName);
	}

	IEnumerator ItemName(string name) {
		itemName.text = name.ToUpper();
		itemName.color = new Color (255, 255, 255, 1);
		Color itemColor = itemName.color;

		yield return new WaitForSeconds (1.5f);
		while (itemColor.a > 0) {
			itemName.GetComponent<Text> ().color = new Color (itemColor.r, itemColor.g, itemColor.b, itemName.color.a - 0.1f);
			yield return new WaitForSeconds (0.1f);
		}
	}

	public void ChangeWeapon(Gun gun) {
		if (player.GetComponent<GunController> ().EquippedGun != null) {
			player.GetComponent<GunController> ().EquippedGun.StopReload ();
		}

		if (gun == null) {
			player.GetComponent<GunController> ().EquipGun (null);
		} else {
			player.GetComponent<GunController> ().EquipGun (gun);
		}
	}
}
