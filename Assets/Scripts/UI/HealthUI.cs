using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {

	public Text text;
	public GameObject bar;

	private LivingEntity playerEntity;
	private Image barImage;

	void Start () {
		playerEntity = GameObject.Find ("Player").GetComponent<LivingEntity> ();
		barImage = bar.GetComponent<Image> ();
	}

	void Update() {
		text.text = playerEntity.Health.ToString();

		float fillAmount = playerEntity.Health / playerEntity.startingHealth;
		barImage.fillAmount = fillAmount; 
	}
}
