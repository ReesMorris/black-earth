using UnityEngine;

public class DayNightCycle : MonoBehaviour {

	// This script assumes that midnight is -90' and midday is 90' on the X rot
	public GameObject sun;
	public float rotationSpeed;

	private WaveManager waveManager;
	private int totalEnemies = 0;
	private float incrementPerDeath = 0;
	private float maxRot;
	private float amountRotated;

	void Start() {
		waveManager = GameObject.Find("GameManager").GetComponent<WaveManager>();
		waveManager.waveStartEvent += OnWaveStart; // [2]
	}

	void Update() {
		if (amountRotated < maxRot) {
			amountRotated += rotationSpeed;
			sun.transform.Rotate (new Vector3 (rotationSpeed, 0f, 0f));
		}
	}

	void OnWaveStart() {
		// Reset the sun
		sun.transform.rotation = Quaternion.Euler (new Vector3(-90f, 90f, 0f));
		totalEnemies = waveManager.TotalToSpawn ();
		maxRot = 0;
		amountRotated = 0;

		// Calculate the total amount to increment each time
		incrementPerDeath = 180 / totalEnemies;
	}

	public void OnEnemyDeath() {
		maxRot += incrementPerDeath;
	}
}
