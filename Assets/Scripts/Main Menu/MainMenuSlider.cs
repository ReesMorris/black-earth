using UnityEngine;
using UnityEngine.UI;

public class MainMenuSlider : MonoBehaviour {

	// [9]

	public string prefName;

	private Slider slider;

	public void Start()
	{
		slider = GetComponent<Slider> ();
		slider.value = PlayerPrefs.GetFloat (prefName, 1);
		slider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
	}

	public void ValueChangeCheck()
	{
		PlayerPrefs.SetFloat (prefName, slider.value);
	}

	void Update() {
		if (PlayerPrefs.GetFloat (prefName) != slider.value) {
			slider.value = PlayerPrefs.GetFloat (prefName);
		}
	}
}
