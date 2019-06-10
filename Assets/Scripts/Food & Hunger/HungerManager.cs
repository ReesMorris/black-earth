using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HungerManager : MonoBehaviour {

	public int hungerIncrease = 1;
	public float hungerMultiplier = 1.5f;
	public int foodDecreaseAmount = 7;

	private ReportManager reportManager;

	[Header("GameObjects")]
	public GameObject hungerBar;

	private FarmManager farmManager;
	private Hunger playerHunger;

	void Start() {
		farmManager = GetComponent<FarmManager> ();
		playerHunger = GameObject.Find ("Player").GetComponent<Hunger> ();
		reportManager = GetComponent<ReportManager> ();

		GenerateHungerBar ();
	}

	// Called by farm manager
	public void OnWaveEnd() {
		FeedSurvivors ();
		GenerateHungerBar ();
	}

	void FeedSurvivors() {
		List<GameObject> survivors = GetUnfedSurvivors();

		// First, make them all hungry
		for (int i = 0; i < survivors.Count; i++) {
			Hunger hunger = survivors [i].GetComponent<Hunger> ();
			hunger.IncreaseHunger (CalculateHunger(hunger));
			hunger.FedToday = false;
		}

		// Now we find the most hungry and feed them
		while (farmManager.CurrentFood > 0) {
			int highestHunger = 0;
			int highestIndex = 0;

			for (int i = 0; i < survivors.Count; i++) {
				Hunger hunger = survivors [i].GetComponent<Hunger> ();
				if (hunger.Hungers > highestHunger) {
					highestHunger = hunger.Hungers;
					highestIndex = i;
				}
			}

			if (highestHunger > 0) {
				Hunger hunger = survivors [highestIndex].GetComponent<Hunger> ();
				hunger.DecreaseHunger (foodDecreaseAmount);
				hunger.FedToday = true;

				farmManager.DecreaseRations(1);
			} else {
				break;
			}
		}

		// If we have no food
		if (farmManager.CurrentFood <= 0) {
			reportManager.AddToReport (ReportManager.ReportTypes.FarmEmpty);
		}

		// Now we make a report of any who are unfed
		for (int i = 0; i < survivors.Count; i++) {
			Hunger hunger = survivors [i].GetComponent<Hunger> ();
			if (!hunger.FedToday) {
				if (hunger.Hungers >= 100) {
					reportManager.AddToReport (ReportManager.ReportTypes.StarvingSurvivor);
					survivors [i].GetComponent<LivingEntity> ().TakeHit (1);
				} else {
					reportManager.AddToReport (ReportManager.ReportTypes.UnfedSurvivor);
				}
			}
		}
	}

	int CalculateHunger(Hunger hunger) {
		int newHunger = (int) ((hunger.DaysSinceLastEaten * hungerMultiplier) + hungerIncrease);
		return newHunger;
	}

	List<GameObject> GetUnfedSurvivors() {
		List<GameObject> survivors = new List<GameObject>(GameObject.FindGameObjectsWithTag("Survivor"));

		for (int i = 0; i < survivors.Count; i++) {
			if (survivors [i].GetComponent<Hunger> () == null) {
				survivors.RemoveAt (i);
			}
		}

		return survivors;
	}

	void GenerateHungerBar() {
		Image bar = hungerBar.transform.Find ("Bar").GetComponent<Image> ();
		Text text = hungerBar.transform.Find ("Text").GetComponent<Text> ();

		text.text = (100 - playerHunger.Hungers).ToString();
		bar.fillAmount = ((100f - (playerHunger.Hungers * 1f)) / 100f);
	}
}
