using UnityEngine;
using System.Collections.Generic;

public class LivingEntity : MonoBehaviour {

	public float startingHealth;

	private float health;
	public float Health {
		get {
			return health;
		}
	}

	private bool dead;
	public bool Dead {
		get {
			return dead;
		}
	}

	void Start() {
		health = startingHealth;
		popUpText = GameObject.Find ("GameManager").GetComponent<PopUpText> ();
	}

	[HideInInspector] public GameObject lastHitBy;
	private PopUpText popUpText;

	// Adding an event called on death
	public delegate void OnDeath();
	public OnDeath deathEvent;

	public void Heal(float amount, bool startingHealthIsMax) {
		health += amount;

		// Prevents the player from over-healing by capping the health
		if (startingHealthIsMax) {
			if (health >= startingHealth) {
				health = startingHealth;
			} else {
				popUpText.SetPopUpText (transform.position, "+" + amount, PopUpText.Types.Default, new Color (0, 255, 0), 4);
			}
		}
	}

	// Reduces the entity's health
	public void TakeHit(float damage) {
		health -= damage;

		if (health <= 0) {
			health = 0;
			if (!dead) {
				Die ();
			}
		}
	}

	// Called on death
	public void Die() {
		dead = true;

		// Calls the event to be used by other scripts
		if (deathEvent != null) {
			deathEvent ();
		}
	}
}
