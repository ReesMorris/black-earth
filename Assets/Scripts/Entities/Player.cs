using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : MonoBehaviour {

	public float moveSpeed = 5f;

	private PlayerController controller;
	private Camera viewCamera;
	private GunController gunController;
	private PauseManager pauseManager;
	private List<GameObject> walls;
	private LivingEntity livingEntity;
	private GameOverManager gameOver;
	private GameModeManager gameModeManager;

	void Start() {
		controller = GetComponent<PlayerController>();
		gunController = GetComponent<GunController> ();
		livingEntity = GetComponent<LivingEntity> ();
		gameModeManager = GameObject.Find ("GameManager").GetComponent<GameModeManager> ();
		gameOver = GameObject.Find ("GameManager").GetComponent<GameOverManager> ();
		pauseManager = GameObject.Find ("GameManager").GetComponent<PauseManager> ();
		viewCamera = Camera.main;
		walls = new List<GameObject>();

		livingEntity.deathEvent += OnDeath;
	}

	void OnDeath() {
		if (gameModeManager.gameMode == "default") {
			gameOver.GameOver (true);
		} else {
			gameOver.GameOver (false);
		}
	}

	void Update() {
		MovementInput ();
		RotationInput ();
		WeaponInput ();
		ResetPosition ();
		CheckWalls ();
	}

	void MovementInput() {
		Vector3 moveInput = new Vector3 (Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
		Vector3 moveVelocity = moveInput.normalized * moveSpeed; // Normalizing will give us the direction of the input
		controller.Move (moveVelocity);
	}

	void RotationInput() {
		Ray ray = viewCamera.ScreenPointToRay (Input.mousePosition); // returns a ray from the camera to the mouse position
		Plane groundPlane = new Plane (Vector3.up, transform.position); // generate a plane programmatically, looking up at 0,0,0
		float rayDistance;

		if (groundPlane.Raycast (ray, out rayDistance)) { // takes a Ray and will give out a variable (rayDistance). Returns true if the ray intersects with the ground plane
			Vector3 point = ray.GetPoint(rayDistance);
			//Debug.DrawLine (ray.origin, point, Color.red);
			controller.LookAt(point);
		}
	}

	void WeaponInput() {
		// If LMB is being pressed down, shoot
		if (Input.GetMouseButton (0)) {
			if(!pauseManager.gameIsPaused) {
				gunController.Shoot ();
			}
		}
	}

	void ResetPosition() {
		if (transform.position.y < -10f) {
			transform.position = new Vector3 (0f, 3f, 0f);
		}
	}

	void CheckWalls() {
		foreach (GameObject wall in walls) {
			Material material = wall.GetComponent<Renderer> ().material;

			// Make it opaque [11]
			material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
			material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
			material.SetInt("_ZWrite", 1);
			material.DisableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = -1;

			// Change the colour
			wall.GetComponent<Renderer> ().material.color = new Color (material.color.r, material.color.g, material.color.b, 1f);
		}
		walls = new List<GameObject> ();

		Vector3 myPosition = Camera.main.transform.position;
		Vector3 rayDirection = Camera.main.transform.forward;    
		int rayLengthMeters = 100;

		// [10]
		RaycastHit[] hitInfo = Physics.RaycastAll (myPosition, rayDirection, rayLengthMeters, LayerMask.GetMask ("Unwalkable"));
		for (int i = 0; i < hitInfo.Length; i++) {
			hitInfo[i].transform.gameObject.layer = 9;
			walls.Add (hitInfo[i].transform.gameObject);
		}

		foreach (GameObject wall in walls) {
			Material material = wall.GetComponent<Renderer> ().material;

			// Make it transparent [11]
			material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
			material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			material.SetInt("_ZWrite", 0);
			material.DisableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = 3000;

			// Change the colour
			wall.GetComponent<Renderer> ().material.color = new Color (material.color.r, material.color.g, material.color.b, 0f);
		}
	}
}
