using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteMouseOver : MonoBehaviour {

	private Color actualColor;
	private SpriteRenderer sprRenderer;

	public string message;
	private GameObject popUp;

	private void Start () {
		sprRenderer = this.GetComponent<SpriteRenderer> ();
	}

	private void OnMouseEnter () {
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
		actualColor = sprRenderer.color;
		sprRenderer.color = Color.blue;
	}

	private void OnMouseExit () {
		if (popUp != null) {
			popUp.SetActive (false);
		}
		sprRenderer.color = actualColor;
	}

	private bool hasMessage() {
		return message != null && !message.Equals ("");
	}	

}
