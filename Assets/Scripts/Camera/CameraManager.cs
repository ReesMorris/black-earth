using UnityEngine;
using UnityStandardAssets.ImageEffects; // [8]

public class CameraManager : MonoBehaviour {

	public GameObject mainCam;

	private Blur blurScript;

	void Start() {
		blurScript = mainCam.GetComponent<Blur>();
		EnableBlur (false);
	}

	public void EnableBlur(bool enabled) {
		blurScript.enabled = enabled;
	}

	public void SetBlurValues (int iterations, float blurSpread) {
		blurScript.iterations = iterations;
		blurScript.blurSpread = blurSpread;
	}
}
