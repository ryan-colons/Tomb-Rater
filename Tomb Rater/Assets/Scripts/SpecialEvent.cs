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
			"carry your legacy through the ages!\n\n...", 1);
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
		this.setExtraMessage ("", 0);

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

public class Event_GuildTreasure : SpecialEvent {
	public Event_GuildTreasure () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		Advisor guildAdvisor = advisorManagement.getAdvisors () [ManageAdvisors.GUILDS];
		this.setMessage (guildAdvisor.getName() + ", your Guild Representative, comes to you during the year.");
		initialiseExtraMessageArray (2);
		this.setExtraMessage ("With the money you have set aside, we can afford to commission a work of art from one of the guilds. " +
			"The best offers are as follows...", 0);
		this.setExtraMessage ("The Masonry Guild will carve a marble statue of you.\nThe Weaver's Alliance will create " +
			"a beautiful tapestry, depicting your finest moments.\nThe Metal Workers will forge a gold-plated candelabra, " +
			"of the highest quality.", 1);
		initialiseButtonTexts (3);
		this.setButtonText ("Carve a statue in my likeness", 0);
		this.setButtonText ("Weave my deeds", 1);
		this.setButtonText ("My tomb needs gold and fire", 2);
	}
	public override void option1 () {
		ManageTreasure treasureManagement = gameController.getTreasureManagement ();
		CharacterData info = gameController.getCharData ();
		treasureManagement.getTreasureList ().Add (new Tre_StatueOfSomeone (info.getPlayerTitle () + " " + info.getPlayerName ()));
		exit ();
	}
	public override void option2 () {
		ManageTreasure treasureManagement = gameController.getTreasureManagement ();
		treasureManagement.getTreasureList ().Add (new Tre_Tapestry ());
		exit ();
	}
	public override void option3 () {
		ManageTreasure treasureManagement = gameController.getTreasureManagement ();
		treasureManagement.getTreasureList ().Add (new Tre_Candelabra ());
		exit ();
	}
}

public class Event_ForeignGuildTreasure : SpecialEvent {
	public Event_ForeignGuildTreasure () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		Advisor guildAdvisor = advisorManagement.getAdvisors () [ManageAdvisors.GUILDS];
		this.setMessage (guildAdvisor.getName() + ", your Guild Representative, comes to you during the year.");
		initialiseExtraMessageArray (2);
		this.setExtraMessage ("With the money you have set aside, we can afford to commission a work of art from one of the guilds. " +
			"There are some truely amazing offers...", 0);
		this.setExtraMessage ("An ornate sarcophagus, made entirely from ruby.\nAn oaken chest, enchanted to swallow would-be tomb robbers.\n " +
			"A self-sustaining fountain of liquid silver.", 1);
		initialiseButtonTexts (3);
		this.setButtonText ("The ruby sarcophagus", 0);
		this.setButtonText ("The cursed chest", 1);
		this.setButtonText ("The silver fountain", 2);
	}
	public override void option1 () {
		ManageTreasure treasureManagement = gameController.getTreasureManagement ();
		treasureManagement.getTreasureList ().Add (new Tre_RubySarcophagus ());
		exit ();
	}
	public override void option2 () {
		ManageTreasure treasureManagement = gameController.getTreasureManagement ();
		treasureManagement.getTreasureList ().Add (new Tre_ChestOfGreed ());
		exit ();
	}
	public override void option3 () {
		ManageTreasure treasureManagement = gameController.getTreasureManagement ();
		treasureManagement.getTreasureList ().Add (new Tre_FountainOfSilver ());
		exit ();
	}
}

public class Event_FoundNewGuild : SpecialEvent {
	public Event_FoundNewGuild () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		Advisor guildAdvisor = advisorManagement.getAdvisors () [ManageAdvisors.GUILDS];
		this.setMessage (guildAdvisor.getName () + ", we are preparing to found a new guild! There are several " +
		"candidates to consider.");
		initialiseExtraMessageArray (4);
		this.setExtraMessage ("A new religion has begun to grow here. The followers of the Badger Prince wish to " +
		"found a temple. The subterranean nature of your tomb building project appeals to them.", 0);
		this.setExtraMessage ("A cohort of bards from across the world have gathered together. They want to form a guild " +
			"based around partying every day, 'going hard', and something called 'nangs'.", 1);
		this.setExtraMessage ("Practioneers of a strange art, calling themselves the Society of Morticians, wish to " +
			"form a guild. They specialise in the study and manipulation of the forces of life and death. It's... interesting.", 2);
		this.setExtraMessage ("Which of these groups would you most like to support?", 3);
		initialiseButtonTexts (3);
		this.setButtonText ("Found the Temple of the Badger Prince", 0);
		this.setButtonText ("Found the Rave Syndicate", 1);
		this.setButtonText ("Found the Society of Morticians", 2);
	}
	public override void option1 () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		advisorManagement.getAdvisors () [ManageAdvisors.TEMPLE] = new Advisor ();
		//set first milestone for badger advisor
		//set tutorial for advisor also
		exit();
	}
	public override void option2 () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		advisorManagement.getAdvisors () [ManageAdvisors.PARTY] = new Advisor ();
		//set first milestone for party advisor
		//set tutorial for advisor also
		ManageYears yearManagement = gameController.getYearManagement();
		yearManagement.addSpecialEventInXYears (new Event_PeopleLoveParties (), 1);
		exit();
	}
	public override void option3 () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		advisorManagement.getAdvisors () [ManageAdvisors.NECRO] = new Advisor ();
		//set first milestone for necro advisor
		//set tutorial for advisor also
		ManageYears yearManagement = gameController.getYearManagement();
		yearManagement.addSpecialEventInXYears (new Event_PeopleFearNecromancy (), 1);
		exit();
	}
}

