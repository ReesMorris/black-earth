using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public GameObject killCount;

	private int kills = 0;
	public int Kills {
		get {
			return kills;
		}
	}

	[HideInInspector] public int totalFoodRations = 0;
	[HideInInspector] public int totalSurvivors = 0;
	[HideInInspector] public int deadSurvivors = 0;

	private TopPanel panel;

	void Start() {
		kills = 0;
		panel = killCount.transform.parent.GetComponent<TopPanel> ();
		SetTextKill ();
	}

	public void AddKill(){
		kills++;
		SetTextKill ();
	}

	public void SetTextKill() {
		killCount.GetComponent<Text> ().text = kills.ToString (); 
		panel.ResizeContainer ();
	}

}
