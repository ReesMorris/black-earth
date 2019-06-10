using UnityEngine;
using System.Collections.Generic;

public class GunManager : MonoBehaviour {

	public AmmoUI ammoUI;

	[Header("Muzzle Particles")]
	public ParticleSystem flashParticles;

	[Header("Collision Particles")]
	public ParticleSystem defaultParticles;
	public ParticleSystem enemyParticles;

	[Header("Don't shoot if...")]
	public bool shootIfDraggingItem = false;
	public List<string> tags;
}
