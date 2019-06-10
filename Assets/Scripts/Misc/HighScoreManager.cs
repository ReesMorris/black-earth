using System.Collections;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour {

	public string highscoreURL;
	public GameObject container;
	public GameObject scorePrefab;

	void Start() {
		LoadScores();
	}

	public void LoadScores() {
		StartCoroutine(GetScores());
	}

	IEnumerator GetScores() {
		using(UnityWebRequest webRequest = UnityWebRequest.Get(highscoreURL)) {
			yield return webRequest.SendWebRequest();

			if (webRequest.isNetworkError) {
				Debug.Log("There was an error getting the high score: " + webRequest.error);
			} else {
				JSONNode results = JSON.Parse(webRequest.downloadHandler.text);

				// Destroy any remaining scores
				foreach (Transform child in container.transform) {
					Destroy(child.gameObject);
				}

				// Loop through each result
				for (int i = 0; i < results.Count; i++) {
					GameObject score = Instantiate(scorePrefab) as GameObject;
					score.transform.SetParent(container.transform, false);
					score.transform.Find("Rank").transform.Find("Text").GetComponent<Text>().text = (i + 1).ToString();
					score.transform.Find("Name").transform.Find("Text").GetComponent<Text>().text = results[i]["name"];
					score.transform.Find("Score").transform.Find("Text").GetComponent<Text>().text = results[i]["score"];
				}
			}
		}
	}
}