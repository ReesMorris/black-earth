using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour {

	[Header("Settings")]
	public int startingWave = 1;
	public int increasePerWave = 1;
	public float timeBetweenSpawns = 1f;

	[Header("GameObjects")]
	public GameObject spawners;
	public GameObject enemyPrefab;
	public GameObject startWaveButton;
	public GameObject viewReportButton;
	public GameObject reportScreen;
	public GameObject waveCount;

	private int currentWave;
	public int CurrentWave {
		get {
			return currentWave;
		}
	}

	private bool waveActive = false;
	public bool WaveActive {
		get {
			return waveActive;
		}
	}

	private int enemiesRemaining = 0;
	private DayNightCycle dayNightCycle;
	private ReportManager reportManager;
	private GameObject reportNew;
	public GameObject ReportNew {
		get {
			return reportNew;
		}
	}

	// Delegates for the waves
	public delegate void OnWaveStart();
	public OnWaveStart waveStartEvent;

	public delegate void OnWaveEnd();
	public OnWaveEnd waveEndEvent;

	public delegate void OnViewReport();
	public OnViewReport viewReportEvent;

	void Start() {
		currentWave = startingWave;
		dayNightCycle = GetComponent<DayNightCycle> ();
		reportManager = GetComponent<ReportManager> ();
		reportNew = viewReportButton.transform.Find ("Image").gameObject;
	}

	// Called when an enemy is killed
	public void EnemyKilled() {
		enemiesRemaining--;
		dayNightCycle.OnEnemyDeath ();

		if (enemiesRemaining <= 0) {
			EndWave ();
		}
	}

	// Calculates how many zombies ONE spawner should spawn
	public int SpawnerToSpawn() {
		int total = currentWave * increasePerWave;
		return total;
	}

	// Calculates how many zombies will spawn
	public int TotalToSpawn() {
		int total = SpawnerToSpawn () * TotalSpawners ();
		return total;
	}

	// Starts the current wave
	public void StartWave() {
		if (!waveActive) {
			if (waveStartEvent != null) {
				waveStartEvent ();
			}
			enemiesRemaining = TotalToSpawn();
			startWaveButton.SetActive(false);
			viewReportButton.SetActive(false);
			reportNew.SetActive (false);
			waveActive = true;
			SetWaveText ();

			// Prepare the next wave
			currentWave++;
		}
	}

	// Ends the current wave
	public void EndWave() {
		if (waveActive) {
			if (waveEndEvent != null) {
				waveEndEvent ();

			}
			startWaveButton.SetActive(true);
			viewReportButton.SetActive(true);

			waveActive = false;
		}
	}

	// Returns the amount of spawners that can spawn this wave
	int TotalSpawners() {
		int totalSpawners = 0;

		for (int i = 0; i < spawners.transform.childCount; i++) {
			if (spawners.transform.GetChild (i).GetComponent<Spawner> ().CanSpawn ()) {
				totalSpawners++;
			}
		}

		return totalSpawners;
	}

	void SetWaveText() {
		waveCount.transform.Find ("Text").GetComponent<Text>().text = "NIGHT " + currentWave;
		waveCount.GetComponent<TopPanel> ().ResizeContainer ();
	}

	public void ViewReport() {
		if (viewReportEvent != null) {
			viewReportEvent ();
		}
	}

	public void SetWave(int wave) {
		currentWave = wave;
	}
}
