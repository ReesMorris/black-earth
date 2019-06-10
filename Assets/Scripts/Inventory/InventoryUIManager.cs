using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryUIManager : MonoBehaviour {

	public GameObject inventorySlotPrefab;
	public GameObject[] inventoryUI;

	private Inventory inventory;
	private Tooltip tooltip;

	private Item itemAtMouse;
	private int indexAtMouse = -1;
	private GameObject item;
	private CameraManager cameraManager;
	private int weaponAmmoAtMouse;
	private float weaponHealthAtMouse;
	private GameObject slotAtMouse;
	private PauseManager pauseManager;

	void Start() {
		itemAtMouse = new Item ();
		inventory = GetComponent<Inventory> ();
		tooltip = GameObject.Find ("UI").transform.Find ("Tooltip").gameObject.GetComponent<Tooltip>();
		item = Instantiate (inventorySlotPrefab) as GameObject;
		item.tag = "Untagged";
		item.GetComponent<Image> ().color = new Color (item.GetComponent<Image> ().color.r, item.GetComponent<Image> ().color.g, item.GetComponent<Image> ().color.b, 0f);
		item.transform.SetParent(GameObject.Find ("UI").transform, false);
		Destroy(item.GetComponent<InventorySlot>());
		item.GetComponent<Image> ().raycastTarget = false;
		cameraManager = GetComponent<CameraManager> ();
		pauseManager = GetComponent<PauseManager> ();

		foreach (GameObject _item in inventoryUI) {
			_item.SetActive (false);
		}
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (inventoryUI [0].activeSelf) {
				foreach (GameObject item in inventoryUI) {
					item.SetActive (false);
					cameraManager.EnableBlur (false);
					pauseManager.SetModalOpen(false);
				}
			}
		}

		else if (Input.GetKeyDown (KeyCode.I) || Input.GetKeyDown(KeyCode.Tab)) {
			if (!pauseManager.gameIsPaused) {
				if (!pauseManager.modalIsOpen || (pauseManager.modalIsOpen && pauseManager.modalName == "inventory")) {
					foreach (GameObject item in inventoryUI) {
						item.SetActive (!item.activeSelf);
					}

					StartCoroutine (UpdateInventory ());

					if (inventoryUI.Length > 0) {
						if (!inventoryUI [0].activeSelf) {
							tooltip.Hide ();
							pauseManager.modalIsOpen = false;
							cameraManager.EnableBlur (false);
						} else {
							pauseManager.modalIsOpen = true;
							pauseManager.modalName = "inventory";
							cameraManager.EnableBlur (true);
							cameraManager.SetBlurValues (1, 0.111f);
						}
					}
				}
			}
		}

		if (indexAtMouse != -1) {
			item.transform.position = Input.mousePosition;
		} else {
			item.transform.position = new Vector3 (Screen.width * 2, Screen.height * 2, 0f);
		}
	}

	IEnumerator UpdateInventory() {
		yield return new WaitForSeconds (0.1f);
		inventory.inventoryChangedEvent ();
	}

	public void SetAtMouse(Item _item, int index, GameObject _slot, InventorySlotData _data) {
		itemAtMouse = _item;
		indexAtMouse = index;
		slotAtMouse = _slot;
		weaponAmmoAtMouse = _data.currentAmmo;
		weaponHealthAtMouse = _data.itemHealth;

		item.transform.Find ("Icon").GetComponent<Image> ().sprite = _item.itemSprite;
		item.transform.Find ("Amount").GetComponent<Text> ().text = "";
		if (_item.Amount > 1) {
			item.transform.Find ("Amount").GetComponent<Text> ().text = _item.Amount.ToString();
		}
	}

	public void EmptyAtMouse() {
		itemAtMouse = new Item();
		indexAtMouse = -1;
		weaponAmmoAtMouse = 0;
		weaponHealthAtMouse = 0;
		slotAtMouse = null;
	}

	public int GetWeaponAmmoAtMouse() {
		return weaponAmmoAtMouse;
	}

	public Item GetItemAtMouse() {
		return itemAtMouse;
	}

	public int GetIndexAtMouse() {
		return indexAtMouse;
	}

	public float GetHealthAtMouse() {
		return weaponHealthAtMouse;
	}

	public GameObject GetSlotAtMouse() {
		return slotAtMouse;
	}
}