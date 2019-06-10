using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {

	public Text titleText;
	public GameObject descriptionObject;
	public GameObject bodyObject;

	private RectTransform rtDesc;
	private RectTransform rtBody;

	void Start() {
		rtDesc = descriptionObject.GetComponent<RectTransform> ();
		rtBody = bodyObject.GetComponent<RectTransform> ();

		Hide ();
	}

	public void ChangeText(string title, string description) {
		titleText.text = title;
		descriptionObject.GetComponent<Text>().text = description;

		rtBody.sizeDelta = CalculateHeight ();
	}

	// Makes the tooltip responsive
	Vector2 CalculateHeight() {
		float height = 0;

		height += 50f; // padding
		height += LayoutUtility.GetPreferredHeight (rtDesc); // [5]

		return new Vector2 (rtBody.sizeDelta.x, height);
	}

	public void SetPosition(Vector2 position) {
		transform.position = position;
	}

	public void Hide() {
		transform.position = new Vector2 (Screen.width * 2, Screen.height * 2);
	}

	public float BodyHeight() {
		return rtBody.sizeDelta.y;
	}
}
