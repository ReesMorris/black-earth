using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	public GameObject[] modals;

	void Start() {
		Time.timeScale = 1f;
	}

	public void LoadScene(int index) {
		Time.timeScale = 1f;
		PlayerPrefs.SetString ("gameMode", "default");
		SceneManager.LoadScene(index);
	}

	public void ExitToDesktop() {
		Application.Quit ();
	}

	public void ShowModal(int id) {
		for (int i = 0; i < modals.Length; i++) {
			modals [i].SetActive (false);
			if (i == id) {
				modals [i].SetActive (true);
			}
		}
	}

	public void ToggleModal(int id) {
		for (int i = 0; i < modals.Length; i++) {
			if (i == id) {
				modals [i].SetActive (!modals [i].activeSelf);
			} else {
				modals [i].SetActive (false);
			}
		}
	}
}
