using UnityEngine;
using UnityEngine.UI;

public class MouseManager : MonoBehaviour {

	public GameObject crosshair;
	public Sprite[] sprites;

	private Sprite currentCrosshair;

	void Start() {
		ChangeMouse(true, false);
		ChangeCrosshair (0);
	}

	public void ChangeCrosshair(int index) {
		if (index > sprites.Length) {
			index = 0;
		}
		crosshair.GetComponent<Image> ().sprite = sprites [index];
	}

	public void ChangeCrosshair(Sprite iconCrosshair) {
		crosshair.GetComponent<Image> ().sprite = iconCrosshair;
	}

	public void ChangeMouse(bool showCrosshair, bool showCursor) {
		// Crosshair visibility
		if (showCrosshair) {
			crosshair.SetActive (true);
		} else {
			crosshair.SetActive (false);
		}

		// Mouse visibility
		if (showCursor) {
			Cursor.visible = true;
		} else {
			Cursor.visible = false;
		}
	}

	void Update() {
		crosshair.transform.position = Input.mousePosition;
	}
}
