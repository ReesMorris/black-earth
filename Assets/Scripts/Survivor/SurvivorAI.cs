using UnityEngine;

public class SurvivorAI : MonoBehaviour {

	//private Unit unit;
	private SpecialisationDatabase database;
	private Survivor survivor;
	private GameObject target;
	private LivingEntity livingEntity;
	private GunController gunController;
	private WaveManager waveManager;
	private Unit unit;
	private bool retreating;

	void Start() {
		livingEntity = GetComponent<LivingEntity> ();
		gunController = GetComponent<GunController> ();
		unit = GetComponent<Unit> ();
		database = GameObject.Find ("SpecialisationDatabase").GetComponent<SpecialisationDatabase> ();
		survivor = GetComponent<Survivor> ();
		waveManager = GameObject.Find ("GameManager").GetComponent<WaveManager> ();
		waveManager.waveEndEvent += OnWaveEnd;
	}

	void Update() {
		if (!livingEntity.Dead) {
			Specialisations ();
		} else {
			unit.target = null;
		}
	}

	// Core functionality to make specialisations work
	void Specialisations() {
		GoToZone ();
		SeekNearbyInterests ();
	}

	// Makes an AI stay / go to their work zone
	void GoToZone() {
		// Make sure a zone is set
		if (database.specialisations[survivor.specialisationID].specialisationZone != null) {
			RaycastHit hit;

			if (Physics.Raycast(transform.position, -transform.up, out hit)) {
				if (hit.transform.gameObject != database.specialisations[survivor.specialisationID].specialisationZone) {
					unit.target = database.specialisations[survivor.specialisationID].specialisationZone;
					unit.moveTowardsTarget = true;
				} else {
					unit.target = null;
				}
			}
		}
	}

	// Checks to see if zombies are nearby, for self-defense
	void SeekNearbyInterests() {

		// Update the target
		if (target == null || target.tag != "Enemy" || target.tag != "DroppedItem") {
			// If the class can fight, see if any enemies are nearby
			if (database.specialisations[survivor.specialisationID].killVisibleEnemies || database.specialisations[survivor.specialisationID].moveToHiddenEnemies) {
				target = FindClosestInRange ("Enemy", database.specialisations[survivor.specialisationID].viewRange);
			}
			// If there are no enemies and the class can pickup items, find some
			if (target == null && database.specialisations[survivor.specialisationID].pickupNearbyItems) {
				target = FindClosestInRange ("DroppedItem", database.specialisations[survivor.specialisationID].viewRange);
			}
		}

		// Do we have enough HP or should we retreat?
		float retreatHP = (livingEntity.startingHealth / 100) * database.specialisations[survivor.specialisationID].retreatPercentage;
		if (livingEntity.Health < retreatHP) {

			// Time to retreat
			if (database.specialisations[survivor.specialisationID].retreatZone != null) {
				retreating = true;
				unit.target = database.specialisations[survivor.specialisationID].retreatZone;
				unit.moveTowardsTarget = true;
			} else {
				retreating = false;
				Debug.LogError (gameObject.name + " is trying to retreat, but " + database.specialisations[survivor.specialisationID].specialisationName + " has no retreat zone set!");
			}
		} else {
			retreating = false;
		}

		// If there is a target
		if (target != null && !retreating) {
			// If the target is an enemy
			if (target.transform.tag == "Enemy") {
				// Can we see the enemy?
				transform.LookAt(target.transform);

				bool canSee = true;
				RaycastHit hit;
				if (Physics.Raycast (transform.position, transform.forward, out hit, database.specialisations[survivor.specialisationID].viewRange, LayerMask.GetMask ("Unwalkable"))) {
					if(hit.distance < Vector3.Distance(transform.position, target.transform.position)) {
						canSee = false;
					}

					// We can't see the enemy
					if(!canSee && database.specialisations[survivor.specialisationID].moveToHiddenEnemies) {
						unit.target = target;
						unit.moveTowardsTarget = true;
					}

				}

				if (canSee) {

					// We can see the enemy
					if (database.specialisations[survivor.specialisationID].killVisibleEnemies || database.specialisations[survivor.specialisationID].moveToHiddenEnemies) {
						unit.moveTowardsTarget = false;
						if (!gunController.EquippedGun.Reloading) {
							gunController.AimGun (target.transform.position);
							gunController.Shoot ();
						}
					} else {
						unit.moveTowardsTarget = true;
					}
				}
			}

			// If the target is an item
			else if (target.transform.tag == "DroppedItem") {
				if (database.specialisations[survivor.specialisationID].pickupNearbyItems) {
					unit.target = target;
					unit.moveTowardsTarget = true;
				}
			}

			// Else, just move towards the destination
			else {
				unit.moveTowardsTarget = true;
			}
		}

	}

	// Will return the closest GameObject in range
	GameObject FindClosestInRange(string tag, float range) {
		GameObject[] targets = GameObject.FindGameObjectsWithTag (tag); // [3]

		if (targets.Length > 0) {
			GameObject closest = targets [0];
			float closestDist = Vector3.Distance(targets[0].transform.position, transform.position);

			foreach (GameObject go in targets) {
				float dist = Vector3.Distance(go.transform.position, transform.position);
				if (dist < closestDist) {
					closestDist = dist;
					closest = go;
				}
			}

			if (closestDist <= range) {
				return closest;
			}
		}
		return null;
	}

	void OnWaveEnd() {
		gunController.EquippedGun.Reload (gunController.EquippedGun.reloadTime);
	}

}
