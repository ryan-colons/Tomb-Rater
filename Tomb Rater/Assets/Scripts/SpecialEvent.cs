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
		onTrigger ();
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

	public void displayFeedbackButReturnToMenu (string feedback) {
		GameObject panel = eventUI.setFeedbackDisplay (feedback);
		Button button = panel.GetComponent<Button> ();
		button.onClick.AddListener (delegate {gameController.loadScene("menu");});
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

	public virtual void onTrigger () {}

	public virtual void option1 () {}
	public virtual void option2 () {}
	public virtual void option3 () {}
	public virtual void option4 () {}
	public virtual void option5 () {}
}

// COMPLETELY FIXED EVENTS

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
		gameController.createManagers ();
		gameController.loadScene ("menu");
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
	public Event_MilitaryAdvisorTutorial (Advisor militaryAdvisor) {
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
	public Event_EconomicAdvisorTutorial (Advisor economicAdvisor) {
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
	public Event_GuildAdvisorTutorial (Advisor guildAdvisor) {
		CharacterData info = gameController.getCharData ();
		string guildName = guildAdvisor.getName();
		this.setMessage ("Good day, Your Grace! I am " + guildName + ", and " +
			"I am here as a representative of the guilds of " + info.getKingdomName () + ".");
		initialiseExtraMessageArray (2);
		setExtraMessage ("The guilds are full of talented artisans, performers, and thinkers. They " +
			"can help make your Tomb into a masterpiece, to be admired for centuries!", 0);
		setExtraMessage ("Talk to me if you would like to invest some money into the guilds. " +
			"In return, they can offer exciting treasures and more!", 1);
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

// SEMI FIXED EVENTS
	
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
		this.setButtonText ("Found the Temple of the Badger Prince (jk, hasn't been implemented sorry)", 0);
		this.setButtonText ("Found the Rave Syndicate", 1);
		this.setButtonText ("Found the Society of Morticians", 2);
	}
	public override void option1 () {
		/*
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		advisorManagement.getAdvisors () [ManageAdvisors.TEMPLE] = new Advisor ();
		advisorManagement.getAdvisors () [ManageAdvisors.TEMPLE].setTutorial (new Event_TempleAdvisorTutorial ());
		//set first milestone for badger advisor
		exit();
		*/
	}
	public override void option2 () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		advisorManagement.getAdvisors () [ManageAdvisors.PARTY] = new Advisor ();
		advisorManagement.getAdvisors () [ManageAdvisors.PARTY].setMilestone (new PM_ThrowParty ());
		advisorManagement.getAdvisors () [ManageAdvisors.PARTY].setTutorial (new Event_PartyAdvisorTutorial ());
		ManageYears yearManagement = gameController.getYearManagement();
		yearManagement.addSpecialEventInXYears (new Event_PeopleLoveParties (), 1);
		exit();
	}
	public override void option3 () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		advisorManagement.getAdvisors () [ManageAdvisors.NECRO] = new Advisor ();
		advisorManagement.getAdvisors () [ManageAdvisors.NECRO].setMilestone (new NM_DabbleNecromancy ());
		advisorManagement.getAdvisors () [ManageAdvisors.NECRO].setTutorial (new Event_NecromancyAdvisorTutorial ());
		ManageYears yearManagement = gameController.getYearManagement();
		yearManagement.addSpecialEventInXYears (new Event_PeopleFearNecromancy (), 1);
		exit();
	}
}

public class Event_TempleAdvisorTutorial : SpecialEvent {
	public Event_TempleAdvisorTutorial () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		string temple = advisorManagement.getAdvisors () [ManageAdvisors.TEMPLE].getName();
		string title = gameController.getCharData ().getPlayerTitle ();
		this.setMessage ("\"Well met, " + title + ". I am " + temple + ", Front Paw of the Temple " +
			"of the Badger Prince. I thank you for funding our humble church. We know you are in the " +
			"process of digging a Grave Burrow beneath the mountains. The Badger Prince can guide " +
			"you there, as he is an expert in sheltering underground.");
		initialiseExtraMessageArray (0);
		initialiseButtonTexts (1);
		setButtonText ("Okay.", 0);
	}
	public override void option1 () {
		gameController.loadScene ("menu");
	}
}

