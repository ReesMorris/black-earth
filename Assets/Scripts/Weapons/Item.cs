using UnityEngine;

[System.Serializable]
public class Item {

	[Header("General")]
	public string itemName;
	public string itemDescription;
	public Sprite itemSprite;
	public GameObject itemPrefab;
	public enum Types{Weapon};
	public Types itemType;
	public Sprite crosshair;

	[Header("Stacking")]
	public bool canStack;

	[Header("Drops")]
	public bool enemyCanDrop;
	public enum Rarities{Common, Uncommon, Rare, VeryRare, UltraRare};
	public Rarities itemRarity;

	[Header("Shop - Buy")]
	public bool canBuy;
	public int buyValue;
	public int availableFromWave = 0;
	public string shopDescription;
	public bool canGoOnSale;
	public int saleValue;

	[Header("Scout")]
	public bool canFind;

	private int amount = 1;
	public int Amount {
		get {
			return amount;
		}
		set {
			amount = value;
		}
	}

	public Item(string name = "", string description = "", Sprite sprite = null, GameObject prefab = null, Rarities rarity = Rarities.Common) {
		itemName = name;
		itemDescription = description;
		itemSprite = sprite;
		itemPrefab = prefab;
		itemRarity = rarity;
		amount = 0;
	}

}