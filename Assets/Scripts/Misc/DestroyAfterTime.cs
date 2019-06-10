using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

	public float timeBeforeDestroy = 0f;

	void Start() {
		StartCoroutine ("DestroyObject");
	}

	IEnumerator DestroyObject() {
		yield return new WaitForSeconds (timeBeforeDestroy);
		Destroy (gameObject);
	}

}
