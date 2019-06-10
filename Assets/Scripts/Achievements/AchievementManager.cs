using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AchievementManager : MonoBehaviour {

	public GameObject achievementPrefab;
	public GameObject achievementPanel;

	private AchievementDatabase database;
	private GameModeManager gameModeManager;

	void Start() {
		database = GameObject.Find ("Databases").transform.Find ("AchievementDatabase").GetComponent<AchievementDatabase> ();
		gameModeManager = GetComponent<GameModeManager> ();
	}

	public void AwardAchievement(int achievementId) {
		if (database.achievements.Count > achievementId && gameModeManager.gameMode == "default") {
			Achievement achievement = database.achievements [achievementId];
			if (!achievement.awarded) {
				GameObject go = Instantiate (achievementPrefab);

				go.transform.SetParent (achievementPanel.transform, false);
				go.transform.Find ("Image").GetComponent<Image> ().sprite = achievement.image;
				go.transform.Find ("Description").GetComponent<Text> ().text = achievement.description;

				achievement.awarded = true;

				StartCoroutine ("FadeOut", go);
			}
		}
	}

	IEnumerator FadeOut(GameObject achievement) {
		yield return new WaitForSeconds (3f);
		Image image = achievement.transform.Find ("Image").GetComponent<Image> ();
		Text title = achievement.transform.Find ("Title").GetComponent<Text> ();
		Text text = achievement.transform.Find ("Description").GetComponent<Text> ();
		Image background = achievement.GetComponent<Image> ();

		float a = background.color.a;
		while (background.color.a >= 0) {

			a -= 0.1f;
			image.color = new Color (image.color.r, image.color.g, image.color.b, a);
			text.color = new Color (text.color.r, text.color.g, text.color.b, a);
			title.color = new Color (title.color.r, title.color.g, title.color.b, a);
			background.color = new Color (background.color.r, background.color.g, background.color.b, a);
			yield return new WaitForSeconds (0.1f);

			if (a <= 0) {
				Destroy (gameObject);
			}
		}
	}
}
