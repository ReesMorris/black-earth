using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SurvivorManager : MonoBehaviour {

	public List<GameObject> survivors;

	[Header("GameObjects")]
	public GameObject survivorPrefab;
	public GameObject noSurvivors;
	public GameObject survivorNumbers;
	public GameObject container;
	public GameObject survivorPanel;
	public GameObject newObject;
	public GameObject survivorCount;
	public GameObject topPanel;

	private AchievementManager achievement;
	private WaveManager waveManager;
	private SpecialisationDatabase specialisationDatabase;
	private GameOverManager gameOverManager;
	private GameModeManager gameModeManager;

	void Start() {
		specialisationDatabase = GameObject.Find ("SpecialisationDatabase").GetComponent<SpecialisationDatabase> ();
		achievement = GameObject.Find ("GameManager").GetComponent<AchievementManager> ();
		gameOverManager = GetComponent<GameOverManager> ();
		gameModeManager = GetComponent<GameModeManager> ();

		waveManager = GetComponent<WaveManager> ();
		waveManager.waveStartEvent += OnWaveStart;
		waveManager.viewReportEvent += OnWaveEnd;

		GenerateList ();
		ChangeSurvivorCount ();
	}

	void Update() {
		if (survivors.Count >= 5) {
			achievement.AwardAchievement (3);
		}
	}

	void ChangeSurvivorCount() {
		topPanel.transform.GetComponentInChildren<Text>().text = survivors.Count.ToString();
		topPanel.GetComponent<TopPanel> ().ResizeContainer ();
	}

	void OnWaveStart() {
		newObject.SetActive (false);
	}

	void OnWaveEnd() {
		GenerateList ();
	}

	public void SpawnSurvivor(Vector3 position) {
		GameObject survivor = Instantiate (survivorPrefab) as GameObject;
		survivor.name = survivorPrefab.name;
		survivor.transform.position = position;

		survivors.Add (survivor);
		newObject.SetActive (true);

		ChangeSurvivorCount ();
	}

	public void KillSurvivor(GameObject survivor) {
		survivors.Remove (survivor);

		if (survivors.Count == 0 && gameModeManager.gameMode == "colony") {
			gameOverManager.GameOver (false);
		}

		ChangeSurvivorCount ();
	}

	public void GenerateList() {
		// Update the UI
		ChangeSurvivors ();
		UpdateSurvivorNumbers();

		// Make everything hidden
		noSurvivors.SetActive(false);
		survivorNumbers.SetActive (false);

		// Destroy all panels
		foreach (Transform child in container.transform) {
			if (child.name == "Survivor Panel") {
				Destroy (child.gameObject);
			}
		}

		if (survivors.Count == 0) {
			noSurvivors.SetActive (true);
		} else {
			survivorNumbers.SetActive (true);

			for (int i = 0; i < survivors.Count; i++) {
				// Instantiate the panel
				GameObject panel = Instantiate (survivorPanel) as GameObject;
				panel.name = survivorPanel.name;
				panel.transform.SetParent (container.transform, false);

				// Change the values
				panel.transform.Find("Name").GetComponent<Text>().text = survivors[i].GetComponent<SurvivorName>().name;
				panel.transform.Find ("Occupation").GetComponent<Text> ().text = specialisationDatabase.specialisations [survivors[i].GetComponent<Survivor>().specialisationID].specialisationName.ToString();

				// Update the icons for their role
				UpdatePanelRoleImages(panel, i);
			}
		}
	}

	void ChangeSurvivors() {
		RectTransform countRT = survivorCount.GetComponent<RectTransform> ();
		RectTransform textRT = survivorCount.transform.Find ("Text").GetComponent<RectTransform> ();

		float width = 100f;
		width += LayoutUtility.GetPreferredWidth (textRT); // [5]

		countRT.sizeDelta = new Vector2(width, countRT.sizeDelta.y);
	}

	public void UpdateSurvivorNumbers() {
		for (int i = 0; i < survivorNumbers.transform.childCount; i++) {
			string text = "";
			text += specialisationDatabase.specialisations [i].currentCount.ToString();
			if (specialisationDatabase.specialisations [i].specialisationHasLimit) {
				text += "/" + specialisationDatabase.specialisations [i].specialisationLimit;
			}

			survivorNumbers.transform.GetChild (i).GetComponent<Text> ().text = text;
		}
	}

	void UpdatePanelRoleImages(GameObject panel, int survivorIndex) {
		Transform roles = panel.transform.Find ("Roles");

		for (int i = 0; i < roles.childCount; i++) {
			GameObject child = roles.GetChild (i).gameObject;
			child.GetComponent<SurvivorButton> ().survivor = survivors [survivorIndex];

			if (child.name != "Scout") {
				if (i == survivors [survivorIndex].GetComponent<Survivor> ().specialisationID) {
					Color color = child.GetComponent<Image> ().color;
					child.GetComponent<Image> ().color = new Color (color.r, color.g, color.b, 1f);
					child.transform.Find ("Image").GetComponent<Image> ().color = new Color (255f, 255f, 255f, 1f);
					child.GetComponent<SurvivorButton> ().active = true;
				} else {
					Color color = child.GetComponent<Image> ().color;
					child.GetComponent<Image> ().color = new Color (color.r, color.g, color.b, 0.5f);
					child.transform.Find ("Image").GetComponent<Image> ().color = new Color (255f, 255f, 255f, 0.5f);
				}
			} else {
				switch (survivors [survivorIndex].GetComponent<Survivor> ().scoutState) {
				case 0:
					child.transform.Find ("Text").GetComponent<Text> ().text = "Send scouting";
					break;
				case 1:
					child.transform.Find ("Text").GetComponent<Text> ().text = "Preparing...";
					break;
				case 2:
					child.transform.Find ("Text").GetComponent<Text> ().text = "Call home";
					break;
				case 3:
					child.transform.Find ("Text").GetComponent<Text> ().text = "Returning tomorrow";
					break;
				}
			}
		}
	}
}
