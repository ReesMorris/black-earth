using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ReportManager : MonoBehaviour {

	public Text finalReportText;
	public enum ReportTypes{NewSurvivor, UnfedSurvivor, StarvingSurvivor, DeadSurvivor, XPPoint, ShopItem, ShopSale, ScoutSent, ScoutReturned, ScoutDied, FarmEmpty, FarmBonus};

	private WaveManager waveManager;
	private List<ReportTypes> reports = new List<ReportTypes> ();
	private string finalReport = "";

	void Start() {
		waveManager = GameObject.Find ("GameManager").GetComponent<WaveManager> ();
		waveManager.viewReportEvent += CompileReport; // [2]
		waveManager.waveStartEvent += EmptyReport; // [2]
	}

	public void AddToReport(ReportTypes type) {
		reports.Add (type);

		waveManager.ReportNew.SetActive (true);
	}

	public bool reportExists() {
		if (reports.Count > 0) {
			return true;
		}
		return false;
	}

	string GenerateReport(int count, string soloString, string pluralString) {
		string report = "";
		if (count > 0) {
			if (count == 1) {
				report += ("- " + soloString + "\n");
			} else {
				report += ("- " + pluralString + "\n");
			}
		}
		return report;
	}

	// Called when the wave starts
	void EmptyReport() {
		reports = new List<ReportTypes> ();
		finalReport = "";
	}

	// Called when the wave ends
	void CompileReport() {
		if(finalReport == "") {
			finalReport += SurvivorReport ();
			finalReport += SpecialisationReport ();
			finalReport += ShopReport ();

			if (finalReport == "") {
				finalReport = "- Nothing interesting happened last night";
			}

			finalReportText.text = finalReport;
		}
	}



	// Returns a string for the SURVIVORS report
	string SurvivorReport() {
		bool reportEmpty = true;
		string report = "";
		int newSurvivors = 0;
		int unfedSurvivors = 0;
		int deadSurvivors = 0;
		int scoutSent = 0;
		int scoutReturned = 0;
		int scoutDied = 0;
		int starvingSurvivors = 0;

		for (int i = 0; i < reports.Count; i++) {
			switch (reports [i]) {
			case ReportTypes.NewSurvivor:
				newSurvivors++;
				reportEmpty = false;
				break;
			case ReportTypes.StarvingSurvivor:
				starvingSurvivors++;
				reportEmpty = false;
				break;
			case ReportTypes.UnfedSurvivor:
				unfedSurvivors++;
				reportEmpty = false;
				break;
			case ReportTypes.ScoutSent:
				scoutSent++;
				reportEmpty = false;
				break;
			case ReportTypes.ScoutReturned:
				scoutReturned++;
				reportEmpty = false;
				break;
			case ReportTypes.ScoutDied:
				scoutDied++;
				reportEmpty = false;
				break;
			}
		}

		if (!reportEmpty) {
			report += "SURVIVORS\n";
			report += GenerateReport (newSurvivors, "A new survivor joined your base last night", newSurvivors + " new survivors joined your base last night");
			report += GenerateReport (starvingSurvivors, "1 survivor is starving to death", starvingSurvivors + " survivors are starving to death");
			report += GenerateReport (unfedSurvivors, "1 survivor could not be fed last night", unfedSurvivors + " survivors could not be fed last night");
			report += GenerateReport (deadSurvivors, "1 survivor died last night", deadSurvivors + " survivors died last night");
			report += GenerateReport (scoutSent, "1 survivor left the base to scout last night", scoutSent + " survivors left the base to scout last night");
			report += GenerateReport (scoutReturned, "1 survivor has returned from scouting", scoutReturned + " survivors have returned from scouting");
			report += GenerateReport (scoutDied, "1 scout was killed whilst scavenging last night", scoutDied + " scouts were killed whilst scavenging last night");
			report += "\n";
		}

		return report;
	}



	// Returns a string for the SPECIALISATION report
	string SpecialisationReport() {
		bool reportEmpty = true;
		string report = "";
		int xpPoints = 0;
		int farmBonus = 0;
		int farmEmpty = 0;

		for (int i = 0; i < reports.Count; i++) {
			switch (reports [i]) {
			case ReportTypes.XPPoint:
				xpPoints++;
				reportEmpty = false;
				break;
			case ReportTypes.FarmBonus:
				farmBonus++;
				reportEmpty = false;
				break;
			case ReportTypes.FarmEmpty:
				farmEmpty++;
				reportEmpty = false;
				break;
			}
		}

		if (!reportEmpty) {
			report += "SPECIALISATIONS\n";
			report += GenerateReport (farmBonus, "Farmers have grown +1 extra food", "Farmers have grown +" + farmBonus + " extra food");
			report += GenerateReport (farmEmpty, "Your farm has run out of food!", "");
			report += GenerateReport (xpPoints, "Researchers have earned +1 Research Point", "Researchers have earned +" + xpPoints + " Research Points");
			report += "\n";
		}

		return report;
	}



	// Returns a string for the SHOP report
	string ShopReport() {
		bool reportEmpty = true;
		string report = "";
		int shopItems = 0;
		int shopSale = 0;

		for (int i = 0; i < reports.Count; i++) {
			switch (reports [i]) {
			case ReportTypes.ShopItem:
				shopItems++;
				reportEmpty = false;
				break;
			case ReportTypes.ShopSale:
				shopSale++;
				reportEmpty = false;
				break;
			}
		}

		if (!reportEmpty) {
			report += "SHOP\n";
			report += GenerateReport (shopItems, "A new item is now available in the shop", shopItems + " new items are now available in the shop");
			report += GenerateReport (shopSale, "An item is on sale in the shop", "");
			report += "\n";
		}

		return report;
	}
}
