using UnityEngine;
using UnityEngine.UI;

public class MainMenuAchievements : MonoBehaviour {

	public int achievementsPerRow = 2;
	public float unobtainedOpacity = 0.85f;
	public GameObject achievementPrefab;
	public GameObject achievementRowPrefab;

	private AchievementDatabase database;

	void Start() {
		database = GameObject.Find ("Databases").transform.Find ("AchievementDatabase").GetComponent<AchievementDatabase> ();
		float totalRows = Mathf.Ceil((database.achievements.Count * 1f) / (achievementsPerRow * 1f));

		int currentIndex = 0;
		for (int i = 0; i < totalRows; i++) {
			GameObject panel = Instantiate (achievementRowPrefab) as GameObject;
			panel.transform.SetParent (transform, false);

			for (int j = 0; j < achievementsPerRow; j++) {
				if (database.achievements.Count > currentIndex) {
					GameObject achievement = Instantiate (achievementPrefab) as GameObject;
					achievement.transform.SetParent (panel.transform, false);

					Text title = achievement.transform.Find ("Title").GetComponent<Text> ();
					Text description = achievement.transform.Find ("Description").GetComponent<Text> ();
					Image image = achievement.transform.Find ("Image").GetComponent<Image> ();
					Image background = achievement.GetComponent<Image> ();

					title.text = database.achievements [currentIndex].title;
					description.text = database.achievements [currentIndex].description;
					image.sprite = database.achievements [currentIndex].image;

					if (!database.achievements [currentIndex].awarded) {
						title.color = new Color (title.color.r, title.color.g, title.color.b, unobtainedOpacity);
						description.color = new Color (description.color.r, description.color.g, description.color.b, unobtainedOpacity);
						image.color = new Color (image.color.r, image.color.g, image.color.b, unobtainedOpacity);
						background.color = new Color (background.color.r, background.color.g, background.color.b, unobtainedOpacity);

					}

					currentIndex++;
				} else {
					if (currentIndex % achievementsPerRow != 0) {
						GameObject achievement = Instantiate (achievementPrefab) as GameObject;
						achievement.transform.SetParent (panel.transform, false);

						foreach (Transform child in achievement.transform)
						{
							Destroy (child.transform.gameObject);
						}

						Image background = achievement.GetComponent<Image> ();
						background.color = new Color (background.color.r, background.color.g, background.color.b, 0f);
					}
				}
			}
		}
	}
}
