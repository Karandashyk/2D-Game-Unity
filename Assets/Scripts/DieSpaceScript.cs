using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieSpaceScript : MonoBehaviour {

	public GameObject respawn;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag("Player")) {
			other.transform.position = respawn.transform.position;
			HealthScript playerHealth = this.GetComponent<HealthScript>();
			if (playerHealth != null) playerHealth.Damage(1, true);
		}
	}
}
