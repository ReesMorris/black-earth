using UnityEngine;
using UnityEngine.UI;

public class PopUpText : MonoBehaviour {

	public GameObject soulsPrefab;
	public GameObject textPrefab;

	public enum Types {Souls, Default};
	private GameObject textObject;

	public void SetPopUpText(Vector3 position, string message, Types type, Color textColor, int textSize = 4) {
		if (type == Types.Default) {
			textObject = Instantiate (textPrefab) as GameObject;
		}
		if (type == Types.Souls) {
			textObject = Instantiate (soulsPrefab) as GameObject;
		}

		textObject.transform.position = position;
		textObject.GetComponent<TextMesh>().text = message;
		textObject.GetComponent<TextMesh>().color = textColor;
		textObject.GetComponent<TextMesh>().characterSize = textSize;
	}

}
