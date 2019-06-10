using UnityEngine;

public class WeaponHold : MonoBehaviour {
	
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); // returns a ray from the camera to the mouse position
		Plane groundPlane = new Plane (Vector3.up, transform.position); // generate a plane programmatically, looking up at 0,0,0
		float rayDistance;

		if (groundPlane.Raycast (ray, out rayDistance)) { // takes a Ray and will give out a variable (rayDistance). Returns true if the ray intersects with the ground plane
			Vector3 point = ray.GetPoint(rayDistance);
			transform.LookAt(point);
		}
	}
}
