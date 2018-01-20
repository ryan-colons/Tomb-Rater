using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TreasurePanel : MonoBehaviour {

	private GameController gameController;
	public GameObject canvasWithBuildingMenu;
	private BuildingMenu menu;
	private RectTransform rect;
	public int hiddenY, displayY;
	private bool isHidden = true;
	public GameObject treasureButtonPrefab;

	private void Start () {
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
		rect = this.GetComponent<RectTransform> ();
		Button button = this.GetComponent<Button> ();
		button.onClick.AddListener (toggleHidden);
		menu = canvasWithBuildingMenu.GetComponent<BuildingMenu> ();
	}

	public void toggleHidden () {
		float x = rect.anchoredPosition.x;
		if (isHidden) {
			rect.anchoredPosition = new Vector2 (x, displayY);
			isHidden = false;
			open ();
		} else {
			rect.anchoredPosition = new Vector2 (x, hiddenY);
			isHidden = true;
			clearButtons ();
		}
	}

	public void open () {
		//close other menus
		menu.closeBuildingMenu();
		menu.closeRoomPanel ();
		addButtons ();
	}

	public void addButtons () {
		clearButtons ();

		ManageTreasure treasureManagement = gameController.getTreasureManagement();
		List<Treasure> treasureList = treasureManagement.getTreasureList ();

		Transform buttonParent = GameObject.Find ("Treasure Panel/ScrollPaneHolder/ScrollPane").transform;
		foreach (Treasure treasure in treasureList) {
			GameObject buttonObj = (GameObject)Instantiate (treasureButtonPrefab, buttonParent);
			Text nameText = buttonObj.transform.Find ("Text").GetComponent<Text> ();
			nameText.text = treasure.getName ();
			Image img = buttonObj.transform.Find ("Image").GetComponent<Image> ();
			img.sprite = treasure.getSprite ();

			Button button = buttonObj.GetComponent<Button> ();
			button.onClick.AddListener (delegate {
				menu.openDecorationMenu (treasure);
				rect.anchoredPosition = new Vector2 (rect.anchoredPosition.x, hiddenY);
				isHidden = true;
			});
			TreasureButtonPopup popupScript = buttonObj.GetComponent<TreasureButtonPopup> ();
			popupScript.setTreasure (treasure);
		}
	}

	public void clearButtons () {
		Transform buttonParent = GameObject.Find ("Treasure Panel/ScrollPaneHolder/ScrollPane").transform;
		foreach (Transform child in buttonParent) {
			Destroy (child.gameObject);
		}
		menu.closeDecorationMenu ();
	}
}
