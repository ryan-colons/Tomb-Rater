using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ConstructionPanel : MonoBehaviour {

	public float hiddenX, displayX;
	private bool isHidden;
	private RectTransform rect;
	private GameController gameController;
	public GameObject constructionButtonPrefab;
	public BuildingMenu buildingMenu;

	private void Start () {
		isHidden = true;
		rect = this.GetComponent<RectTransform> ();
		Button button = this.GetComponent<Button> ();
		button.onClick.AddListener (toggleHidden);
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
	}

	public void toggleHidden () {
		float y = rect.anchoredPosition.y;
		if (isHidden) {
			rect.anchoredPosition = new Vector2 (displayX, y);
			isHidden = false;
			buildingMenu.closeDecorationMenu ();
			addButtons ();
		} else {
			rect.anchoredPosition = new Vector2 (hiddenX, y);
			isHidden = true;
			clearButtons ();
		}
	}

	public void addButtons () {
		ManageBuilding buildingManagement = gameController.getBuildingManagement ();
		TombRoom[] availableRooms = buildingManagement.getAvailableRooms ();

		Transform buttonParent = GameObject.Find ("Construction Panel/ScrollPaneHolder/ScrollPane").transform;
		foreach (TombRoom room in availableRooms) {
			GameObject buttonObj = (GameObject)Instantiate (constructionButtonPrefab, buttonParent);
			Text nameText = buttonObj.transform.Find ("Name Text").GetComponent<Text> ();
			Text costText = buttonObj.transform.Find ("Cost Text").GetComponent<Text> ();
			nameText.text = room.getName ();
			costText.text = room.getMinSize ().ToString();
			Button button = buttonObj.GetComponent<Button> ();
			button.onClick.AddListener (delegate {
				buildingMenu.openRoomPanel(room);
			});
		}
	}

	public void clearButtons () {
		Transform buttonParent = GameObject.Find ("Construction Panel/ScrollPaneHolder/ScrollPane").transform;
		foreach (Transform child in buttonParent) {
			Destroy (child.gameObject);
		}
	}
}
