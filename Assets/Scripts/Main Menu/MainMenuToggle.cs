using UnityEngine;
using UnityEngine.UI;

public class MainMenuToggle : MonoBehaviour {

	public string prefName;

	private Toggle toggle;

	void Start () {
		toggle = GetComponent<Toggle> ();
		toggle.isOn = PlayerPrefs.GetInt (prefName) != 0;

		toggle.onValueChanged.AddListener (ToggleChanged);
	}

	void ToggleChanged(bool value) {
		PlayerPrefs.SetInt(prefName, (value ? 1 : 0));
	}

	void Update() {
		bool active = PlayerPrefs.GetInt (prefName) != 0;
		if (toggle.isOn != active) {
			toggle.isOn = active;
		}
	}
}
