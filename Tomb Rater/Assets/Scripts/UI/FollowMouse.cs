using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

	private float dragSpeed = 20f;

	private void Start () {
		SpriteRenderer sprRenderer = this.GetComponent<SpriteRenderer> ();
		Treasure treasure = BuildingMenu.currentlyPlacing;
		if (treasure != null) {
			sprRenderer.sprite = treasure.getSprite ();
		}
	}

	private void Update () {
		Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		this.transform.position = Vector3.Lerp (this.transform.position, mousePos, Time.deltaTime * dragSpeed);
	}

}
