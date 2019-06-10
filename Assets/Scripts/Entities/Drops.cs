using UnityEngine;
using System.Collections.Generic;

public class Drops : MonoBehaviour {

	[Range(0, 100)]
	public float chanceOfHavingItem;

	private List<GameObject> potentialDrops;
	private GameObject heldItem;
	private ItemDatabase database;

	void Start() {
		potentialDrops = new List<GameObject> ();
		database = GameObject.Find ("Databases").transform.Find("ItemDatabase").GetComponent<ItemDatabase> ();

		float random = Random.Range (0, 100);
		if (random <= chanceOfHavingItem) {
			// Enemy is carrying an item!

			// calculate the rarities
			for (int i = 0; i < database.items.Count; i++) {
				Item item = database.items[i];

				// if it can be dropped by an enemy
				if (item.enemyCanDrop) {
					switch (item.itemRarity) {
					case Item.Rarities.Common:
						AddToPotentialDrops (item.itemPrefab, 10);
						break;
					case Item.Rarities.Uncommon:
						AddToPotentialDrops (item.itemPrefab, 8);
						break;
					case Item.Rarities.Rare:
						AddToPotentialDrops (item.itemPrefab, 5);
						break;
					case Item.Rarities.VeryRare:
						AddToPotentialDrops (item.itemPrefab, 3);
						break;
					case Item.Rarities.UltraRare:
						AddToPotentialDrops (item.itemPrefab, 1);
						break;
					}
				}
			}

			// pick an item, if possible
			if (potentialDrops.Count > 0) {
				int randomNum = Random.Range (0, potentialDrops.Count);
				heldItem = potentialDrops [randomNum];
			}
		}
	}

	void AddToPotentialDrops(GameObject item, int amount) {
		for(int i = 0; i < amount; i++) {
			potentialDrops.Add (item);
		}
	}

	public bool IsHoldingItem() {
		if (heldItem == null) {
			return false;
		}
		return true;
	}

	public GameObject GetHeldItem() {
		return heldItem;
	}
}