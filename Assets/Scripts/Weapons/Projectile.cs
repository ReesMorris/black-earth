using UnityEngine;

public class Projectile : MonoBehaviour {

	public AudioSource[] SFX;
	public LayerMask collisionMask; // which layer(s) the projectile should collide with

	private float speed = 10f;
	private float damage = 1f;
	private int penetration = 1;
	private bool canDamage = true;
	private GameObject firedBy;

	private GunManager gunManager;
	private AchievementManager achievement;
	private int startingPenetration = 1;

	void Start() {
		gunManager = GameObject.Find("GameManager").GetComponent<GunManager>();
		achievement = GameObject.Find ("GameManager").GetComponent<AchievementManager> ();
	}

	public void SetSpeed(float newSpeed) {
		speed = newSpeed;
	}

	public void SetDamage(float newDamage) {
		damage = newDamage;
	}

	public void SetPenetration(int newPenetration) {
		penetration = newPenetration;
		startingPenetration = newPenetration;
	}

	public void SetFiredBy(GameObject newSurvivor) {
		firedBy = newSurvivor;
	}

	void Update() {
		transform.Translate (Vector3.forward * Time.deltaTime * speed);

		if (transform.position.y > 20f) {
			Destroy (gameObject);
		}
	}

	// Upon colliding with something
	void OnCollisionEnter(Collision collision) {
		// If colliding with an entity that has health, damage it
		LivingEntity collisionEntity = collision.gameObject.GetComponent<LivingEntity> ();
		if (collisionEntity != null || collision.gameObject.tag == "Enemy") {
			if (collisionEntity == null) {
				collisionEntity = transform.parent.parent.GetComponent<LivingEntity> ();
			}
			bool damageDealt = false;

			if (collision.gameObject.name == "Survivor") {
				if (collision.gameObject.GetComponent<Survivor> ().friendlyFireOn) {
					collisionEntity.TakeHit (damage);
					damageDealt = true;
				}
			} else {
				if (canDamage) {
					collisionEntity.lastHitBy = firedBy;
					collisionEntity.TakeHit (damage);
				}
				damageDealt = true;
				penetration--;
				if (startingPenetration - penetration <= 0 && startingPenetration == 5) {
					achievement.AwardAchievement (2);
				}

				if (penetration <= 0) {
					// Destroy the bullet 
					canDamage = false;
					Destroy (gameObject);
				}
			}
			if (!collisionEntity.Dead && damageDealt) {
				// Only spawns damage particles if the entity is still alive
				Instantiate (gunManager.enemyParticles, transform.position, Quaternion.identity);
			}
		} else {
			// Spawn some flash particles
			Instantiate(gunManager.defaultParticles, transform.position, Quaternion.identity);

			if (SFX.Length > 0) {
				int random = Random.Range (0, SFX.Length-1);

				AudioSource newAudio = Instantiate (SFX [random]);
				newAudio.transform.position = transform.position;
				newAudio.GetComponent<Sound> ().PlaySound ();

				// Destroy the bullet
				Destroy (gameObject);
			}
		}
	}
}
