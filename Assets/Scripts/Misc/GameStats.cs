using UnityEngine;

public class GameStats : MonoBehaviour {

	// Per Wave
	// These stats are changed constantly
	[HideInInspector] public int enemiesRemaining = 0;

	// Per Game
	// These stats are only incremented, and are kept throughout each wave

	// Kills
	[HideInInspector] public int enemiesKilled = 0;
	[HideInInspector] public int survivorsKilled = 0;

	// Bullets
	[HideInInspector] public int playerBulletsShot = 0;
	[HideInInspector] public int survivorBulletsShot = 0;
}
