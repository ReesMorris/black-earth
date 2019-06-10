using UnityEngine;
using UnityEngine.UI;

public class TopPanel : MonoBehaviour {

	private GameObject text;
	private RectTransform textRT;

	void Start() {
		text = transform.Find ("Text").gameObject;
		textRT = text.GetComponent<RectTransform> ();

		ResizeContainer ();
	}

	public void ResizeContainer() {
		GetComponent<RectTransform>().sizeDelta = CalculateSize(); // [3]
	}

	// Will calculate the correct width of the container, including padding
	// based on the text and images
	Vector2 CalculateSize() {
		float width = 0;
		if (transform.Find ("Image").gameObject.activeSelf) {
			width += 45f;
		} else {
			text.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (8f, 0f, 0f); // [4]
			width += 10f;
		}

		width += LayoutUtility.GetPreferredWidth (textRT); // [5]
		width += 6f;

		return new Vector2 (width, GetComponent<RectTransform>().sizeDelta.y);
	}

}
