using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {

	public GameObject pauseScreen;
	public GameObject optionsScreen;

	[HideInInspector] public bool modalIsOpen;
	[HideInInspector] public string modalName;
	[HideInInspector] public bool gameIsPaused;

	private CameraManager cameraManager;
	private MouseManager mouseManager;
	private float nextOpenTime;

	void Start() {
		cameraManager = GetComponent<CameraManager> ();
		mouseManager = GetComponent<MouseManager> ();

		pauseScreen.SetActive(false);
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (!modalIsOpen && Time.time > nextOpenTime) {
				if (!gameIsPaused) {
					ShowPauseMenu ();
				} else {
					HidePauseMenu ();
				}
			}
		}
	}

	public void SetModalOpen(bool isModalOpen) {
		modalIsOpen = isModalOpen;
		if (!isModalOpen) {
			nextOpenTime = Time.time + 0.1f; // prevents it opening when you press ESC to close a modal
		}
	}

	public void ShowPauseMenu() {
		Time.timeScale = 0f;
		gameIsPaused = true;
		mouseManager.ChangeMouse (false, true);
		cameraManager.EnableBlur (true);
		cameraManager.SetBlurValues (10, 0.51f);
		pauseScreen.SetActive (true);
	}

	public void HidePauseMenu() {
		Time.timeScale = 1f;
		gameIsPaused = false;
		mouseManager.ChangeMouse (true, false);
		cameraManager.EnableBlur (false);
		pauseScreen.SetActive (false);
		optionsScreen.SetActive (false);
	}

	public void LoadScene(int index) {
		Time.timeScale = 1f;
		SceneManager.LoadScene(index);
	}

	public void ExitToDesktop() {
		Application.Quit ();
	}

	public void ShowOptionsMenu() {
		pauseScreen.SetActive (false);
		optionsScreen.SetActive (true);
	}

	public void HideOptionsMenu() {
		pauseScreen.SetActive (true);
		optionsScreen.SetActive (false);
	}
}
