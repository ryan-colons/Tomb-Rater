using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishYearButton : MonoBehaviour {

	private void OnMouseDown () {
		AdvisorMenu menu = GameObject.Find ("Canvas").GetComponent<AdvisorMenu> ();
		if (menu.validateMoneyAllocation ()) {
			GameController gc = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
			gc.nextYear ();
		} else {
			this.GetComponent<SpriteMouseOver> ().setMessage ("You don't have enough money!");
		}
	}
}
