using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour {

	public GameObject gameControllerPrefab;

	private void Start () {
		if (GameObject.FindGameObjectsWithTag ("GameController").Length == 0) {
			Instantiate (gameControllerPrefab, this.transform.position, Quaternion.identity);
		}
	}

	public void startNewGame () {
		Debug.Log ("Starting a new game!");
	}

	public void continueGame () {
		Debug.Log ("Continuing a previous game!");
	}

	public void exitApplication () {
		Application.Quit ();
	}
}
