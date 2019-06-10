using UnityEngine;
using UnityEngine.UI;

public class SurvivorName : MonoBehaviour {

	public string name;
	public GameObject survivorName;

	private SurvivorDatabase database;

	void Start() {
		database = GameObject.Find ("Databases").transform.Find("SurvivorDatabase").GetComponent<SurvivorDatabase>();

		GenerateRandomName ();
	}

	void Update () {
		survivorName.transform.LookAt (2 * transform.position - Camera.main.transform.position);
	}

	public void SetPlayerName(string _name) {
		survivorName.GetComponent<Text> ().text = _name;
		name = _name;
	}

	void GenerateRandomName() {
		if (database.survivors.Count > 0) {
			int random = Random.Range (0, database.survivors.Count - 1);
			SetPlayerName (database.survivors [random].survivorName);

			database.survivors.RemoveAt (random);
		} else {
			SetPlayerName ("");
			Debug.LogError ("SurvivorDatabase has run out of names!");
		}
	}
}