public class Event_TempleConflict : SpecialEvent {
	public Event_TempleConflict () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		string temple = advisorManagement.getAdvisors () [ManageAdvisors.TEMPLE].getName();
		CharacterData info = gameController.getCharData ();
		this.setMessage (info.getComplainName () + ", a prominent citizen of " + info.getKingdomName () +
		", has come to lodge a complaint about the new temple in town.");
		initialiseExtraMessageArray (1);
		this.setExtraMessage (info.getComplainName () + " claims that Badger worship amounts to heresy," +
		"and should not be encouraged. " + info.getUpperComplainPronouns () [0] + " suggests that " +
		"Badgerian symbols, such as black-and-white stripes, should not be allowed in public spaces.", 0);
		this.setExtraMessage (temple + ", Adherent of the Badger Prince, angrily disagrees, and says " +
		"that if Badgerian symbols are disallowed in public, then symbols of the Sun Lord should be too.", 1);
		initialiseButtonTexts (3);
		this.setButtonText ("Dismiss " + info.getComplainName () + "'s complaints", 0);
		this.setButtonText ("Criminalise public display of Badgerian symbols", 1);
		this.setButtonText ("Criminalise public display of all religious symbols", 2);
	}
	public override void onTrigger () {
		ManageSpecialEvents specialEventManagement = gameController.getSpecialEventManagement ();
		specialEventManagement.incrementComplaintsHeard ();
	}
	public override void option1 () {
		CharacterData info = gameController.getCharData ();
		displayFeedback (info.getComplainName () + " leaves in a huff.");
	}
	public override void option2 () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		string temple = advisorManagement.getAdvisors () [ManageAdvisors.TEMPLE].getName();
		displayFeedback (temple + " appears upset, but agrees to keep the Badger worship underground.");
	}
	public override void option3 () {
		ManageOpinion opinion = gameController.getOpinionManagement ();
		opinion.incrementFavour (-1);
		displayFeedback ("Within a week, all religious symbols are removed from public view. " +
			"There are a lot of complaints from the various clergy, but all comply.");
	}
}
	
public class Event_PartyAdvisorTutorial : SpecialEvent {
	public Event_PartyAdvisorTutorial () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		string party = advisorManagement.getAdvisors () [ManageAdvisors.PARTY].getName();
		this.setMessage ("\"Hey dude, I'm " + party + ", from the Rave Society or whatever. " +
			"So tight of you to hook us up my dude, we're gonna turn it up around here! " +
			"And like, I know you've got this whole tomb thing going, so hit me up and we can " +
			"suss you some dope shit for it!\"");
		initialiseExtraMessageArray (1);
		this.setExtraMessage (party + " extends a clenched hand towards you.", 0);
		initialiseButtonTexts (3);
		this.setButtonText ("Accept the fist bump", 0);
		this.setButtonText ("Ignore the fist, but nod", 1);
		this.setButtonText ("Order " + party + " to leave", 2);
	}
	public override void option1 () {
		ManageAdvisors manageAdvisors = gameController.getAdvisorManagement ();
		Advisor partyAdvisor = manageAdvisors.getAdvisors () [ManageAdvisors.PARTY];
		partyAdvisor.setTutorial (null);
		displayFeedbackButReturnToMenu ("As you leave, some of your other advisors hurry to " +
			"give " + partyAdvisor.getName() + " instructions on how to properly address you.");
	}
	public override void option2 () {
		this.option1 ();
	}
	public override void option3 () {
		ManageAdvisors manageAdvisors = gameController.getAdvisorManagement ();
		string partyName = manageAdvisors.getAdvisors () [ManageAdvisors.PARTY].getName ();
		manageAdvisors.getAdvisors () [ManageAdvisors.PARTY] = null;
		displayFeedbackButReturnToMenu (partyName + " complains, but leaves.");
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
		opinionManagement.incrementFavour (1);
		exit ();
	}
}

