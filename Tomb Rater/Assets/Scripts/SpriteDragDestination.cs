using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDragDestination : MonoBehaviour {

	/* SPRITE MUST HAVE A TRIGGER COLLIDER AND A RIGIDBODY
	 * SET EVERYTHING ON THE RIGIDBODY TO 0, SO IT DOESN'T MOVE
	 */

	private SpriteSupervisor sprSupervisor;

	private Ray ray;
	private RaycastHit hit;

	private void Start () {
		sprSupervisor = GameObject.Find ("SpriteSupervisor").GetComponent<SpriteSupervisor> ();
	}

	private void OnTriggerStay2D (Collider2D other) {
		if (sprSupervisor.getDraggedSpr () == other.gameObject) {
			if (Input.GetButtonUp ("LClick")) {
				GameObject dragObj = sprSupervisor.getDraggedSpr ();
				SpriteDrag otherDrag = dragObj.GetComponent<SpriteDrag> ();
				otherDrag.setEndPos (this.transform.position);
			}
		}
	}
}
