using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour {

	[Header("Settings")]
	public float chanceOfSale = 20f;

	[Header("GameObjects")]
	public int shopsPerRow = 2;
	public Image menuAlertIcon;
	public GameObject shopContainer;
	public GameObject shopRowPrefab;
	public GameObject shopItemPrefab;
	public GameObject money;
	public GameObject shopEmpty;
	public Sprite saleSprite;
	public Sprite newSprite;
	public Sprite nothingSprite;
	public AudioSource errorSound;

	private ItemDatabase itemDatabase;
	private WaveManager waveManager;
	private CurrencyManager currencyManager;
	private bool generated = false;
	private Item saleItem = null;
	private int itemsLastWave = 0;
	private ReportManager reportManager;

	void Start() {
		waveManager = GetComponent<WaveManager> ();
		currencyManager = GetComponent<CurrencyManager> ();
		reportManager = GetComponent<ReportManager> ();
		itemDatabase = GameObject.Find ("Databases").transform.Find("ItemDatabase").GetComponent<ItemDatabase> ();

		waveManager.waveEndEvent += OnWaveEnd;
		currencyManager.soulsChangedEvent += ChangeMoney;

		GenerateShop ();
		ChangeMoney ();

		itemsLastWave = 0;
	}

	void OnWaveEnd() {
		GenerateShop ();
	}

	public void ChangeMoney() {
		money.transform.Find ("Text").GetComponent<Text> ().text = currencyManager.Souls.ToString();
		ResizeMoney ();
	}

	void ResizeMoney() {
		RectTransform moneyRT = money.GetComponent<RectTransform> ();
		RectTransform textRT = money.transform.Find ("Text").GetComponent<RectTransform> ();

		float width = 100f;
		width += LayoutUtility.GetPreferredWidth (textRT); // [5]

		moneyRT.sizeDelta = new Vector2(width, moneyRT.sizeDelta.y);
	}

	void GenerateShop() {
		// Empty the shop
		menuAlertIcon.gameObject.SetActive(false);
		foreach (Transform child in shopContainer.transform) {
			Destroy (child.gameObject);
		}

		// Generate a new list of items
		List<Item> shopItems = new List<Item>();
		for (int i = 0; i < itemDatabase.items.Count; i++) {
			Item item = itemDatabase.items [i];

			if (item.canBuy && waveManager.CurrentWave >= item.availableFromWave) {
				shopItems.Add (item);
			}
		}

		// Chance of sale
		saleItem = null;
		if (Random.Range (0, 100) <= chanceOfSale && shopItems.Count > 0) {
			int index = Random.Range (0, shopItems.Count);
			saleItem = shopItems [index];
			menuAlertIcon.sprite = saleSprite;
			menuAlertIcon.gameObject.SetActive(true);

			reportManager.AddToReport (ReportManager.ReportTypes.ShopSale);
		}

		// Are there new items?
		if (shopItems.Count > itemsLastWave) {
			menuAlertIcon.sprite = newSprite;
			menuAlertIcon.gameObject.SetActive(true);

			for (int i = 0; i < (shopItems.Count - itemsLastWave); i++) {
				reportManager.AddToReport (ReportManager.ReportTypes.ShopItem);
			}

			itemsLastWave = shopItems.Count;
		}

		// Display the items
		float totalRows = (shopItems.Count * 1f) / (shopsPerRow * 1f);
		int currentCount = 0;

		if (totalRows == 0) {
			GameObject empty = Instantiate (shopEmpty) as GameObject;
			empty.transform.SetParent (shopContainer.transform, false);
		}

		for (int i = 0; i < totalRows; i++) {
			GameObject row = Instantiate (shopRowPrefab) as GameObject;
			row.transform.SetParent (shopContainer.transform, false);

			for (int j = 0; j < shopsPerRow; j++) {
				if (shopItems.Count > currentCount) {
					GameObject shop = Instantiate (shopItemPrefab) as GameObject;
					shop.transform.SetParent (row.transform, false);

					Item item = shopItems [currentCount];
					shop.name = item.itemName;
					ShopItem shopItem = shop.GetComponent<ShopItem> ();

					shop.transform.Find ("Image").GetComponent<Image> ().sprite = nothingSprite;
					shop.transform.Find ("Image").transform.Find ("Icon").GetComponent<Image> ().sprite = item.itemSprite;
					shop.transform.Find ("Title").GetComponent<Text> ().text = item.itemName;
					shop.transform.Find ("Description").GetComponent<Text> ().text = item.shopDescription;
					shop.transform.Find ("Pricing").transform.Find ("Price").GetComponent<Text> ().text = item.buyValue.ToString ();

					shopItem.price = item.buyValue;
					shopItem.item = item;

					if (saleItem == item) {
						shop.transform.Find ("Image").transform.Find ("Image").gameObject.SetActive (true);
						shop.transform.Find ("Pricing").transform.Find ("Price").GetComponent<Text> ().text = item.saleValue.ToString ();
						shopItem.price = item.saleValue;
					}

					currentCount++;
				} else {
					if (currentCount % shopsPerRow != 0) {
						GameObject shop = Instantiate (shopItemPrefab) as GameObject;
						shop.transform.SetParent (row.transform, false);
						shop.name = "Empty";

						shop.GetComponent<Image> ().color = new Color (255, 255, 255, 0f);
						shop.transform.Find ("Image").gameObject.SetActive (false);
						shop.transform.Find ("Title").gameObject.SetActive (false);
						shop.transform.Find ("Description").gameObject.SetActive (false);
						shop.transform.Find ("Pricing").gameObject.SetActive (false);
					}
				}
			}
		}
	}
}
