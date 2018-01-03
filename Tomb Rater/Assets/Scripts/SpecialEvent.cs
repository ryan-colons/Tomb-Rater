using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialEvent {

	public static SpecialEventUI eventUI;
	public static GameController gameController;

	private bool reuse;
	private int probability = 5;
	private string message;
	private string[] extraMessages = new string[0];
	private int messageIndex = 0;
	private string[] buttonTexts = new string[0];
	private string sprite;

	public void setMessage(string str) {
		message = str;
	}
	public string getMessage() {
		return message;
	}
	public void initialiseExtraMessageArray (int size) {
		this.extraMessages = new string[size];
	}
	public void setExtraMessage(string str, int index) {
		this.extraMessages [index] = str;
	}
	public string[] getButtonTexts () {
		return buttonTexts;
	}
	public void initialiseButtonTexts (int size) {
		this.buttonTexts = new string[size];
	}
	public void setButtonText (string str, int index) {
		this.buttonTexts [index] = str;
	}
	public void setReuse (bool b) {
		this.reuse = b;
	}
	public bool getReuse () {
		return this.reuse;
	}
	public void setProbability (int n) {
		this.probability = n;
	}
	public int getProbability () {
		return this.probability;
	}

	/* this needs testing - important that it doesn't think two events
	are the same simply in virtue of them both inheriting from SpecialEvent*/
	public bool isSameAs (SpecialEvent otherEvent) {
		if (this.GetType() == otherEvent.GetType()) {
			return true;
		} else {
			return false;
		}
	}

	public virtual bool prereq () {
		return true;
	}

	public void go() {
		//do everything
		GameObject panel = eventUI.setEventPanel(this.message);
		addButtons (panel);
	}

	public void next() {
		//progress to next message
		GameObject panel = eventUI.setEventPanel(this.extraMessages[messageIndex]);
		messageIndex += 1;
		addButtons (panel);
	}

	public void exit() {
		gameController.loadScene ("turn");
	}

	public void displayFeedback (string feedback) {
		GameObject panel = eventUI.setFeedbackDisplay (feedback);
		Button button = panel.GetComponent<Button> ();
		button.onClick.AddListener (exit);
	}

	public void addButtons(GameObject panel) {
		Button button = panel.GetComponent<Button> ();
		if (messageIndex < extraMessages.Length) {
			//if there are extra messages to see, prepare appropriately
			button.onClick.AddListener (next);
			EventPanel panelScript = panel.GetComponent<EventPanel> ();
			panelScript.prepareToDestroyThis ();
		} else {
			//provide buttons for the final decision
			GameObject[] buttonObjects = eventUI.setOptionsPanel(this);
			//add listeners to buttons
			for (int i = 0; i < buttonObjects.Length; i++) {
				switch (i) {
				case 0:
					buttonObjects[i].GetComponent<Button>().onClick.AddListener (option1);
					break;
				case 1:
					buttonObjects[i].GetComponent<Button>().onClick.AddListener (option2);
					break;
				case 2:
					buttonObjects[i].GetComponent<Button>().onClick.AddListener (option3);
					break;
				case 3:
					buttonObjects[i].GetComponent<Button>().onClick.AddListener (option4);
					break;
				case 4:
					buttonObjects[i].GetComponent<Button>().onClick.AddListener (option5);
					break;
				default:
					Debug.Log ("There are too many buttons, and not enough methods! " + buttonObjects.Length);
					break;
				}
			}
		}

	}

	public virtual void option1 () {}
	public virtual void option2 () {}
	public virtual void option3 () {}
	public virtual void option4 () {}
	public virtual void option5 () {}
}


public class Event_Introduction : SpecialEvent {
	
	public Event_Introduction () {
		this.setMessage ("Waking on your birthday, you think for the first time about " +
		"your own mortality. How will your people remember you when you are gone?\n\n...");
		
		initialiseExtraMessageArray(2);
		this.setExtraMessage ("You decide immediately that you will build a magnficient tomb, " +
		"to carry your legacy through the ages.\n\n...", 0);
		this.setExtraMessage ("And then a third, final panel with a question...", 1);

		/* The extra messages here should explain the next menu, ideally.
		 * i.e. make it clear that your talking to advisors about plans.
		 * but also the next screen should have some tutorial panels
		 */

		initialiseButtonTexts (5);
		this.setButtonText ("Go to main scene", 0);
		this.setButtonText ("Two", 1);
		this.setButtonText ("A third option with lots of words", 2);
		this.setButtonText ("A fourth option with even more text on it, to test the limits", 3);
		this.setButtonText ("Webster's dictionary defines fifth options as the longest of the long. Check out all this text. Does it even fit??", 4);
	}