public class Event_NecromancyAdvisorTutorial : SpecialEvent {
	public Event_NecromancyAdvisorTutorial () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		string necro = advisorManagement.getAdvisors () [ManageAdvisors.NECRO].getName();
		this.setMessage ("\"Good day, Your Excellence. I am " + necro + ", a representative " +
			"from the Society of Morticians. I believe we can help each other. We would like " +
			"to contribute to your project.");
		initialiseExtraMessageArray (0);
		initialiseButtonTexts (2);
		setButtonText ("Continue", 0);
		setButtonText ("Order " + necro + " to leave", 1);
	}
	public override void option1 () {
		gameController.loadScene ("menu");
		ManageAdvisors manageAdvisors = gameController.getAdvisorManagement ();
		Advisor necroAdvisor = manageAdvisors.getAdvisors () [ManageAdvisors.NECRO];
		necroAdvisor.setTutorial (null);
	}
	public override void option2 () {
		ManageOpinion opinion = gameController.getOpinionManagement ();
		opinion.incrementFavour (1);
		ManageAdvisors manageAdvisors = gameController.getAdvisorManagement ();
		string necroName = manageAdvisors.getAdvisors () [ManageAdvisors.NECRO].getName ();
		manageAdvisors.getAdvisors () [ManageAdvisors.NECRO] = null;
		displayFeedbackButReturnToMenu (necroName + " leaves, clearly disappointed. Some of " +
			"your other advisors look relieved.");
	}
}

public class Event_NecromancerDabblingGifts : SpecialEvent {
	public Event_NecromancerDabblingGifts () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		string necroName = advisorManagement.getAdvisors () [ManageAdvisors.NECRO].getName();
		this.setMessage (necroName + " comes to you with gifts for your tomb.");
		initialiseExtraMessageArray (2);
		this.setExtraMessage (necroName + " presents a set of bells, strangely blackened somehow.\n " +
			"\"These bells will ring out when intruders enter your tomb, and your corpse will rise to fight them!", 0);
		this.setExtraMessage ("A woman hands you a large jar, made of black glass.\nPut your blood in this jar when " +
			"you are dead. People will notice your spirit.", 1);
		initialiseButtonTexts (2);
		setButtonText ("Accept the weird gifts", 0);
		setButtonText ("Decline the weird gifts", 1);

	}
	public override void option1 () {
		ManageTreasure treasureManagement = gameController.getTreasureManagement ();
		treasureManagement.getTreasureList ().Add (new Tre_BlackenedAlarmBells ());
		treasureManagement.getTreasureList ().Add (new Tre_BloodJar ());
		exit ();
	}
	public override void option2 () {
		displayFeedback ("They nod and leave the room in silence.");
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

public class Event_NecromancerRitualsAreWeird : SpecialEvent {
	public Event_NecromancerRitualsAreWeird () {
		this.setReuse (false);
		this.setProbability (3);
		CharacterData info = gameController.getCharData ();
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		Advisor necro = advisorManagement.getAdvisors () [ManageAdvisors.NECRO];
		this.setMessage ("A prominent member of the community, " + info.getComplainName() + " has " +
		"come to lodge a complaint.");
		initialiseExtraMessageArray (2);
		setExtraMessage ("\"People in black robes have been hiding painted bones in trees and flowers. " +
			"The plants are dying! It's weird! Do something about this, " + info.getPlayerTitle() +"!\"", 0);
		setExtraMessage ("It seems " + necro.getName () + " and colleagues are causing a disturbance.", 1);
		initialiseButtonTexts (3);
		setButtonText ("Dismiss the complaints", 0);
		setButtonText ("Order " + necro.getName () + " to stop", 1);
		setButtonText ("Plant more trees and flowers", 2);
	}
	public override void onTrigger () {
		ManageSpecialEvents specialEventManagement = gameController.getSpecialEventManagement ();
		specialEventManagement.incrementComplaintsHeard ();
	}
	public override void option1 () {
		ManageOpinion opinion = gameController.getOpinionManagement ();
		opinion.incrementFavour (-1);
		CharacterData info = gameController.getCharData ();
		displayFeedback (info.getComplainName() + " left in a huff.");
	}
	public override void option2 () {
		ManageOpinion opinion = gameController.getOpinionManagement ();
		opinion.incrementFavour (1);
		displayFeedback ("The Morticians quietly agreed to stop, to the relief of many citizens.");
	}
	public override void option3 () {
		gameController.setMoney (gameController.getMoney () - 5);
		displayFeedback ("The dying plants were quickly replaced, at your cost.");
	}
}

public class Event_BecomeLich : SpecialEvent {
	public Event_BecomeLich () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		Advisor necro = advisorManagement.getAdvisors () [ManageAdvisors.NECRO];
		setMessage ("As promised, " + necro.getName () + " has prepared an experimental " +
		"ritual to 'hide you from death'. One night, the Society of Morticians " +
		"come to your quarters...");
		initialiseExtraMessageArray (1);
		setExtraMessage ("You wake the next morning with no memory of the night before, " +
		"covered in bruises and colourful paints. You feel... very relaxed...", 0);
		initialiseButtonTexts (1);
		setButtonText ("Continue", 0);
	}
	public override void option1 () {
		ManageSpecialEvents spManagement = gameController.getSpecialEventManagement ();
		spManagement.addPossibleEvent (new Event_LichDeath ());
		spManagement.addPossibleEvent (new Event_LichFlavour ());
		PathToDeath health = gameController.getHealth ();
		health.setLossPerYear (0);
		exit ();
	}
}

