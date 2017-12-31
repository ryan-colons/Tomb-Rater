using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ResourcePanel : MonoBehaviour {

	public float hiddenY, displayY;
	private bool isHidden;
	private RectTransform rect;
	private GameController gameController;

	public GameObject woodSubPanel, stoneSubPanel, metalSubPanel, foodSubPanel;

	private void Start () {
		isHidden = true;
		rect = this.GetComponent<RectTransform> ();
		Button button = this.GetComponent<Button> ();
		button.onClick.AddListener (toggleHidden);
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}

	private void toggleHidden () {
		updateNumbers ();
		float x = rect.anchoredPosition.x;
		if (isHidden) {
			rect.anchoredPosition = new Vector2 (x, displayY);
			isHidden = false;
		} else {
			rect.anchoredPosition = new Vector2 (x, hiddenY);
			isHidden = true;
		}
	}

	private void updateNumbers () {
		ManageResources resourceManagement = gameController.getResourceManagement ();
		Text woodText = woodSubPanel.transform.Find ("Text").GetComponent<Text> ();
		Text stoneText = stoneSubPanel.transform.Find ("Text").GetComponent<Text> ();
		Text metalText = metalSubPanel.transform.Find ("Text").GetComponent<Text> ();
		Text foodText = foodSubPanel.transform.Find ("Text").GetComponent<Text> ();

		woodText.text = resourceManagement.getResourceTypeAmount (ResourceType.WOOD).ToString ();
		stoneText.text = resourceManagement.getResourceTypeAmount (ResourceType.STONE).ToString ();
		metalText.text = resourceManagement.getResourceTypeAmount (ResourceType.METAL).ToString ();
		foodText.text = resourceManagement.getResourceTypeAmount (ResourceType.FOOD).ToString ();
	}
}
