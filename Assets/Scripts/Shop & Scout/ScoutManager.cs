using UnityEngine;
using System.Collections.Generic;

public class ScoutManager : MonoBehaviour {

	public GameObject newReportIcon;
	public int uncommonThreshold;
	public int rareThreshold;
	public int veryRareThreshold;
	public int ultraRareThreshold;

	private ItemDatabase itemDatabase;
	private Inventory inventory;

	void Start() {
		itemDatabase = GameObject.Find ("Databases").transform.Find ("ItemDatabase").GetComponent<ItemDatabase> ();
		inventory = GameObject.Find ("GameManager").GetComponent<Inventory> ();
	}

	[Header("Chance of Death")]
	public int deathChance = 0;
	public int deathChanceInc = 7;

	public bool DoesScoutDie(int daysScouting) {
		int chance = deathChance + (daysScouting * deathChanceInc);

		if (Random.Range (0, 100) < chance || chance >= 100) {
			return true;
		}

		return false;
	}

	public void GetScoutItems(int daysScouting) {
		List<Item> scoutItems = new List<Item>();

		for (int i = 0; i < itemDatabase.items.Count; i++) {
			if (itemDatabase.items [i].canFind) {
				scoutItems.Add (itemDatabase.items [i]);
			}
		}

		List<Item> itemsFound = new List<Item> ();

		print (scoutItems.Count);

		if (scoutItems.Count > 0) {
			// Uncommon
			if (daysScouting < uncommonThreshold) {
				for(int i = 0; i < scoutItems.Count; i++) {
					if (scoutItems[i].itemRarity == Item.Rarities.Uncommon || scoutItems[i].itemRarity == Item.Rarities.Rare || scoutItems[i].itemRarity == Item.Rarities.VeryRare || scoutItems[i].itemRarity == Item.Rarities.UltraRare) {
						scoutItems.RemoveAt (i);
					}
				}
			}

			// Rare
			if (daysScouting < rareThreshold && daysScouting > uncommonThreshold) {
				for(int i = 0; i < scoutItems.Count; i++) {
					if (scoutItems[i].itemRarity == Item.Rarities.Rare || scoutItems[i].itemRarity == Item.Rarities.VeryRare || scoutItems[i].itemRarity == Item.Rarities.UltraRare) {
						scoutItems.RemoveAt (i);
					}
				}
			}

			// Very Rare
			if (daysScouting < veryRareThreshold && daysScouting > rareThreshold) {
				for(int i = 0; i < scoutItems.Count; i++) {
					if (scoutItems[i].itemRarity == Item.Rarities.VeryRare || scoutItems[i].itemRarity == Item.Rarities.UltraRare) {
						scoutItems.RemoveAt (i);
					}
				}
			}

			// Ultra Rare
			if (daysScouting < ultraRareThreshold && daysScouting > veryRareThreshold) {
				for(int i = 0; i < scoutItems.Count; i++) {
					if (scoutItems[i].itemRarity == Item.Rarities.UltraRare) {
						scoutItems.RemoveAt (i);
					}
				}
			}

			// Now see how many items should be returned
			int itemsToReturn = Random.Range (0, daysScouting);

			for (int i = 0; i < itemsToReturn; i++) {
				int random = Random.Range (0, scoutItems.Count - 1);
				inventory.AddItemToInventory (scoutItems [random]);
			}
		}
	}

	public void ShowIcon() {
		newReportIcon.SetActive (true);
	}
}
