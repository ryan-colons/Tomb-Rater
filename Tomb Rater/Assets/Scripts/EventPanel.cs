using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventPanel : MonoBehaviour {

	public void prepareToDestroyThis () {
		Button button = this.GetComponent<Button> ();
		button.onClick.AddListener (destroyThis);
	}

	public void destroyThis () {
		Destroy (this.gameObject);
	}
}
