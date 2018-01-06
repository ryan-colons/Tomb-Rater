using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AdvisorButton : MonoBehaviour {

	private Advisor advisor;
	public GameObject canvas;
	private GameController gameController;

	private void Start () {
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
	}

	public void setAdvisor (Advisor newAdvisor) {
		this.advisor = newAdvisor;
	}

	private void OnMouseDown () {
		if (EventSystem.current.IsPointerOverGameObject ()) {
			return;
		}
		if (advisor.getTutorial () != null) {
			gameController.loadEvent (advisor.getTutorial ());
			return;
		}
		//toggle canvas
		if (canvas.activeSelf) {
			closeCanvas ();
		} else {
			openCanvas ();
		}
	}

	public void openCanvas () {
		//open just this canvas
		AdvisorMenu menu = GameObject.Find ("Canvas").GetComponent<AdvisorMenu> ();
		menu.closeAllAdvisorCanvases ();
		canvas.SetActive (true);
		//set text
		Text speechText = canvas.transform.Find("Panel/Speech Text").GetComponent<Text>();
		Text paymentText = canvas.transform.Find("Panel/Payment Text").GetComponent<Text>();
		speechText.text = advisor.getSpeech ();
		paymentText.text = "Giving " + advisor.getPayment().ToString() + "g";
		//set slider
		Slider paymentSlider = canvas.transform.Find("Panel/Payment Slider").GetComponent<Slider>();
		paymentSlider.minValue = 0;
		paymentSlider.maxValue = gameController.getMoney ();
		paymentSlider.value = advisor.getPayment ();
	}

	public void closeCanvas () {
		canvas.SetActive (false);
	}

	public void updatePayment () {
		Slider paymentSlider = canvas.transform.Find("Panel/Payment Slider").GetComponent<Slider>();
		Text paymentText = canvas.transform.Find("Panel/Payment Text").GetComponent<Text>();
		advisor.setPayment ((int)paymentSlider.value);
		paymentText.text = "Giving " + advisor.getPayment().ToString() + "g";
	}
	public int getPayment () {
		return advisor.getPayment();
	}
}
