using UnityEngine;

public class SoundManager : MonoBehaviour {

	public GameObject player;

	[Header("General Config")]
	[Range(0,1)] public float MusicMaxVolume = 1f;
	[Range(0,1)] public float SFXMaxVolume = 1f;

	[Header("Stereo Config")]
	public bool stereoSound = true;
	public bool changePan = true;
	public bool changeVolume = true;

	[Header("Values")]
	public float panStereoDivision = 7f;
	public float fadeoutDivision = 15f;

	void Start() {
		MusicMaxVolume = PlayerPrefs.GetFloat ("music", 1);
		SFXMaxVolume = PlayerPrefs.GetFloat ("sfx", 1);
		stereoSound = PlayerPrefs.GetInt ("stereo") != 0;
	}

	void Update() {
		float music = PlayerPrefs.GetFloat ("music");
		float sfx = PlayerPrefs.GetFloat ("sfx", 1);
		bool stereo = PlayerPrefs.GetInt ("stereo") != 0;

		if (MusicMaxVolume != music) {
			MusicMaxVolume = music;
		}

		if (SFXMaxVolume != sfx) {
			SFXMaxVolume = sfx;
		}

		if (stereoSound != stereo) {
			stereoSound = stereo;
		}
	}
}