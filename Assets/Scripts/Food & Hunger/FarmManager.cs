using UnityEngine;
using UnityEngine.UI;

public class FarmManager : MonoBehaviour {

	public int startingFood = 3;
	public int foodPerRound = 1;
	public int farmerBonusPerRound = 1;
	public GameObject foodPanel;

	private int currentFood;
	public int CurrentFood {
		get {
			return currentFood;
		}
	}

	private WaveManager waveManager;
	private HungerManager hungerManager;
	private Specialisation farmerSpecialisation;
	private ReportManager reportManager;

	void Start() {
		IncreaseRations (startingFood);

		hungerManager = GetComponent<HungerManager> ();
		reportManager = GetComponent<ReportManager> ();
		farmerSpecialisation = GameObject.Find ("SpecialisationDatabase").GetComponent<SpecialisationDatabase> ().specialisations[1];

		waveManager = GetComponent<WaveManager> ();
		waveManager.waveEndEvent += OnWaveEnd;

		UpdateUI ();
	}

	public void IncreaseRations(int amount) {
		currentFood += amount;
		GetComponent<ScoreManager> ().totalFoodRations += amount;
	}

	public void DecreaseRations(int amount) {
		currentFood -= amount;

		if (currentFood < 0) {
			currentFood = 0;
		}

		UpdateUI ();
	}

	void OnWaveEnd() {
		IncreaseRations (foodPerRound);

		int bonusRations = farmerSpecialisation.currentCount * farmerBonusPerRound;

		if (bonusRations > 0) {
			IncreaseRations (bonusRations);
			for (int i = 0; i < bonusRations; i++) {
				reportManager.AddToReport (ReportManager.ReportTypes.FarmBonus);
			}
		}


		UpdateUI ();

		hungerManager.OnWaveEnd ();
	}

	void UpdateUI() {
		foodPanel.transform.Find("Text").GetComponent<Text>().text = currentFood.ToString();
		foodPanel.GetComponent<TopPanel> ().ResizeContainer ();
	}
}
