﻿using UnityEngine;

public class DontDestroy : MonoBehaviour {

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}
}
