using UnityEngine;

public class GunController : MonoBehaviour {

	public Gun startingGun;
	public Transform weaponHold;

	private MouseManager mouseManager;
	private ItemDatabase database;
	private Gun equippedGun;
	public Gun EquippedGun {
		get {
			return equippedGun;
		}
	}

	private GunManager gunManager;

	void Start() {
		mouseManager = GameObject.Find ("GameManager").GetComponent<MouseManager> ();
		database = GameObject.Find ("Databases").transform.Find ("ItemDatabase").GetComponent<ItemDatabase> ();
		gunManager = GameObject.Find ("GameManager").GetComponent<GunManager> ();
		if(startingGun != null) {
			EquipGun (startingGun);
		}
	}

	public void EquipGun(Gun gunToEquip) {
		
		// Destroy the currently equipped gun
		if (equippedGun != null) {
			Destroy (equippedGun.gameObject);
		}

		if (gunToEquip != null) {
			// Create the new gun and set its parent
			equippedGun = Instantiate (gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
			equippedGun.transform.parent = weaponHold;
			if (gameObject.name == "Player") {
				mouseManager.ChangeCrosshair (database.items [equippedGun.GetComponent<ItemObject> ().databaseID].crosshair);
				mouseManager.ChangeMouse (true, false);
				gunManager.ammoUI.Display (true);
				gunManager.ammoUI.SetMaxAmmo (gunToEquip.clipSize);
				gunManager.ammoUI.SetCurrentAmmo (0);
			}
		} else {
			mouseManager.ChangeMouse (false, true);
			gunManager.ammoUI.Display (false);
		}
	}

	public void Shoot() {
		if(equippedGun != null) {
			equippedGun.Shoot ();
		}
	}

	public void AimGun(Vector3 position) {
		Vector3 look = new Vector3 (position.x, equippedGun.transform.position.y, position.z);
		equippedGun.transform.LookAt (look);
	}
}
