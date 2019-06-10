using UnityEngine;
using System.Collections;

public class GameModeManager : MonoBehaviour {

	public string gameMode;

	[Header("Colony")]
	public Transform colonySpawn;

	[Header("Devil's Advocate")]
	public int startingSurvivors = 3;
	public int[] startingItems;

	private ChanceNewSurvivor chanceNewSurvivor;
	private SurvivorManager survivorManager;
	private SpecialisationDatabase specialisationDatabase;
	private ItemDatabase itemDatabase;
	private WaveManager waveManager;
	private Inventory inventory;
	private GameObject player;
	private LivingEntity playerEntity;

	void Start() {
		player = GameObject.Find ("Player").gameObject;
		waveManager = GetComponent<WaveManager> ();
		chanceNewSurvivor = GetComponent<ChanceNewSurvivor> ();
		survivorManager = GetComponent<SurvivorManager> ();
		inventory = GetComponent<Inventory> ();
		specialisationDatabase = GameObject.Find ("SpecialisationDatabase").GetComponent<SpecialisationDatabase> ();
		itemDatabase = GameObject.Find ("Databases").transform.Find ("ItemDatabase").GetComponent<ItemDatabase> ();
		playerEntity = player.GetComponent<LivingEntity> ();

		gameMode = PlayerPrefs.GetString ("gameMode");

		if (gameMode == "colony") {
			chanceNewSurvivor.acceptingNewSurvivor = false;
			specialisationDatabase.specialisations [0].pickupNearbyItems = false;
			for (int i = 0; i < 15; i++) {
				Vector3 randomPos = new Vector3 (Random.Range (colonySpawn.position.x - 10f, colonySpawn.position.x + 10f), colonySpawn.position.y, Random.Range (colonySpawn.position.z - 10f, colonySpawn.position.z + 10f));
				survivorManager.SpawnSurvivor (randomPos);
			}
		} else if (gameMode == "devil") {
			waveManager.SetWave (666);
			for (int i = 0; i < startingSurvivors; i++) {
				Vector3 randomPos = new Vector3 (Random.Range (colonySpawn.position.x - 10f, colonySpawn.position.x + 10f), colonySpawn.position.y, Random.Range (colonySpawn.position.z - 10f, colonySpawn.position.z + 10f));
				survivorManager.SpawnSurvivor (randomPos);
			}
			inventory.startingItemIds = startingItems;
		} else if (gameMode == "staying-alive") {
			player.GetComponent<LivingEntity> ().startingHealth = 100;
			chanceNewSurvivor.acceptingNewSurvivor = false;
			StartCoroutine (ReduceHealth ());
		}
	}

	IEnumerator ReduceHealth() {
		while (true) {
			if (waveManager.WaveActive) {
				playerEntity.TakeHit (1);
			}
			yield return new WaitForSeconds (1f);
		}
	}
}
