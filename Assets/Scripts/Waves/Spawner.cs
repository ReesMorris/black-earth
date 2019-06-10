using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	// Setting a value to 0 or lower means that it will not affect anything.
	public int activationWave = 0;
	public int deactivationWave = 0;

	private int toSpawn = 0;
	private WaveManager waveManager;

	void Start() {
		waveManager = GameObject.Find ("GameManager").GetComponent<WaveManager> ();
		waveManager.waveStartEvent += OnWaveStart;
	}

	// Will make sure that the spawner is able to spawn enemies
	public bool CanSpawn () {
		bool canSpawn = true;

		// Is activated?
		if (activationWave > 0) {
			if (waveManager.CurrentWave < activationWave) {
				canSpawn = false;
			}
		}

		// Is deactivated?
		if (deactivationWave > 0) {
			if (waveManager.CurrentWave >= deactivationWave) {
				canSpawn = false;
			}
		}

		return canSpawn;
	}

	void OnWaveStart() {
		// If the spawner can spawn, spawn
		if (CanSpawn ()) {
			toSpawn = waveManager.SpawnerToSpawn ();
			StartCoroutine ("SpawnWave");
		}
	}

	IEnumerator SpawnWave() {
		while (true) {

			// If there are no more to spawn
			if (toSpawn <= 0) {
				StopCoroutine ("SpawnWave");
				break;
			}

			// Spawn an enemy
			GameObject enemy = Instantiate (waveManager.enemyPrefab, transform.position, transform.rotation);
			enemy.name = waveManager.enemyPrefab.name;

			// Reduce the amount left to spawn
			toSpawn--;

			yield return new WaitForSeconds (1f);
		}
	}
}