	public override void option1 () {
		gameController.loadScene ("menu");
	}
	public override void option2 () {
		Debug.Log ("2");
	}
	public override void option3 () {
		Debug.Log ("3");
	}
	public override void option4 () {
		Debug.Log ("4");
	}
	public override void option5 () {
		Debug.Log ("5");
	}
}

public class Event_SpecialEventTest : SpecialEvent {
	public Event_SpecialEventTest () {
		this.setMessage ("This is just a test, to make sure the Special Event " +
		"system is working!");
		initialiseExtraMessageArray (1);
		this.setExtraMessage ("Looks like everything is in order... Carry on!", 0);
		initialiseButtonTexts (1);
		this.setButtonText ("Okey dokey", 0);
	}
	public override void option1 () {
		exit ();
	}
}

public class Event_LabourAllocationTutorial : SpecialEvent {

	public Event_LabourAllocationTutorial () {
		this.setMessage ("Well met, Your Majesty! I am here to discuss your plans for " +
		"managing our resources this year.");

		initialiseExtraMessageArray (1);
		this.setExtraMessage ("Okay, well hopefully that explains everything.", 0);

		initialiseButtonTexts (1);
		this.setButtonText ("Let's begin.", 0);
	}

	public override void option1 () {
		gameController.loadScene ("work_map");
	}
}

public class Event_TombBuildingTutorial : SpecialEvent {

	public Event_TombBuildingTutorial () {
		this.setMessage ("Well met, Your Highness! I am here to discuss your plans for " +
		"the construction of your tomb.");

		initialiseExtraMessageArray (1);
		this.setExtraMessage ("Well, you probably know best, let's just dive in.", 0);

		initialiseButtonTexts (1);
		this.setButtonText ("Yes, let's go.", 0);
	}

	public override void option1 () {
		gameController.loadScene ("building_map");
	}
}

public class Event_IncurableTerminalIllness : SpecialEvent {

	public Event_IncurableTerminalIllness () {
		this.setProbability (1);
		this.setReuse (false);
		this.setMessage ("Your physician brings terrible news! You have been diagnosed with " +
		"a terminal disease, for which there is absolutely no cure.");
		initialiseExtraMessageArray (1);
		this.setExtraMessage ("You have exactly 5 years to live. Best get that tomb finished...", 0);
		initialiseButtonTexts (1);
		this.setButtonText ("Persevere", 0);
	}
	public override void option1 () {
		ManageYears yearManagement = gameController.getYearManagement ();
		yearManagement.addSpecialEventInXYears(new Event_Death ("You were felled by an incurable disease. A physician " +
			"overseeing the autopsy observed your body and immediately invented an easy cure - tough break!"), 5);
		exit ();
	}
}

public class Event_Death : SpecialEvent {
	public Event_Death (string deathSummary) {
		this.setMessage ("You have died. " + deathSummary);
		initialiseExtraMessageArray (0);
		initialiseButtonTexts (1);
		this.setButtonText ("View legacy", 0);
	}
	public override void option1 () {
		//go to final scene, where you see your legacy etc
	}
}

public class Event_TradeOpportunity : SpecialEvent {
	public Event_TradeOpportunity () {
		this.setMessage ("A band of traders from " + gameController.getTradeCivName () + " has come. " +
		"They offer you an array of treasures.");
		initialiseExtraMessageArray (0);
		initialiseButtonTexts (5);
		this.setButtonText ("unimplemented", 0);
		this.setButtonText ("unimplemented", 1);
		this.setButtonText ("unimplemented", 2);
		this.setButtonText ("Seize all the goods by force (unimplemented)", 3);
		this.setButtonText ("Buy nothing", 4);
	}
	public override void option5 () {
		displayFeedback ("Visibly disappointed, the traders pick up their things and leave.");
	}
}

/* Checklist for writing a SpecialEvent
 * Constructor:
 *      -set probability and reusability
 * 		-initialise extra message array, and set extra messages
 * 		-initialise button texts array, and set button texts
 * 		-set the sprite (??)
 * Implement an option for each button (option1... optionN)
 */