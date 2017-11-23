using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ResourcePanel : MonoBehaviour {

	public float hiddenY, displayY;
	private bool isHidden;
	private RectTransform rect;

	private void Start () {
		isHidden = true;
		rect = this.GetComponent<RectTransform> ();
		Button button = this.GetComponent<Button> ();
		button.onClick.AddListener (toggleHidden);
	}

	private void toggleHidden () {
		float x = rect.anchoredPosition.x;
		if (isHidden) {
			rect.anchoredPosition = new Vector2 (x, displayY);
			isHidden = false;
		} else {
			rect.anchoredPosition = new Vector2 (x, hiddenY);
			isHidden = true;
		}
	}
}
