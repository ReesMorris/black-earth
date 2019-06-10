using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SurvivorButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	[HideInInspector] public GameObject survivor;
	public int specialisationId;
	[HideInInspector] public bool active = false;

	private SpecialisationDatabase specialisationDatabase;
	private SurvivorManager survivorManager;
	private Tooltip tooltip;
	private bool hovering = false;
	private Specialisation specialisation;

	void Start() {
		tooltip = GameObject.Find ("UI").transform.Find ("Tooltip").GetComponent<Tooltip>();
		survivorManager = GameObject.Find ("GameManager").GetComponent<SurvivorManager> ();
		specialisationDatabase = GameObject.Find ("SpecialisationDatabase").GetComponent<SpecialisationDatabase> ();
		specialisation = specialisationDatabase.specialisations [specialisationId];
	}

	void Update() {
		if (hovering) {
			tooltip.SetPosition (new Vector2 (Input.mousePosition.x, Input.mousePosition.y - 120f + tooltip.BodyHeight ()));
		}
	}

	public void OnPointerEnter(PointerEventData dataName) {
		hovering = true;
		if (!active && specialisationId != 4 && (survivor.GetComponent<Survivor> ().scoutState == 0 || survivor.GetComponent<Survivor> ().scoutState == 1)) {
			Color color = GetComponent<Image> ().color;
			GetComponent<Image> ().color = new Color (color.r, color.g, color.b, 1f);
			transform.Find ("Image").GetComponent<Image> ().color = new Color (255f, 255f, 255f, 1f);
		}
		tooltip.ChangeText (specialisation.specialisationName.ToString (), specialisation.specialisationDescription);
	}

	public void OnPointerExit(PointerEventData dataName) {
		hovering = false;
		tooltip.Hide ();
		if (!active && specialisationId != 4 && (survivor.GetComponent<Survivor> ().scoutState == 0 || survivor.GetComponent<Survivor> ().scoutState == 1)) {
			Color color = GetComponent<Image> ().color;
			GetComponent<Image> ().color = new Color (color.r, color.g, color.b, 0.5f);
			transform.Find("Image").GetComponent<Image> ().color = new Color (255f, 255f, 255f, 0.5f);
		}
	}

	public void ChangeSpecialisation() {
		int scoutState = survivor.GetComponent<Survivor> ().scoutState;

		if (specialisationId == 4) {
			if (scoutState == 0) {
				survivor.GetComponent<Survivor> ().scoutState = 1;
				UpdateSpecialisation ();
			} else if (scoutState == 2) {
				survivor.GetComponent<Survivor> ().scoutState = 3;
				UpdateSpecialisation ();
			}
		} else {
			if (scoutState == 0 || scoutState == 1) {
				survivor.GetComponent<Survivor> ().scoutState = 0;
				UpdateSpecialisation ();
			}
		}
	}

	void UpdateSpecialisation() {
		survivor.GetComponent<Survivor> ().UpdateSpecialisation (specialisationId);
		survivorManager.GenerateList ();
		survivorManager.UpdateSurvivorNumbers ();
	}
}