public class Event_LichFlavour : SpecialEvent {
	private string[] flavours = new string[] {
		"Today you drank several glasses of a delicious beverage " +
		"that turned out to be Throne Cleaner. You feel fine though!",
		"Today you accidentally cut yourself badly with a cheese knife, " +
		"and smoke came out instead of blood. You feel fine though!",
		"Today you took a bath, and the water immediately turned black " +
		"and started to boil. When you got out, you were bone dry."
	};
	public Event_LichFlavour () {
		setReuse (true);
		setProbability (6);
		setMessage (flavours[Random.Range(0, flavours.Length)]);
		initialiseExtraMessageArray (0);
		initialiseButtonTexts (1);
		setButtonText ("Hmmm", 0);
	}
	public override void option1 () {
		exit ();
	}
}

public class Event_LichDeath : SpecialEvent {
	public Event_LichDeath () {
		setReuse (true);
		setProbability (1);
		setMessage ("A warrior in gleaming armor tumbles through the window of " +
		"your throne room! Brandishing a spiked mace, she yells something about " +
		"the Society of Morticians and crimes against nature, and dashes towards you!");
		initialiseButtonTexts (3);
		setButtonText ("Call for the guards", 0);
		setButtonText ("Reach for your sceptre", 1);
		setButtonText ("Try to run away", 2);
	}
	public override void option1 () {
		int guards = gameController.getAdvisorManagement ().getDefensiveMight ();
		if (guards >= Random.Range (1, 50)) {
			displayFeedback ("Your guards manage to subdue the assassain in time.");
		} else {
			Event_Death deathEvent = new Event_Death ("You were murdered for your " +
			                         "participation in dark rituals.");
			gameController.loadEvent (deathEvent);
		}
	}
	public override void option2 () {
		if (Random.Range (0, 2) == 0) {
			ManageOpinion opinion = gameController.getOpinionManagement ();
			opinion.setPublicAwe (opinion.getPublicAwe () + 1);
			displayFeedback ("Wielding your sceptre as a mace, you hold back the assassain " +
			"until your guards arrive. The guards are impressed with your fighting skills!");
		} else {
			Event_Death deathEvent = new Event_Death ("You were murdered for your " +
				"participation in dark rituals.");
			gameController.loadEvent (deathEvent);
		}
	}
	public override void option3 () {
		Event_Death deathEvent = new Event_Death ("You were murdered for your " +
			"participation in dark rituals.");
		gameController.loadEvent (deathEvent);
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
		gameController.loadScene("legacy");
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

// RANDOM EVENTS

public class Event_ComplaintAboutTaxes : SpecialEvent {
	public Event_ComplaintAboutTaxes () {
		setProbability (5);
		setReuse (true);

		CharacterData info = gameController.getCharData ();
		string name = info.getComplainName ();
		this.setMessage (name + ", a prominent member of the community, has " +
			"come to lodge a complaint. \"Taxes are too high!\", " + info.getComplainPronouns () [0] +
			" he says. \"Either lower taxes, so we can live more comfortably, or " +
			"put more money into improving the quality of life for the average citizen!\"");
		initialiseExtraMessageArray (0);
		initialiseButtonTexts (3);
		this.setButtonText ("Lower taxes", 0);
		this.setButtonText ("Increase public spending", 1);
		this.setButtonText ("Dismiss the complaint", 2);
	}
	public override void onTrigger () {
		ManageSpecialEvents specialEventManagement = gameController.getSpecialEventManagement ();
		specialEventManagement.incrementComplaintsHeard ();
	}
	public override void option1 () {
		gameController.getOpinionManagement ().incrementFavour (1);
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		advisorManagement.setGPT (advisorManagement.getGPT () - 5);
		exit ();
	}
	public override void option2 () {
		option1 ();
	}
	public override void option3 () {
		exit ();
	}
}

public class Event_YetAnotherComplaint : SpecialEvent {
	public Event_YetAnotherComplaint () {
		setProbability (2);
		setReuse (false);
		CharacterData info = gameController.getCharData ();
		string name = info.getComplainName ();
		this.setMessage (name + ", a prominent member of the community, has " +
			"come to lodge a complaint. \"It's too rainy these days!\", " + info.getComplainPronouns () [0] +
			"says. \"You need to put more money into awnings, public umbrellas and " +
			"raincoats. What else do we pay taxes for?\"");
		initialiseExtraMessageArray (1);
		this.setExtraMessage ("\"What do you think?\"", 0);
		initialiseButtonTexts (4);
		this.setButtonText ("Arrest " + name + " for wasting your time", 0);
		this.setButtonText ("Execute " + name + " for being annoying", 1);
		this.setButtonText ("Calmly dismiss the complaint", 2);
		this.setButtonText ("Set up an anti-rain committee", 3);
	}
	public override void option1 () {
		ManageOpinion opinion = gameController.getOpinionManagement ();
		opinion.setPublicFear (opinion.getPublicFear () + 1);
		CharacterData info = gameController.getCharData ();
		string name = info.getComplainName ();
		info.generateComplainer ();
		displayFeedback (name + " is thrown in the dungeon. You savour the silence in your court.");
	}
	public override void option2 () {
		ManageOpinion opinion = gameController.getOpinionManagement ();
		opinion.setPublicFear (opinion.getPublicFear () + 2);
		CharacterData info = gameController.getCharData ();
		string name = info.getComplainName ();
		info.generateComplainer ();
		displayFeedback (name + " is killed on the spot, as a message to all who would disrupt " +
			"your peace with frivilous complaints.");
	}
	public override void option3 () {
		CharacterData info = gameController.getCharData ();
		string name = info.getComplainName ();
		info.generateComplainer ();
		displayFeedback (name + " grumbles and leaves.");
	}
	public override void option4 () {
		CharacterData info = gameController.getCharData ();
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		advisorManagement.setGPT (advisorManagement.getGPT () - 5);
		ManageOpinion opinion = gameController.getOpinionManagement ();
		opinion.incrementFavour (2);
		displayFeedback ("You fund an new anti-rain committee, and begin supplying " +
			info.getKingdomName () + " with supplies for dealing with the mild annoyances of rain. " +
			"Truely, you are a " + info.getPlayerTitle () + " of the people!");
	}
}


// HEALTH EVENTS

public class Event_EatingLotsOfCheese : SpecialEvent {
	public Event_EatingLotsOfCheese () {
		CharacterData info = gameController.getCharData ();
		setMessage ("A new type of cheese has been forged in " + info.getKingdomName() + "! " +
			"It is made from goats milk, and infused with honey, salt, sugar, egg yolks, and butter-essence. " +
			"By all accounts, this is the most superior cheese in the world.");
		initialiseExtraMessageArray (1);
		setExtraMessage ("Cheese is known to be the most royal of foods, and all successful rulers are expected " +
			"to partake generously at every meal. Your advisors ensure that your kitchens are well stocked.", 0);
		initialiseButtonTexts (3);
		setButtonText ("Excellent", 0);
		setButtonText ("Very good", 1);
		setButtonText ("Mmm, cheeeese!", 2);
	}
	public override void option1 () {exit ();}
	public override void option2 () {exit ();}
	public override void option3 () {exit ();}
}

public class Event_CheeseSickness : SpecialEvent {
	public Event_CheeseSickness () {
		setMessage ("At breakfast one morning, while reaching for a second bowl of " +
			"melted goat cheese, you suddenly experience strong discomfort in your stomach. " +
			"As soon as you have finished eating, you pay a visit to the Royal Physician.");
		initialiseExtraMessageArray (4);
		setExtraMessage ("After finding flakes of cheese in your blood, the Royal Physician " +
			"suggests that your diet may have caused you some health issues. Fortunately, " +
			"she has a solution at hand.", 0);
		setExtraMessage ("She produces a tank of murky water, with thousands of tiny specks. " +
			"She explains that these specks are microscopic creatures. If you drink from this water every day, " +
			"these creatures can live inside your body and eat away any excess cheese!", 1);
		setExtraMessage ("Pleased to have the world's finest medical team at your side, you move the tank " +
			"into your quarters, so that you can drink from it regularly.", 2);
		setExtraMessage ("One of your chefs also suggests switching to a diet of snake cheese. This would be more" +
			"expensive to import, but is said to be much healthier than our goat cheese.", 3);
		initialiseButtonTexts (2);
		setButtonText ("Stick with the goat cheese diet", 0);
		setButtonText ("Switch to a healthier cheese diet", 1);
	}
	public override void option1 () {
		exit ();	
	}
	public override void option2 () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		advisorManagement.setGPT (advisorManagement.getGPT () - 2);
		PathToDeath health = gameController.getHealth ();
		health.increment (5);
		exit ();
	}
}

public class Event_OveractiveParasites : SpecialEvent {
	public Event_OveractiveParasites () {
		setMessage ("You have been feeling nauseous and tired lately. " +
			"Today, you make a visit to the Royal Physician, who quickly " +
			"identifies the problem.");
		initialiseExtraMessageArray (3);
		setExtraMessage ("It appears that the cheese eating parasites in your " +
			"blood stream have grown, and are now eating your internal organs. They " +
			"will need to be removed.", 0);
		setExtraMessage ("Your Royal Physician has a solution. Every fortnight, " +
			"your blood should be passed through a special medical sieve, " +
			"to filter out the parasites. Then, it can be put back into your body.", 1);
		setExtraMessage ("This process may be tough for your body to handle. Some of your " +
			"Advisors recommend mandatory blood donations from your citizens, to help " +
			"you through the process. Others fear this might not be popular decision.", 2);
		initialiseButtonTexts (2);
		setButtonText ("Undergo the process alone", 0);
		setButtonText ("Make citizens donate blood", 1);
	}
	public override void option1 () {
		exit ();
	}
	public override void option2 () {
		ManageOpinion opinion = gameController.getOpinionManagement ();
		opinion.incrementFavour (-1);
		PathToDeath health = gameController.getHealth ();
		health.increment (5);
		exit ();
	}
}

public class Event_TerminalIllness : SpecialEvent {
	public Event_TerminalIllness () {
		setMessage ("Lately you have noticed that the veins in your " +
			"arms are very black. Appreciating that good health is important, " +
			"you schedule a visit with the Royal Physician.");
		initialiseExtraMessageArray (2);
		setExtraMessage ("The Royal Physician runs several diagnostic tests, " +
			"looking more and more distressed as she goes. Finally, she says " +
			"that you have somehow contracted Chronic Blood Sickness, " +
			"a rare terminal disease.", 0);
		setExtraMessage ("There is no real cure for Chronic Blood Sickness. " +
			"The Royal Physician gives you 3 years to live. However, she advises " +
			"that snorting crushed emeralds could fortify your heart, perhaps " +
			"giving you an extra few years of life.", 1);
		initialiseButtonTexts (2);
		setButtonText ("Accept your fate with grace", 0);
		setButtonText ("Snort as many emeralds as possible (100g)", 1);
	}
	public override void option1 () {
		ManageYears yearManagement = gameController.getYearManagement ();
		Event_Death deathEvent = new Event_Death ("You succumbed to the deadly effects " +
			"of Chronic Blood Sickness.");
		yearManagement.addSpecialEventInXYears (deathEvent, 3);
		exit ();
	}
	public override void option2 () {
		gameController.setMoney (gameController.getMoney () - 100);
		ManageYears yearManagement = gameController.getYearManagement ();
		Event_Death deathEvent = new Event_Death ("You succumbed to the deadly effects " +
			"of Chronic Blood Sickness.");
		yearManagement.addSpecialEventInXYears (deathEvent, 5);
		displayFeedback ("Inhaling all that emerald dust actually made you feel a bit better!");
	}
}

// REVOLUTION EVENTS

public class Event_Unpopular : SpecialEvent {
	public Event_Unpopular () {
		CharacterData info = gameController.getCharData ();
		setMessage ("One of your Advisors comes to show you some anti-" + info.getPlayerName() +
		" propaganda posters that have been showing up in some corners of the city. Apparently, " +
		"some of your recent decisions have not been popular with the people.");
		initialiseExtraMessageArray (0);
		initialiseButtonTexts (2);
		setButtonText ("Endeavour to do better for your people", 0);
		setButtonText ("Shrug", 1);
	}
	public override void option1 () { exit ();}
	public override void option2 () { exit ();}
}

public class Event_Protests : SpecialEvent {
	public Event_Protests () {
		CharacterData info = gameController.getCharData ();
		setMessage ("There have been reports of violent protests breaking out in " + info.getKingdomName() + 
		". It seems the people are upset about some of your recent decisions. Your Advisors are concerned, " +
		"and offer some possible responses.");

		initialiseButtonTexts (3);
		setButtonText ("Ignore the protesters", 0);
		setButtonText ("Arrest the protesters", 1);
		setButtonText ("Send a cake to every citizen (50g)", 2);
	}
	public override void option1 () { exit ();}
	public override void option2 () {
		gameController.getOpinionManagement ().incrementFavour (-1);
		PathToDeath revolution = gameController.getRevolution ();
		revolution.increment (10);
		displayFeedback ("You sent your guards to arrest the protesters. The citizens seemed " +
		"angry about this, but the protests quickly stopped.");
	}
	public override void option3 () {
		gameController.setMoney (gameController.getMoney () - 50);
		gameController.getOpinionManagement ().incrementFavour (1);
		exit ();
	}
}

public class Event_AssassinationPrevented : SpecialEvent{
	public Event_AssassinationPrevented () {
		setMessage ("One day, your guards find a group of strangers lurking " +
		"around the palace kitchen. After arresting and questioning them, the guards " +
		"determine that they were trying to poison your food!");
		initialiseExtraMessageArray (1);
		setExtraMessage ("Apparently, your citizens are disgruntled enough to try to kill you! " +
		"Your Advisors come up with some plans to help prevent future assassination attempts.", 0);
		initialiseButtonTexts (3);
		setButtonText ("Criminalise assassination", 0);
		setButtonText ("Put more money into helping your citizens", 1);
		setButtonText ("Publicly execute the assassins who tried to poison you", 2);
	}
	public override void option1 () {
		displayFeedback ("You make sure everyone knows that it's a crime to murder you. That should do it.");
	}
	public override void option2 () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		advisorManagement.setGPT (advisorManagement.getGPT () - 5);
		gameController.getOpinionManagement ().incrementFavour (1);
		PathToDeath revolution = gameController.getRevolution ();
		revolution.increment (5);
		displayFeedback ("You set up several expensive ongoing programs for raising the quality of life " +
		"for your citizens.");
	}
	public override void option3 () {
		gameController.getOpinionManagement ().incrementFavour (-1);
		PathToDeath revolution = gameController.getRevolution ();
		revolution.increment (5);
		displayFeedback ("The mood at the public execution was... tense. It didn't seem to go over well, " +
		"but at least everyone knows not to mess with you.");
	}
}

public class Event_AssassinationSuccessful : SpecialEvent {
	public Event_AssassinationSuccessful () {
		setMessage ("Walking back to your throne from the kitchen after a delicious " +
		"meal, you suddenly notice that none of your guards are around. It's quiet...");
		initialiseExtraMessageArray (1);
		setExtraMessage ("Several citizens step out of the shadows, brandishing knives! " +
		"They quickly surround you, yelling about your crimes against them, and carve " +
		"you up like an ice sculpture.", 0);
		initialiseButtonTexts (1);
		setButtonText ("Ouch", 0);
	}
	public override void option1 () {
		Event_Death deathEvent = new Event_Death ("You were assassinated by disgruntled citizens.");
		gameController.loadEvent (deathEvent);
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