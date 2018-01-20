using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TreasureButtonPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	private Treasure treasure;
	private GameObject popup;

	public void setTreasure (Treasure t) {
		this.treasure = t;
	}

	public void OnPointerEnter (PointerEventData data) {
		if (treasure == null) {
			return;
		}
		if (popup == null) {
			popup = (GameObject)Instantiate (Resources.Load ("PopUpPanel"),
				new Vector2 (this.transform.position.x, this.transform.position.y - 70),
				Quaternion.identity);
			popup.transform.SetParent (GameObject.Find("Canvas").transform, false);
			popup.transform.position = new Vector2 (this.transform.position.x, this.transform.position.y - 80);
		}
		if (popup != null) {
			popup.transform.Find ("Text").GetComponent<Text> ().text = treasure.getDesc ();
			popup.SetActive (true);
		}
	}

	public void OnPointerExit (PointerEventData data) {
		if (popup != null) {
			popup.SetActive (false);
		}
	}

}
