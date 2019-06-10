using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AmmoUI : MonoBehaviour {

	public Text currentAmmo;
	public Text maxAmmo;
	public Text reloadingText;

	private RectTransform maxAmmoRT;

	void Start() {
		maxAmmoRT = maxAmmo.GetComponent<RectTransform> ();
		StopReloadText ();
	}

	public void Display(bool showing) {
		for (int i = 0; i < transform.childCount; i++) {
			if (showing) {
				transform.GetChild (i).gameObject.SetActive (true);
			} else {
				transform.GetChild (i).gameObject.SetActive (false);
			}
		}
	}

	public void SetCurrentAmmo(int amount) {
		currentAmmo.text = amount.ToString();
	}

	public void SetMaxAmmo(int amount) {
		maxAmmoRT = maxAmmo.GetComponent<RectTransform> ();
		maxAmmo.text = amount.ToString();
		maxAmmoRT.sizeDelta = CalculateSize ();
	}

	Vector2 CalculateSize() {
		maxAmmoRT = maxAmmo.GetComponent<RectTransform> ();
		float width = LayoutUtility.GetPreferredWidth (maxAmmoRT); // [5]
		return new Vector2 (width, maxAmmoRT.sizeDelta.y);
	}

	public void StartReloadText() {
		reloadingText.color = new Color (reloadingText.color.r, reloadingText.color.g, reloadingText.color.b, 0f);
		StopCoroutine ("ReloadText");
		StartCoroutine ("ReloadText");
	}

	public void StopReloadText() {
		StopCoroutine ("ReloadText");
		reloadingText.color = new Color (reloadingText.color.r, reloadingText.color.g, reloadingText.color.b, 0f);
	}

	IEnumerator ReloadText() {
		Color col = reloadingText.color;
		bool fadingIn = true;

		while (true) {
			if (fadingIn) {
				reloadingText.color = new Color (col.r, col.g, col.b, reloadingText.color.a + 0.1f);
				if (reloadingText.color.a >= 1) {
					fadingIn = false;
				}
			} else {
				reloadingText.color = new Color (col.r, col.g, col.b, reloadingText.color.a - 0.1f);
				if (reloadingText.color.a <= 0.5) {
					fadingIn = true;
				}
			}
			yield return new WaitForSeconds (0.1f);
		}
	}
}
