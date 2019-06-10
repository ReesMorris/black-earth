using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {

	[Header("Score")]
	public string highScoreURL;

	[Header("GameObjects")]
	public GameObject gameOverPanel;
	public Text topTime;
	public Text topWaves;
	public Text enemyKills;
	public Text totalFood;
	public Text totalSurvivors;
	public Text totalRP;
	public Text finalScoreText;
	public GameObject usernameField;
	public GameObject scoreSubmitted;
	public GameObject scoreUnsubmitted;

	[Header("Main")]
	public Text kills;
	public Text food;
	public Text survivorsAlive;
	public Text survivorsDead;
	public Text rp;

	private MouseManager mouseManager;
	private CameraManager cameraManager;
	private PauseManager pauseManager;
	private WaveManager waveManager;
	private ScoreManager scoreManager;

	void Start() {
		mouseManager = GetComponent<MouseManager>();
		cameraManager = GetComponent<CameraManager>();
		pauseManager = GetComponent<PauseManager>();
		waveManager = GetComponent<WaveManager>();
		scoreManager = GetComponent<ScoreManager>();
	}

	public void GameOver(bool canSubmitHighScore) {

		if (!canSubmitHighScore) {
			scoreSubmitted.transform.Find("Submit").GetComponent<Text>().text = "High Scores unavailable in this mode";
			scoreSubmitted.SetActive(true);
			scoreUnsubmitted.SetActive(false);
		}

		mouseManager.ChangeMouse(false, true);
		cameraManager.EnableBlur(true);
		cameraManager.SetBlurValues(10, 0.51f);

		gameOverPanel.SetActive(true);

		pauseManager.gameIsPaused = true;
		pauseManager.SetModalOpen(true);
		pauseManager.modalName = "gameover";
		Time.timeScale = 0;

		SetTopText();
		CalculateFinalScore();
	}

	void SetTopText() {
		// [12]
		float minutes = Mathf.Floor(Time.time / 60);
		float seconds = Mathf.Round(Time.time % 60);

		topTime.text = minutes.ToString("00") + ":" + seconds.ToString("00"); // [13]

		string waves = " Waves";
		if (waveManager.CurrentWave - 2 == 1) {
			waves = " Wave";
		}
		topWaves.text = waveManager.CurrentWave - 2 + waves;
	}

	void CalculateFinalScore() {
		enemyKills.text = scoreManager.Kills.ToString();
		totalFood.text = scoreManager.totalFoodRations.ToString();
		totalSurvivors.text = scoreManager.totalSurvivors.ToString();
		totalRP.text = 0. ToString();

		kills.text = scoreManager.Kills.ToString();
		food.text = scoreManager.totalFoodRations.ToString();
		survivorsAlive.text = GetComponent<SurvivorManager>().survivors.Count.ToString();
		survivorsDead.text = scoreManager.deadSurvivors.ToString();
		rp.text = 0. ToString();

		finalScoreText.text = FinalScore().ToString();
	}

	int FinalScore() {
		return scoreManager.Kills + scoreManager.totalFoodRations + scoreManager.totalSurvivors + 0;
	}

	public void GoToMainMenu() {
		SceneManager.LoadScene(0);
	}

	public void SubmitHighScore() {
		StartCoroutine(PostScores(usernameField.transform.Find("Text").GetComponent<Text>().text, FinalScore()));

		scoreSubmitted.SetActive(true);
		scoreUnsubmitted.SetActive(false);
	}

	// [14]
	IEnumerator PostScores(string name, int score) {

		WWWForm form = new WWWForm();
		form.AddField("name", name);
		form.AddField("score", score);

		using(UnityWebRequest www = UnityWebRequest.Post(highScoreURL, form)) {
			yield return www.SendWebRequest();
			if (www.isNetworkError || www.isHttpError) Debug.Log(www.error);
		}
	}
}