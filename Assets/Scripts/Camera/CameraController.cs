using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform target;
	public bool cameraLock;

	[Header("Distance")]
	public float distanceX;
	public float distanceY;
	public float distanceZ;

	void Update() {
		if (cameraLock) {
			if (target != null) {
				Vector3 position = new Vector3 (target.position.x + distanceX, target.position.y + distanceY, target.position.z + distanceZ);
				transform.position = position;
				transform.LookAt (target);
			} else {
				Debug.LogError ("Camera cannot lock on to target: Target is null!");
			}
		}
	}
}
