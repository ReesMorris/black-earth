using UnityEngine;

public class Hunger : MonoBehaviour {

	public int startingHunger;

	private int hunger;
	public int Hungers {
		get {
			return hunger;
		}
	}

	private int daysSinceLastEaten = 0;
	public int DaysSinceLastEaten {
		get {
			return daysSinceLastEaten;
		}
	}

	private bool fedToday = false;
	public bool FedToday {
		get {
			return fedToday;
		}
		set {
			fedToday = value;
		}
	}

	void Start() {
		IncreaseHunger (startingHunger);	
	}

	public void IncreaseHunger(int amount) {
		hunger += amount;
		if (hunger >= 100) {
			hunger = 100;
		}
	}

	public void DecreaseHunger(int amount) {
		hunger -= amount;
		daysSinceLastEaten = 0;
		if (hunger <= 0) {
			hunger = 0;
		}
	}
}
