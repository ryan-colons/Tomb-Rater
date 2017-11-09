using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDrag : MonoBehaviour {

	private bool dragging;
	private float dragSpeed = 10f;
	private Vector3 endPos;
	private SpriteSupervisor sprSupervisor;
	private string actualSortingLayer;

	private void Start () {
		endPos = this.transform.position;
		sprSupervisor = GameObject.Find ("SpriteSupervisor").GetComponent<SpriteSupervisor>();
		actualSortingLayer = this.GetComponent<SpriteRenderer> ().sortingLayerName;
	}

	private void OnMouseDown () {
		endPos = this.transform.position;
		dragging = true;
		sprSupervisor.setDraggedSpr (this.gameObject);
		actualSortingLayer = this.GetComponent<SpriteRenderer> ().sortingLayerName;
		this.GetComponent<SpriteRenderer> ().sortingLayerName = "Drag";
	}

	private void OnMouseUp () {
		dragging = false;
		sprSupervisor.setDraggedSpr (null);
		this.GetComponent<SpriteRenderer> ().sortingLayerName = actualSortingLayer;
	}

	private void Update () {
		if (dragging) {
			Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			this.transform.position = Vector3.Lerp (this.transform.position, mousePos, Time.deltaTime * dragSpeed);
		} else if (this.transform.position != endPos) {
			this.transform.position = Vector3.Lerp (this.transform.position, endPos, Time.deltaTime * dragSpeed);
		}
	}

}
