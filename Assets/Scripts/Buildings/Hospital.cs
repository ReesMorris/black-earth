using UnityEngine;
using System.Collections;

public class Hospital : MonoBehaviour {

	public float defaultHealSpeed = 3f;
	public float doctorHealReduce = 0.5f;

	private Specialisation doctorSpecialisation;
	private WaveManager waveManager;

	void Start() {
		doctorSpecialisation = GameObject.Find("SpecialisationDatabase").GetComponent<SpecialisationDatabase>().specialisations[3];
		waveManager = GameObject.Find ("GameManager").GetComponent<WaveManager> ();

		waveManager.waveEndEvent += OnWaveEnd;
	}

	void OnWaveEnd() {
		if (doctorSpecialisation.currentCount > 0) {
			GameObject[] survivors = GameObject.FindGameObjectsWithTag ("Survivor");
			for (int i = 0; i < survivors.Length; i++) {
				LivingEntity entity = survivors [i].GetComponent<LivingEntity> ();
				if (entity != null) {
					entity.Heal (99999, true);
				}
			}
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.layer == 14 && collider.transform.root.name == "Player") {
			LivingEntity entity = collider.transform.root.GetComponent<LivingEntity> ();
			StopCoroutine ("Heal");
			StartCoroutine ("Heal", entity);
		}
	}

	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.layer == 14 && collider.transform.root.name == "Player") {
			StopCoroutine ("Heal");
		}
	}

	IEnumerator Heal(LivingEntity entity) {
		while (true) {
			float waitTime = defaultHealSpeed - (doctorHealReduce * doctorSpecialisation.currentCount);
			if (waitTime < 0.4f) {
				waitTime = 0.4f;
			}

			yield return new WaitForSeconds (waitTime);

			if (doctorSpecialisation.currentCount > 0) {
				entity.Heal (1, true);
			}
		}
	}
}