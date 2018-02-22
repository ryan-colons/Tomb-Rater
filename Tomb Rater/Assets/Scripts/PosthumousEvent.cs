using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosthumousEvent {
	private int probability = 5;
	private bool reuse = false;
	public virtual string trigger (TombRater rater) {
		return "";
	}

	public void setProbability (int n) {
		probability = n;
	}
	public virtual int getProbability (TombRater rater) {
		return probability;
	}
	public void setReuse (bool b) {
		reuse = b;
	}
	public bool getReuse () {
		return reuse;
	}
}

// ANY TIME

public class Post_Entropy : PosthumousEvent {
	public Post_Entropy() {
		setProbability (5);
		setReuse (true);
	}
	public override string trigger (TombRater rater) {
		rater.incrementScore (-2);
		return "Time passes. Dust gathers on the history books containing your name.";
	}
}

public class Post_AmateurTombRobbers : PosthumousEvent {
	public Post_AmateurTombRobbers () {
		setProbability (2);
		setReuse (true);
	}
	public override string trigger (TombRater rater) {
		if (Random.Range (0, 5) < rater.getTombSecurity ()) {
			//robbers fail
			rater.incrementScore(3);
			return "A team of tomb robbers attempt to infiltrate your tomb. Stories of their " +
				"failure spread, and people think about you.";
		} else {
			//robbers succeed
			rater.incrementScore(-3);
			return "A bunch of hooligans manage to break into your tomb and steal some treasures. " +
				"Your tomb suddenly seems less impressive to many people.";
		}
	}
}

public class Post_ExpertTombRobbers : PosthumousEvent {
	public Post_ExpertTombRobbers () {
		setProbability (1);
		setReuse (false);
	}
	public override string trigger (TombRater rater) {
		if (Random.Range (0, 20) < rater.getTombSecurity ()) {
			//robbers fail
			rater.incrementScore(8);
			return "A party of adventurers tries to rob your tomb - but fails spectacularly!";
		} else {
			//robbers succeed
			rater.incrementScore(-5);
			return "A party of adventurers pillages your tomb. From then on, when your tomb comes " +
				"up in conversation, people think of them as much as you.";
		}
	}
}

// EARLY

public class Post_ExcitingNewReign : PosthumousEvent {
	public Post_ExcitingNewReign () {
		setProbability (4);
		setReuse (false);
	}
	public override string trigger (TombRater rater) {
		rater.incrementScore (-5);
		return "A new leader is crowned, and has lots of revolutionary ideas. " +
		"Your reign is forgotten a little bit.";
	}
}

public class Post_Vandalism : PosthumousEvent {
	public Post_Vandalism () {
		setReuse (true);
	}
	public override string trigger (TombRater rater) {
		rater.incrementScore (-2);
		return "A group of haters vandalises the outside of your " +
			"tomb with dirt, paint, and chisels.";
	}
	public override int getProbability (TombRater rater) {
		ManageOpinion opinionManagement = rater.getOpinionManagement ();
		int opinion = opinionManagement.getNetFavour ();
		return 5 - opinion;
	}
}

public class Post_MassVisitation : PosthumousEvent {
	public Post_MassVisitation () {
		setProbability (4);
		setReuse (true);
	}
	public override string trigger (TombRater rater) {
		rater.incrementScore (1);
		CharacterData info = rater.getCharData ();
		return "On Throne Day, a parade of people come to visit the resting " +
		"place of their favourite " + info.getPlayerTitle () + "... which is you!";
	}
	public override int getProbability (TombRater rater) {
		ManageOpinion opinionManagement = rater.getOpinionManagement ();
		int opinion = opinionManagement.getNetFavour ();
		return 3 + opinion;
	}
}

// MID

public class Post_NewTomb : PosthumousEvent {
	public Post_NewTomb () {
		setProbability (4);
		setReuse (false);
	}
	public override string trigger (TombRater rater){
		if (Random.Range (0, 2) == 0) {
			rater.incrementScore (-1);
			return "A new ruler builds their tomb near yours. Newer technologies " +
			"and ideas make your tomb look outdated and inferior next to it.";
		} else {
			rater.incrementScore (1);
			return "A new ruler build their tomb near yours. The project is poorly " +
			"managed, and it ends up looking inferior next to the timeless design " +
			"of your own tomb.";
		}
	}
}

public class Post_BloodJarHaunting : PosthumousEvent {
	// only if you have the blood jar treasure installed in your tomb
	public Post_BloodJarHaunting () {
		setProbability (8);
		setReuse (false);
	}
	public override string trigger (TombRater rater){
		rater.incrementScore (4);
		return "Blood stirs within a black jar in your tomb. People in nearby " +
			"cities begin seeing your face in their dreams.";
	}
}

// LATE

public class Post_ArrivalOfElves : PosthumousEvent {
	public Post_ArrivalOfElves () {
		setProbability (10000);
		setReuse (false);
	}
	public override string trigger (TombRater rater) {
		return "Elves arrive from across the sea, and quickly take control " +
		"of most of the continent.";
	}
}

// REALLY LATE

public class Post_Apocalypse : PosthumousEvent {
	public Post_Apocalypse () {
		setProbability (10000);
		setReuse (false);
	}
	public override string trigger (TombRater rater) {
		rater.clearAllLists ();
		rater.addReallyLateEvent (new Post_Entropy());
		return "A great fire washes over the land, destroying nearly all life. " +
		"Only the beetles remain.";
	}
}

public class Post_BeetlesRemember : PosthumousEvent {
	public Post_BeetlesRemember () {
		setProbability (1);
		setReuse (true);
	}
	public override string trigger (TombRater rater) {
		rater.incrementScore (1);
		return "The beetles talk to the rocks about your deeds.";
	}
}