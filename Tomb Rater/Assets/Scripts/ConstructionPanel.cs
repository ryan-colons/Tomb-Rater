using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ConstructionPanel : MonoBehaviour {

	public float hiddenX, displayX;
	private bool isHidden;
	private RectTransform rect;

	private void Start () {
		isHidden = true;
		rect = this.GetComponent<RectTransform> ();
		Button button = this.GetComponent<Button> ();
		button.onClick.AddListener (toggleHidden);
	}

	public void toggleHidden () {
		float y = rect.anchoredPosition.y;
		if (isHidden) {
			rect.anchoredPosition = new Vector2 (displayX, y);
			isHidden = false;
			//refresh buttons
		} else {
			rect.anchoredPosition = new Vector2 (hiddenX, y);
			isHidden = true;
		}
	}
}
