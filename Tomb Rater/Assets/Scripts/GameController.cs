using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	private SpecialEvent holsteredEvent;

	private void Awake () {
		DontDestroyOnLoad (this.gameObject);
		SpecialEvent.gameController = this;
		SpecialEventUI.gameController = this;
	}

	public void loadScene (string sceneName) {
		SceneManager.LoadScene (sceneName);
	}

	public void loadEvent (SpecialEvent specialEvent) {
		this.holsteredEvent = specialEvent;
		loadScene ("event");
	}

	public SpecialEvent getHolsteredEvent () {
		return holsteredEvent;
	}

}
