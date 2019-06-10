using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour {

	public int databaseID;

	private ItemDatabase database;
	private Inventory inventory;
	private bool checking = false;

	void Start() {
		database = GameObject.Find ("Databases").transform.Find("ItemDatabase").GetComponent<ItemDatabase> ();
		inventory = GameObject.Find("GameManager").GetComponent<Inventory>();
	}

	public void DropItem() {
		string itemTag = "DroppedItem";

		gameObject.transform.parent = null;

		gameObject.AddComponent<Rigidbody> ();

		gameObject.tag = itemTag;
		gameObject.layer = 13;

		Transform[] allChildren = GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren) {
			child.gameObject.tag = itemTag;
			child.gameObject.layer = 13;
			if (child.name == "PickupTrigger") {
				child.gameObject.tag = "Untagged";
			}
			child.gameObject.AddComponent<BoxCollider> ();
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Survivor" && gameObject.layer == 13 && !checking) {
			checking = true;
			Item item = database.items [databaseID];
			if (inventory.AddItemToInventory (item)) {
				Destroy (gameObject);
			} else {
				checking = false;
			}
		}
	}
}
