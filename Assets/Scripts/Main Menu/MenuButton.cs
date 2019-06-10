using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public string modeName;

	Vector3 normalScale;
	Vector3 hoverScale;

	void Start() {
		normalScale = transform.localScale;
		hoverScale = new Vector3 (transform.localScale.x + 0.1f, transform.localScale.y + 0.1f, transform.localScale.z);
	}

	public void OnPointerEnter(PointerEventData eventData) {
		Resize (hoverScale);
	}

	public void OnPointerExit(PointerEventData eventData) {
		Resize (normalScale);
	}

	void Resize(Vector3 size) {
		transform.localScale = size;
	}

	public void LoadGame() {
		PlayerPrefs.SetString ("gameMode", modeName);
		SceneManager.LoadScene (1);
	}
}
