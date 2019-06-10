using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
	private Vector3 velocity;
	private Animator animator;

	void Start() {
		rb = GetComponent<Rigidbody> ();
		animator = transform.Find ("PlayerObject").GetComponent<Animator> ();
	}

	public void Move(Vector3 _velocity) {
		velocity = _velocity;
	}

	public void LookAt(Vector3 lookPoint) {
		transform.LookAt (lookPoint);
	}
		
	void FixedUpdate() {
		// FixedUpdate is good for varying frame rates
		rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime); // fixedDeltaTime is the time between calls to FixedUpdate
		Animate();
	}

	void Animate() {
		if (velocity != Vector3.zero) {
			animator.SetBool ("isWalking", true);
		} else {
			animator.SetBool ("isWalking", false);
		}
	}
}
