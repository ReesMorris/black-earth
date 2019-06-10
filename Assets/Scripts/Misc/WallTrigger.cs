using UnityEngine;

public class WallTrigger : MonoBehaviour {

	public GameObject[] walls;

	void Start() {
		GetComponent<MeshRenderer> ().enabled = false;
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.name == "PlayerLeg") {
			foreach (GameObject wall in walls) {
				wall.SetActive (false);
			}
		}
	}

	void OnTriggerExit(Collider collider) {
		if (collider.name == "PlayerLeg") {
			foreach (GameObject wall in walls) {
				wall.SetActive (true);
			}
		}
	}
}
