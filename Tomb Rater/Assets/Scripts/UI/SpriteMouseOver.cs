﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class SpriteMouseOver : MonoBehaviour {

	private Color actualColor;
	private SpriteRenderer sprRenderer;
	private SpriteSupervisor sprSupervisor;

	public string message;
	private GameObject popUp;

	private void Start () {
		sprRenderer = this.GetComponent<SpriteRenderer> ();
		actualColor = sprRenderer.color;
		sprSupervisor = GameObject.Find ("SpriteSupervisor").GetComponent<SpriteSupervisor> ();
	}

	private void OnMouseEnter () {
		if (EventSystem.current.IsPointerOverGameObject ()) {
			return;
		}
		if (popUp == null && hasMessage()) {
			popUp = (GameObject)Instantiate (Resources.Load ("PopUpCanvas"),
				new Vector3 (this.transform.position.x, this.transform.position.y + 2, this.transform.position.z),
				Quaternion.identity);
			popUp.transform.SetParent (this.transform);
		}
		if (popUp != null) {
			popUp.transform.Find ("Panel/Text").GetComponent<Text> ().text = message;
			popUp.SetActive (true);
		}
		sprRenderer.color = Color.blue;
	}

	private void OnMouseExit () {
		if (sprSupervisor.getDraggedSpr () != this.gameObject) {
			if (popUp != null) {
				popUp.SetActive (false);
			}
			sprRenderer.color = actualColor;
		}
	}

	private bool hasMessage() {
		return message != null && !message.Equals ("");
	}

	public void setMessage(string str) {
		message = str;
	}

	public void setBaseSpriteColor (Color c) {
		sprRenderer.color = c;
		actualColor = c;
	}
		

}