public class Event_PeopleLoveParties : SpecialEvent {
	public Event_PeopleLoveParties () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		Advisor partyAdvisor = advisorManagement.getAdvisors () [ManageAdvisors.PARTY];
		this.setMessage ("The newly founded Party Guild has been wildly popular with the people of " +
		gameController.getCharData ().getKingdomName () + "! " + partyAdvisor.getName () + " has " +
		"been drunkenly singing your praises in every tavern in town.");
		initialiseExtraMessageArray (0);
		initialiseButtonTexts (1);
		this.setButtonText ("Continue", 0);
	}
	public override void option1 () {
		ManageOpinion opinionManagement = gameController.getOpinionManagement ();
		opinionManagement.incrementFavour (2);
		exit ();
	}
}

public class Event_PeopleFearNecromancy : SpecialEvent {
	public Event_PeopleFearNecromancy () {
		this.setMessage ("Controversy has arisen around the recently founded Society of Morticians. " +
		"Their practices, though legal, seem to upset some citizens of " +
		gameController.getCharData ().getKingdomName () + ".");
		initialiseExtraMessageArray (1);
		this.setExtraMessage ("There have been a few protests. Your Advisors assure you that it is nothing to " +
		"worry about.", 0);
		initialiseButtonTexts (1);
		this.setButtonText ("Continue", 0);
	}
	public override void option1 () {
		ManageOpinion opinionManagement = gameController.getOpinionManagement ();
		opinionManagement.incrementFavour (-1);
		opinionManagement.setPublicFear (opinionManagement.getPublicFear () + 1);
		opinionManagement.setPublicAwe (opinionManagement.getPublicAwe () + 1);
		exit ();
	}
}

public class Event_FoundAnotherNewGuild : SpecialEvent {
	public Event_FoundAnotherNewGuild () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		Advisor guildAdvisor = advisorManagement.getAdvisors () [ManageAdvisors.GUILDS];
		this.setMessage (guildAdvisor.getName () + ", the time has come to found a new guild! The options are as follows...");
		initialiseExtraMessageArray (3);
		initialiseButtonTexts (2);
		if (advisorManagement.getAdvisors () [ManageAdvisors.NECRO] == null) {
			this.setExtraMessage ("Practioneers of a strange art, calling themselves the Society of Morticians, wish to " +
			"form a guild. They specialise in the study and manipulation of the forces of life and death. It's... interesting.", 0);
			this.setButtonText ("Found the Society of Morticians", 0);
		} else {
			this.setExtraMessage ("A new religion has begun to grow here. The followers of the Badger Prince wish to " +
			"found a temple. The subterranean nature of your tomb building project appeals to them.", 0);
			this.setButtonText ("Found the Temple of the Badger Prince", 0);
		}
		if (advisorManagement.getAdvisors () [ManageAdvisors.TEMPLE] == null) {
			this.setExtraMessage ("A new religion has begun to grow here. The followers of the Badger Prince wish to " +
			"found a temple. The subterranean nature of your tomb building project appeals to them.", 1);
			this.setButtonText ("Found the Temple of the Badger Prince", 1);
		} else {
			this.setExtraMessage ("A cohort of bards from across the world have gathered together. They want to form a guild " +
			"based around partying every day, 'going hard', and something called 'nangs'.", 1);
			this.setButtonText ("Found the Rave Syndicate", 1);
		}
		this.setExtraMessage ("Which of these groups would you most like to support?", 3);
	}
	public override void option1 () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		if (advisorManagement.getAdvisors () [ManageAdvisors.NECRO] == null) {
			advisorManagement.getAdvisors () [ManageAdvisors.NECRO] = new Advisor ();
			//set first milestone for necro advisor
			//set tutorial for advisor also
		} else {
			advisorManagement.getAdvisors () [ManageAdvisors.TEMPLE] = new Advisor ();
			//set first milestone for badger advisor
			//set tutorial for advisor also
		}
		exit();
	}
	public override void option2 () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		if (advisorManagement.getAdvisors () [ManageAdvisors.TEMPLE] == null) {
			advisorManagement.getAdvisors () [ManageAdvisors.TEMPLE] = new Advisor ();
			//set first milestone for badger advisor
			//set tutorial for advisor also
		} else {
			advisorManagement.getAdvisors () [ManageAdvisors.PARTY] = new Advisor ();
			//set first milestone for party advisor
			//set tutorial for advisor also
		}
		exit();
	}
}

public class Event_ImmigratingArtists : SpecialEvent {
	public Event_ImmigratingArtists () {
		CharacterData info = gameController.getCharData ();
		this.setMessage (info.getKingdomName () + " has been attracting attention lately. Hearing of your " +
		"work with the guilds of the city, artisans have been travelling across the world to contribute " +
		"to your tomb.");
		initialiseButtonTexts (1);
		this.setButtonText ("Continue", 0);
	}
	public override void option1 () {
		gameController.getOpinionManagement ().incrementFavour (1);
		exit ();
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