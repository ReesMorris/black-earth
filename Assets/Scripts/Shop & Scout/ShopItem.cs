using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopItem : MonoBehaviour {

	public int price;
	public Item item;
	public Text notice;

	private ShopManager shopManager;
	private CurrencyManager currencyManager;
	private GameObject player;
	private Inventory inventory;
	private int totalBought = 0;

	void Start() {
		player = GameObject.Find ("Player").gameObject;
		shopManager = GameObject.Find ("GameManager").GetComponent<ShopManager> ();
		currencyManager = GameObject.Find ("GameManager").GetComponent<CurrencyManager> ();
		inventory = GameObject.Find ("GameManager").GetComponent<Inventory> ();
	}

	public void BuyItem() {
		// Has enough money
		if (currencyManager.Souls >= price) {

			// If inventory has enough space
			if (inventory.AddItemToInventory (item)) {
				totalBought++;

				currencyManager.LoseSouls(price);

				if (item.itemType == Item.Types.Weapon) {
					GameObject sound = Instantiate (item.itemPrefab);
					Vector3 position = new Vector3 (player.transform.position.x, -1f, player.transform.position.z);
					sound.transform.position = position;
					sound.GetComponent<Gun> ().reloadSound.Play ();
					StartCoroutine ("DestroyGun", sound);
				}

				StopCoroutine ("Notice");
				StartCoroutine ("Notice", "You have bought x" + totalBought + " " + item.itemName);

			} else {
				StopCoroutine ("Notice");
				StartCoroutine ("Notice", "You do not have enough inventory space!");
				shopManager.errorSound.Play ();
			}
		} else {
			StopCoroutine ("Notice");
			StartCoroutine ("Notice", "You cannot afford this!");
			shopManager.errorSound.Play ();
		}
	}

	IEnumerator Notice(string text) {
		notice.transform.parent.gameObject.SetActive(true);
		notice.text = text;

		Image background = notice.transform.parent.gameObject.GetComponent<Image> ();
		notice.color = new Color (notice.color.r, notice.color.g, notice.color.b, 1f);
		background.color = new Color (background.color.r, background.color.g, background.color.b, 1f);
		yield return new WaitForSeconds (2f);

		while (notice.color.a >= 0) {
			float a = notice.color.a - 0.1f;
			notice.color = new Color (notice.color.r, notice.color.g, notice.color.b, a);
			background.color = new Color (background.color.r, background.color.g, background.color.b, a);

			if (notice.color.a <= 0) {
				notice.transform.parent.gameObject.SetActive(false);
				StopCoroutine ("Notice");
			}

			yield return new WaitForSeconds (0.1f);
		}
	}

	IEnumerator DestroyGun(GameObject gun) {
		yield return new WaitForSeconds (5f);
		Destroy (gun);
	}

}
