using UnityEngine;

[System.Serializable]
public class Specialisation {

	public enum Roles{Fighter, Farmer, Reesearcher, Doctor, Scout};
	public Roles specialisationName;
	public string specialisationDescription;

	public GameObject model;
	public GameObject specialisationZone;
	public GameObject retreatZone;

	[Header("Survival Instincts")]
	public float retreatPercentage = 50;
	public int viewRange;
	public bool killVisibleEnemies = true;
	public bool moveToHiddenEnemies = false;
	public bool pickupNearbyItems = false;

	[Header("Limits")]
	public bool specialisationHasLimit;
	public int specialisationLimit;
	public int currentCount = 0;
}
