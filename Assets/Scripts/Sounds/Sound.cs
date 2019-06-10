using UnityEngine;
using System;

public class Sound : MonoBehaviour {

	public bool destroyAfterPlay = false;
	public AudioSource audioSource;
	public enum SoundTypes{Music, SFX};
	public SoundTypes soundType;

	private float distance;
	private float distanceX;
	private SoundManager soundManager;
	private float maxVolume;
	private bool played = false;

	void Start() {
		audioSource = GetComponent<AudioSource> ();
		soundManager = GameObject.Find ("GameManager").GetComponent<SoundManager> ();

		if (audioSource == null) {
			AudioSource attached = GetComponent<AudioSource> ();
			if (attached != null) {
				audioSource = attached;
			}
		}

		ChangeVolume ();

	}

	void Update() {
		ChangeVolume ();
		if (soundManager.stereoSound) {
			SetDistance ();
			ChangePan ();
		} else {
			audioSource.volume = maxVolume;
			audioSource.panStereo = 0;
		}

		if (!audioSource.isPlaying && destroyAfterPlay && played) {
			Destroy (gameObject);
		}
	}

	void SetDistance() {
		Vector3 playerPos = new Vector3 (soundManager.player.transform.position.x, transform.position.y, transform.position.z);
		distanceX = Vector3.Distance (transform.position, playerPos);

		playerPos = new Vector3 (soundManager.player.transform.position.x, transform.position.y, soundManager.player.transform.position.z);
		distance = Vector3.Distance (transform.position, playerPos);
	}

	void ChangePan() {
		if (soundManager.changePan) {
			// Check player direction
			Vector3 playerPos = new Vector3 (soundManager.player.transform.position.x, 0f, soundManager.player.transform.position.y);
			Vector3 audioPos = new Vector3 (transform.position.x, 0f, transform.position.y);

			// They are in identical points
			if (playerPos == audioPos) {
				audioSource.panStereo = 0f;
			}

			// The player is on the right
			if (playerPos.x > audioPos.x) {
				audioSource.panStereo = -(distanceX / soundManager.panStereoDivision);
			}

			// The player is on the left
			if (playerPos.x < audioPos.x) {
				audioSource.panStereo = distanceX / soundManager.panStereoDivision;
			}
		}
	}

	void ChangeVolume() {

		switch (soundType) {
		case SoundTypes.Music:
			maxVolume = soundManager.MusicMaxVolume;
			break;
		case SoundTypes.SFX:
			maxVolume = soundManager.SFXMaxVolume;
			break;
		default:
			maxVolume = 1f;
			break;
		}

		audioSource.volume = maxVolume;
		if (soundManager.stereoSound) {
			audioSource.volume = maxVolume - (distance / soundManager.fadeoutDivision);
		}
	}

	public void PlaySound() {
		played = true;
		audioSource.Play ();
	}
}