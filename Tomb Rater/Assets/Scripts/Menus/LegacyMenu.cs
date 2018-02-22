using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LegacyMenu : MonoBehaviour {

	public Scrollbar scrollbar;
	public Text reportText;
	public GameObject finishButton;

	private void Start () {
		GameController gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
		TombRater rater = gameController.getTombRater ();
		int score = rater.rateTomb ();
		List<string> report = rater.getReport ();
		StartCoroutine (displayReport (report, score));
	}

	public void addToText (string str) {
		reportText.text += str + "\n\n";
		scrollToBottom ();
	}

	public IEnumerator displayReport (List<string> report, int score) {
		while (report.Count > 0) {
			string news = report [0];
			report.RemoveAt (0);
			addToText (news);
			yield return new WaitForSeconds (2f);
		}
		addToText ("You were remembered for " + score + " years after your death.");
		finishButton.SetActive (true);
	}

	public void scrollToBottom () {
		scrollbar.value = 0;
	}

	public void quit () {
		// save game here
		Application.Quit ();
	}
}
