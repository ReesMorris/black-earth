using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	private MouseManager mouseManager;
	private GameObject player;

	void Start () {
		mouseManager = GameObject.Find ("GameManager").GetComponent<MouseManager> ();
		player = GameObject.Find ("Player").gameObject;
	}

	public void OnPointerEnter(PointerEventData dataName) {
		mouseManager.ChangeMouse (false, true);
	}

	public void OnPointerExit(PointerEventData dataName) {
		if (player.GetComponent<GunController> ().EquippedGun != null) {
			mouseManager.ChangeMouse (true, false);
		}
	}
}
