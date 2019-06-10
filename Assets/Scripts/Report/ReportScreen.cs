using UnityEngine;

public class ReportScreen : MonoBehaviour {

	public GameObject[] modals;

	private PauseManager pauseManager;
	private MouseManager mouseManager;
	private GameObject player;

	void Awake() {
		mouseManager = GameObject.Find ("GameManager").GetComponent<MouseManager> ();
		player = GameObject.Find ("Player");
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Escape)) { // we don't need any conditionals as this only works when report screen is enabled
			HideReportScreen ();
		}
	}

	public void ShowReportScreen() {
		ChangeModal (0);
		pauseManager = GameObject.Find ("GameManager").GetComponent<PauseManager> ();

		pauseManager.gameIsPaused = true;
		pauseManager.SetModalOpen (true);
		pauseManager.modalName = "report";

		gameObject.SetActive (true);
		mouseManager.ChangeMouse (false, true);
	}

	public void HideReportScreen() {
		pauseManager = GameObject.Find ("GameManager").GetComponent<PauseManager> ();

		pauseManager.gameIsPaused = false;
		pauseManager.SetModalOpen (false);
		gameObject.SetActive (false);

		if (player.GetComponent<GunController> ().EquippedGun != null) {
			mouseManager.ChangeMouse (true, false);
		}
	}

	public void ChangeModal(int id) {
		if (modals.Length > id) {
			for (int i = 0; i < modals.Length; i++) {
				modals [i].SetActive (false);
				if (i == id) {
					modals [i].SetActive (true);
				}
			}
		}
	}

	public void HideIcon(GameObject icon) {
		icon.SetActive (false);
	}
}
