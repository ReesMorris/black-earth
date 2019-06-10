using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour {

	public GameObject target;
	public bool moveTowardsTarget;
	private bool hasHadTarget = false;

	private NavMeshAgent agent;
	private Animator animator;
	private Vector3 targetPosition;

	void Start () {
		agent = GetComponent<NavMeshAgent>();
		SetAnimator ();
	}

	void Update() {
		MoveTowardsTarget ();
		Animate ();
	}

	void MoveTowardsTarget() {
		if (target != null && moveTowardsTarget) {
			hasHadTarget = true;
			agent.Resume ();
			targetPosition = target.transform.position;
			agent.destination = targetPosition;
		} else {
			if (hasHadTarget) {
				agent.Stop ();
			}
		}
	}

	void Animate() {
		if (agent.velocity == Vector3.zero || !moveTowardsTarget || target == null) {
			animator.SetBool ("isWalking", false);
		} else {
			animator.SetBool ("isWalking", true);
		}
	}

	public void SetAnimator() {
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild (i).gameObject.activeSelf) {
				animator = transform.GetChild (i).GetComponent<Animator> ();
				break;
			}
		}
	}
}