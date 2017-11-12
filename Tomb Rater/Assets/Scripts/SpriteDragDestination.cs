using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider2D))]
public class SpriteDragDestination : MonoBehaviour {

	/* SPRITE MUST HAVE A TRIGGER COLLIDER AND A RIGIDBODY
	 * SET EVERYTHING ON THE RIGIDBODY TO 0, SO IT DOESN'T MOVE
	 */
	public bool site;

	private SpriteSupervisor sprSupervisor;

	private SpriteRenderer sprRenderer;
	private Color actualColor;

	private Ray ray;
	private RaycastHit hit;

	private void Start () {
		sprSupervisor = GameObject.Find ("SpriteSupervisor").GetComponent<SpriteSupervisor> ();
		sprRenderer = this.GetComponent<SpriteRenderer> ();
		actualColor = sprRenderer.color;
	}

	private void OnTriggerStay2D (Collider2D other) {
		if (sprSupervisor.getDraggedSpr () == other.gameObject) {
			sprRenderer.color = Color.red;
			if (Input.GetButtonUp ("LClick")) {
				GameObject dragObj = sprSupervisor.getDraggedSpr ();
				SpriteDrag otherDrag = dragObj.GetComponent<SpriteDrag> ();
				otherDrag.setEndPos (this.transform.position);
				if (site) {
					this.GetComponent<Site> ().handleDrop (other.gameObject);
					Destroy (other.gameObject);
				}
			}
		}
	}

	private void OnTriggerExit2D (Collider2D other) {
		sprRenderer.color = actualColor;
	}

	public void setSite (bool b) {
		site = b;
	}
}
