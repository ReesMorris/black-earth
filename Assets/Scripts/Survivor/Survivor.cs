using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Survivor : MonoBehaviour {

	public int specialisationID = 0;
	public bool friendlyFireOn = false;

	private int _specialisationID = -1;
	private SpecialisationDatabase database;
	private ItemDatabase itemDatabase;
	private ReportManager reportManager;
	private SurvivorManager survivorManager;
	private WaveManager waveManager;
	private ScoutManager scoutManager;
	private ScoreManager scoreManager;

	private LivingEntity livingEntity;
	private Specialisation specialisation;
	public Specialisation Specialisation {
		get {
			return specialisation;
		}
	}

	[HideInInspector] public int scoutState = 0; // 0 = not scout, 1 = sending, 2 = scouting, 3 = returning
	private int daysScouting = 0;

	void Start() {
		GameObject.Find ("GameManager").GetComponent<ScoreManager> ().totalSurvivors++;

		livingEntity = GetComponent<LivingEntity> ();
		itemDatabase = GameObject.Find ("Databases").transform.Find ("ItemDatabase").GetComponent<ItemDatabase> ();
		database = GameObject.Find ("SpecialisationDatabase").GetComponent<SpecialisationDatabase> ();
		survivorManager = GameObject.Find ("GameManager").GetComponent<SurvivorManager> ();
		reportManager = GameObject.Find ("GameManager").GetComponent<ReportManager> ();
		waveManager = GameObject.Find ("GameManager").GetComponent<WaveManager> ();
		scoutManager = GameObject.Find ("GameManager").GetComponent<ScoutManager> ();
		scoreManager = GameObject.Find ("GameManager").GetComponent<ScoreManager> ();

		livingEntity.deathEvent += OnDeath;
		waveManager.waveStartEvent += OnWaveStart;
		waveManager.waveEndEvent += OnWaveEnd;
	}

	void Update() {
		// The specialisation ID has changed
		if (specialisationID != _specialisationID) {
			UpdateSpecialisation (specialisationID);
		}
	}

	void OnWaveStart() {
		if (scoutState == 1) { // sending
			reportManager.AddToReport (ReportManager.ReportTypes.ScoutSent);
			scoutState = 2;
			transform.localScale = Vector3.zero;
			gameObject.tag = "Untagged";
			scoutManager.ShowIcon ();
		}
	}

	void OnWaveEnd() {
		if (scoutState == 2) {
			daysScouting++;
			if (scoutManager.DoesScoutDie (daysScouting)) {
				scoutState = 0;
				scoutManager.ShowIcon ();
				reportManager.AddToReport (ReportManager.ReportTypes.ScoutDied);
				survivorManager.KillSurvivor (gameObject);
				database.specialisations [specialisationID].currentCount--;
				Destroy (gameObject);
			}
		} else if (scoutState == 3) { // returning
			scoutState = 0;
			UpdateSpecialisation(0);
			transform.localScale = Vector3.one;
			reportManager.AddToReport (ReportManager.ReportTypes.ScoutReturned);
			gameObject.tag = "Survivor";
			daysScouting = 0;

			scoutManager.GetScoutItems (daysScouting);
		}
	}

	public void UpdateSpecialisation(int index) {
		if(!database.specialisations[index].specialisationHasLimit || (database.specialisations[index].specialisationHasLimit && database.specialisations[index].specialisationLimit > database.specialisations[index].currentCount)) {
			specialisationID = index;
			database.specialisations [index].currentCount++;

			// Change the model
			for (int i = 0; i < database.specialisations.Count; i++) {
				transform.GetChild (i).gameObject.SetActive (false);
				if (transform.GetChild (i).gameObject.name == database.specialisations [index].model.name) {
					transform.GetChild (i).gameObject.SetActive (true);
				}
			}
			GetComponent<Unit> ().SetAnimator ();

			// Change the count on the specialisation
			if (_specialisationID != -1) {
				database.specialisations [_specialisationID].currentCount--;
			}

			_specialisationID = index;
			specialisation = database.specialisations [index];
		}
	}

	void OnDeath() {
		GetComponent<GunController> ().EquippedGun.destroyed = true;
		if (itemDatabase.items [GetComponent<GunController> ().EquippedGun.GetComponent<ItemObject> ().databaseID].itemName != "Pistol") {
			GetComponent<GunController> ().EquippedGun.GetComponent<ItemObject> ().DropItem ();
		}
		Rigidbody rb = GetComponent<Rigidbody> ();
		rb.freezeRotation = false;

		database.specialisations [specialisationID].currentCount--;

		scoreManager.deadSurvivors++;

		reportManager.AddToReport (ReportManager.ReportTypes.DeadSurvivor);

		survivorManager.KillSurvivor (gameObject);

		if (livingEntity.Dead) {
			StartCoroutine ("BodyDisappear");
		}
	}

	IEnumerator BodyDisappear() {
		yield return new WaitForSeconds (5f);
		gameObject.SetActive (false);
	}
}
