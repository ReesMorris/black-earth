using UnityEngine;

public class SurvivorEntity : MonoBehaviour {

	public int survivorHunger;

	private  Hunger hunger;

	void Start() {
		hunger = GetComponent<Hunger> ();
	}
}
