using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Gun : MonoBehaviour {

	[Header("Settings")]
	public int penetration = 1;
	public float reloadTime = 1f;
	public int damage = 1;
	public float msBetweenShots = 100f;
	public float muzzleVelocity = 35f;

	[Header("Clip & Damage")]
	public int clipSize = 5;
	public bool takesDamage = true;
	public float startingHealth = 10;


	[Header("GameObjects")]
	public AudioSource[] SFX;
	public AudioSource reloadSound;
	public AudioSource breakSound;
	public Transform[] muzzles;
	public Projectile projectile;

	[HideInInspector] public bool destroyed = false;
	private bool reloading = false;
	public bool Reloading {
		get {
			return reloading;
		}
	}

	private float nextshotTime;
	private GunManager gunManager;
	private ParticleSystem flashParticles;
	private InventorySelector inventorySelector;
	private InventoryUIManager inventoryUI;
	private Inventory inventory;

	private int currentAmmo; // used by AI only, not player

	void Start() {
		currentAmmo = clipSize;
		inventory = GameObject.Find ("GameManager").GetComponent<Inventory> ();
		gunManager = GameObject.Find("GameManager").GetComponent<GunManager>();
		inventoryUI = GameObject.Find("GameManager").GetComponent<InventoryUIManager>();
		flashParticles = Instantiate (gunManager.flashParticles);
		flashParticles.transform.parent = transform;
		foreach (Transform muzzle in muzzles) {
			flashParticles.transform.position = muzzle.position;
		}
		inventorySelector = GameObject.Find ("GameManager").GetComponent<InventorySelector> ();

		InventorySlotData data = inventorySelector.CurrentSlotObject.GetComponent<InventorySlotData> ();
		gunManager.ammoUI.SetCurrentAmmo (data.currentAmmo);
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.R)) {
			if (!reloading && !destroyed) {
				Reload (reloadTime);
			}
		}
	}

	void IncreaseAmmo(int amount) {
		if (gameObject.transform.parent.parent.name == "Player") {
			if (inventorySelector != null) {
				InventorySlotData data = inventorySelector.CurrentSlotObject.GetComponent<InventorySlotData> ();
				data.currentAmmo += amount;

				if (data.currentAmmo > clipSize) {
					data.currentAmmo = clipSize;
				}

				gunManager.ammoUI.SetCurrentAmmo (data.currentAmmo);
			}
		} else {
			currentAmmo += amount;
		}
	}
		
	void ReduceAmmo(int amount) {
		if (gameObject.transform.parent.parent.name == "Player") {
			if (inventorySelector != null) {
				InventorySlotData data = inventorySelector.CurrentSlotObject.GetComponent<InventorySlotData> ();
				data.currentAmmo -= amount;

				if (data.currentAmmo < 0) {
					data.currentAmmo = 0;
				}

				gunManager.ammoUI.SetCurrentAmmo (data.currentAmmo);
			}
		} else {
			currentAmmo -= amount;
		}
	}

	void ReduceHealth (int amount) {
		if (takesDamage) {
			if (inventorySelector != null) {
				InventorySlotData data = inventorySelector.CurrentSlotObject.GetComponent<InventorySlotData> ();

				data.itemHealth -= amount;
				data.SetItemHealth ();

				if (data.itemHealth <= 0) {
					destroyed = true;
					if (breakSound != null) {
						AudioSource sound = Instantiate (breakSound) as AudioSource;
						sound.transform.parent = GameObject.Find ("Player").transform;
						sound.transform.localPosition = Vector3.zero;
						sound.Play ();
					}
					inventory.RemoveAtIndex (inventorySelector.CurrentSlotObject.GetComponent<InventorySlot> ().slotID);
					data.Empty ();
					Destroy (gameObject);
				}
			}
		}
	}

	public void Reload(float reloadTime) {
		bool canReload = true;
		if (!reloading) {
			reloading = true;
			if (reloadSound != null) {
				reloadSound.Play ();
			}
			if (!destroyed) {
				StartCoroutine ("ReloadWeapon", reloadTime);
			}
		}
	}

	public void StopReload() {
		reloading = false;
		if (reloadSound != null) {
			reloadSound.Stop ();
		}
		StopCoroutine ("ReloadWeapon");
		gunManager.ammoUI.StopReloadText ();
	}

	public void Shoot() {
		int ammo;

		if (gameObject.transform.parent.parent.name == "Player") {
			InventorySlotData data = inventorySelector.CurrentSlotObject.GetComponent<InventorySlotData> ();
			ammo = data.currentAmmo;
		} else {
			ammo = currentAmmo;
		}

		if (ammo > 0) {
			if (Time.time > nextshotTime && CanShoot()) {
				if (reloading) {
					StopReload ();
				}
				nextshotTime = Time.time + msBetweenShots / 1000f;

				foreach (Transform muzzle in muzzles) {
					Projectile newProjectile = Instantiate (projectile, muzzle.position, muzzle.rotation) as Projectile;
					newProjectile.SetSpeed (muzzleVelocity);
					newProjectile.SetDamage (damage);
					newProjectile.SetPenetration (penetration);
					newProjectile.SetFiredBy (gameObject.transform.parent.transform.parent.gameObject);
				}

				if (gameObject.transform.parent.parent.name == "Player") {
					ReduceHealth (1);
				}
				ReduceAmmo (1);

				if (flashParticles != null) {
					flashParticles.Play ();
				}

				if (SFX.Length > 0) {
					int random = Random.Range (0, SFX.Length - 1);

					SFX [random].Play ();
				}
			}
		} else {
			Reload (reloadTime);
		}
	}

	// Returns true if the mouse is not over a UI with tag defined in GunManager
	bool CanShoot() {
		bool canShoot = true;

		if (gameObject.transform.parent.parent.name == "Player") {
			// [7]
			PointerEventData pointerData = new PointerEventData (EventSystem.current) {
				position = Input.mousePosition
			};

			List<RaycastResult> results = new List<RaycastResult> ();
			EventSystem.current.RaycastAll (pointerData, results);

			for (int i = 0; i < results.Count; i++) {
				for (int j = 0; j < gunManager.tags.Count; j++) {
					if (results [i].gameObject.tag == gunManager.tags [j]) {
						canShoot = false;
						break;
					}
				}
			}

			if (canShoot && !gunManager.shootIfDraggingItem) {
				if (inventoryUI.GetIndexAtMouse () != -1) {
					canShoot = false;
				}
			}
		}

		return canShoot;
	}

	IEnumerator ReloadWeapon(float reloadTime) {
		if (gameObject.transform.parent.parent.name != null) {
			if (gameObject.transform.parent.parent.name == "Player") {
				gunManager.ammoUI.StartReloadText ();
			}
		}
		yield return new WaitForSeconds(reloadTime);
		if (gameObject.transform.parent.parent.name == "Player") {
			gunManager.ammoUI.StopReloadText ();
		}
		IncreaseAmmo (clipSize);
		reloading = false;
	}

}
