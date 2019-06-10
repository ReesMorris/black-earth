using UnityEngine;

public class ChanceNewSurvivor : MonoBehaviour {
	
	public Vector3 spawnPoint;
	public bool acceptingNewSurvivor = true;

	[Header("Single Survivor Chance")]
	public int startingChance = 0;
	public int chanceIncrease = 5;
	public int maxChance = 60;

	private int chanceOfNewSurvivor;
	private WaveManager waveManager;
	private SurvivorManager survivorManager;
	private ReportManager reportManager;

	void Start() {
		chanceOfNewSurvivor = startingChance;
		survivorManager = GetComponent<SurvivorManager> ();
		reportManager = GetComponent < ReportManager> ();

		waveManager = GetComponent<WaveManager> ();

		waveManager.waveEndEvent += OnWaveEnd;
	}

	void OnWaveEnd() {
		if (acceptingNewSurvivor) {
			if (NewSurvivor ()) {
				survivorManager.SpawnSurvivor (spawnPoint);
				reportManager.AddToReport (ReportManager.ReportTypes.NewSurvivor);
			}
		}
	}

	bool NewSurvivor() {
		int chance = Random.Range (0, 100);

		if (chanceOfNewSurvivor >= chance) {
			chanceOfNewSurvivor = 0;
			return true;
		} else {
			chanceOfNewSurvivor += chanceIncrease;
			if (chanceOfNewSurvivor > maxChance) {
				chanceOfNewSurvivor = maxChance;
			}
			return false;
		}
	}
}
