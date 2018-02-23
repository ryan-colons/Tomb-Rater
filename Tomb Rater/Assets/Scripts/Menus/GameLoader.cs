using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour {

	public GameObject gameControllerPrefab;
	private GameController gameController;

	private void Start () {
		GameObject[] gcList = GameObject.FindGameObjectsWithTag ("GameController");
		if (gcList.Length == 0) {
			GameObject obj = (GameObject)Instantiate (gameControllerPrefab, this.transform.position, Quaternion.identity);
			gameController = obj.GetComponent<GameController> ();
		} else if (gcList.Length == 1) {
			gameController = gcList [0].GetComponent<GameController> ();
		} else {
			Debug.Log ("There are too many GameControllers! " + gcList.Length);
			exitApplication ();
		}
	}

	public void startNewGame () {
		gameController.loadScene ("char_creation");
	}

	public void continueGame () {
		if (gameController.load ()) {
			gameController.loadScene ("menu");
		}
	}

	public void exitApplication () {
		Application.Quit ();
	}
}
