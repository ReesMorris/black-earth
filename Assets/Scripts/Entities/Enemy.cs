using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float damage = 1f;
	public GameObject deathEffect;
	public int soulsValue;

	private WaveManager waveManager;
	private LivingEntity livingEntity;
	private Drops drops;
	private Unit unit;
	private ScoreManager scoreManager;
	private CurrencyManager currencyManager;
	private PopUpText popuptext;
	private bool attacking = false;
	private LivingEntity playerEntity;
	private GameModeManager gameModeManager;

	private float nextHitTime;

	void Start() {
		waveManager = GameObject.Find ("GameManager").GetComponent<WaveManager> ();
		scoreManager = GameObject.Find ("GameManager").GetComponent<ScoreManager> ();
		currencyManager = GameObject.Find ("GameManager").GetComponent<CurrencyManager> ();
		popuptext = GameObject.Find ("GameManager").GetComponent<PopUpText> ();
		playerEntity = GameObject.Find ("Player").GetComponent<LivingEntity> ();
		gameModeManager = GameObject.Find ("GameManager").GetComponent<GameModeManager> ();
		unit = GetComponent<Unit> ();
		drops = GetComponent<Drops>();

		livingEntity = GetComponent<LivingEntity> ();
		livingEntity.deathEvent += OnDeath;
	}

	void Update() {
		CheckTarget();
	}

	// Tries to make sure that the zombie always has a target
	void CheckTarget() {
		GameObject target = FindClosestSurvivor ();

		// As long as I am not dead, let's roll.
		if (!livingEntity.Dead && target != null) {
			unit.target = target;
		}
	}

	// When the enemy dies
	void OnDeath() {
		
		scoreManager.AddKill (); // add a kill to the score
		currencyManager.AddSouls (soulsValue); // add souls
		popuptext.SetPopUpText(transform.position, "+" + soulsValue, PopUpText.Types.Souls, new Color (255,255,255));

		waveManager.EnemyKilled();

		if (gameModeManager.gameMode == "staying-alive") {
			playerEntity.Heal (5, true);
		}

		Vector3 pos = new Vector3 (transform.position.x, transform.position.y + 2f, transform.position.z);
		GameObject particles = Instantiate (deathEffect, pos, Quaternion.identity) as GameObject;
		if (livingEntity.lastHitBy != null && livingEntity.lastHitBy.GetComponent<GunController>().EquippedGun != null) {
			particles.transform.rotation = livingEntity.lastHitBy.GetComponent<GunController> ().EquippedGun.transform.rotation;
		} else if(GameObject.Find("Player").GetComponent<GunController>().EquippedGun != null) {
			particles.transform.rotation = GameObject.Find("Player").GetComponent<GunController> ().EquippedGun.transform.rotation;
		}

		if (drops.IsHoldingItem ()) {
			GameObject weapon = Instantiate (drops.GetHeldItem (), transform.position, Quaternion.identity) as GameObject;
			weapon.GetComponent<ItemObject> ().DropItem ();
		}

		unit.target = null;
		Destroy (gameObject);
	}

	// Will return the closest, alive, GameObject with the Survivor tag.
	GameObject FindClosestSurvivor() {
		GameObject[] survivors = GameObject.FindGameObjectsWithTag ("Survivor");

		// If there are any survivors
		if (survivors.Length > 0) {
			float closestDistance = -1; // [1]
			int closestIndex = -1;

			// Loop through every survivor
			for (int i = 0; i < survivors.Length; i++) {
				float distance = Vector3.Distance (transform.position, survivors [i].transform.position);
				LivingEntity survivorEntity = survivors [i].GetComponent<LivingEntity> ();

				// If the survivor is closer than the last, and not dead
				if ((distance < closestDistance || closestDistance == -1) && !survivorEntity.Dead) {
					closestDistance = distance;
					closestIndex = i;
				}
			}

			// Return the closest survivor GameObject
			if (closestIndex != -1) {
				return survivors [closestIndex];
			}
		}

		// No survivors, return null
		return null;
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Survivor") {
			if (!attacking) {
				attacking = true;
				StartCoroutine ("DamageSurvivor", collision.gameObject);
			}
		}
	}

	void OnCollisionExit(Collision collision) {
		if (collision.gameObject.tag == "Survivor") {
			attacking = false;
			StopCoroutine ("DamageSurvivor");
		}
	}

	IEnumerator DamageSurvivor(GameObject survivor) {
		LivingEntity survivorEntity = survivor.GetComponent<LivingEntity> ();
		while (!survivorEntity.Dead) {
			if (Time.time > nextHitTime) {
				nextHitTime = Time.time + 0.6f;
				survivorEntity.TakeHit (damage);
				popuptext.SetPopUpText (survivor.transform.position, "-" + damage, PopUpText.Types.Default, new Color (255, 0, 0), 2);
			}
			yield return new WaitForSeconds (0.5f);
		}
	}
}
