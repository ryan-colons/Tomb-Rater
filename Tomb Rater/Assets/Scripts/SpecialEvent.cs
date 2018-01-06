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
		CharacterData info = gameController.getCharData ();
		this.setMessage ("You wake up in a cold sweat, haunted by nightmares. For the first time, " +
		"on your " + info.getAgeOrdinal () + " birthday, your thoughts have turned to your own mortality.\n\n...");
		
		initialiseExtraMessageArray(3);
		this.setExtraMessage ("You're going to be gone forever! Your accomplishments are impermanent, " +
			"meaningless in the face of eternity! You'll be dumped into earth, doomed to decay while the " +
			"people above forget your name, a nameless pile of dust in an uncaring universe...\n\n...", 0);
		this.setExtraMessage ("No! You refuse to be forgotten!\n\nA tomb will be built! A glorious tomb, to " +
			"carry your legacy through the ages!", 1);
		this.setExtraMessage ("You assemble a team of your wisest Advisors, to bring your visions into reality. " +
			"", 2);

		initialiseButtonTexts (1);
		this.setButtonText ("Continue", 0);
	}

	public override void option1 () {
		gameController.loadScene ("menu");
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

public class Event_MilitaryAdvisorTutorial : SpecialEvent {
	public Event_MilitaryAdvisorTutorial () {
		this.setMessage ("");
		initialiseButtonTexts (1);
		this.setButtonText ("Continue", 0);
	}
	public override void option1 () {
		gameController.loadScene ("menu");
		ManageAdvisors manageAdvisors = gameController.getAdvisorManagement ();
		Advisor militaryAdvisor = manageAdvisors.getAdvisors () [ManageAdvisors.MILITARY];
		militaryAdvisor.setTutorial (null);
	}
}

public class Event_EconomicAdvisorTutorial : SpecialEvent {
	public Event_EconomicAdvisorTutorial () {
		this.setMessage ("");
		initialiseButtonTexts (1);
		this.setButtonText ("Continue", 0);
	}
	public override void option1 () {
		gameController.loadScene ("menu");
		ManageAdvisors manageAdvisors = gameController.getAdvisorManagement ();
		Advisor economicAdvisor = manageAdvisors.getAdvisors () [ManageAdvisors.ECONOMY];
		economicAdvisor.setTutorial (null);
	}
}

public class Event_GuildAdvisorTutorial : SpecialEvent {
	public Event_GuildAdvisorTutorial () {
		this.setMessage ("");
		initialiseButtonTexts (1);
		this.setButtonText ("Continue", 0);
	}
	public override void option1 () {
		gameController.loadScene ("menu");
		ManageAdvisors manageAdvisors = gameController.getAdvisorManagement ();
		Advisor guildAdvisor = manageAdvisors.getAdvisors () [ManageAdvisors.GUILDS];
		guildAdvisor.setTutorial (null);
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

public class Event_GettingRaided : SpecialEvent {
	
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