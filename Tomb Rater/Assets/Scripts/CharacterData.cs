using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData {

	private string playerName;
	//private string playerNomPronoun;
	//private string playerAccPronoun;
	//private string playerGenPronoun;
	//private string playerAdjective;
	private string playerTitle;

	private int playerAge;

	private string kingdomName;
	private string tradeCivName;
	private string rivalCivName;
	private string complainCitizen;
	private string[] complainPronouns;

	public string getPlayerName () {
		return playerName;
	}
	public void setPlayerName (string str) {
		playerName = str;
	}
	/*
	public string getNomPronoun () {
		return playerNomPronoun;
	}
	public void setNomPronoun (string str) {
		playerNomPronoun = str;
	}
	public string getAccPronoun () {
		return playerAccPronoun;
	}
	public void setAccPronoun (string str) {
		playerAccPronoun = str;
	}
	public string getGenPronoun () {
		return playerGenPronoun;
	}
	public void setGenPronoun (string str) {
		playerGenPronoun = str;
	}

	public string getPlayerAdj () {
		return playerAdjective;
	}
	public void setPlayerAdj (string str) {
		playerAdjective = str;
	}
	*/
	public string getPlayerTitle () {
		return playerTitle;
	}
	public void setPlayerTitle (string str) {
		playerTitle = str;
	}

	public int getPlayerAge () {
		return playerAge;
	}
	public void setPlayerAge (int n) {
		playerAge = n;
	}

	public string getKingdomName () {
		return kingdomName;
	}
	public void setKingdomName (string str) {
		kingdomName = str;
	}
	public string getTradeCivName () {
		return tradeCivName;
	}
	public void setTradeCivName (string str) {
		tradeCivName = str;
	}
	public string getRivalCivName () {
		return rivalCivName;
	}
	public void setRivalCivName (string str) {
		rivalCivName = str;
	}
	public string getComplainName () {
		return complainCitizen;
	}
	public string[] getComplainPronouns () {
		return complainPronouns;
	}
	public string[] getUpperComplainPronouns () {
		return new string[] {
			complainPronouns[0][0].ToString().ToUpper() + complainPronouns[0].Substring(1),
			complainPronouns[1][0].ToString().ToUpper() + complainPronouns[1].Substring(1),
			complainPronouns[2][0].ToString().ToUpper() + complainPronouns[2].Substring(1),
		};
	}


	public void generateCivNames () {
		string[] cityName1 = new string[] {"Ben", "Brath", "Cyn", "Clint", "Dae D", "Duum", "Eriph", "Font Fon",
			"Gab", "Glin", "Hag", "Halm", "Indor", "Jur", "L", "Ll", "Liss", "Mont M", "Myn", "Nymb", "Qua'"
		};
		string[] cityName2 = new string[] {"arm", "ampa", "ebo", "en", "es", "i", "iroa", "o", "olm", "uris"};
		tradeCivName = cityName1 [Random.Range (0, cityName1.Length)] + cityName2 [Random.Range (0, cityName2.Length)];
		rivalCivName = cityName1 [Random.Range (0, cityName1.Length)] + cityName2 [Random.Range (0, cityName2.Length)];
		while (tradeCivName.Equals (rivalCivName)) {
			tradeCivName = cityName1 [Random.Range (0, cityName1.Length)] + cityName2 [Random.Range (0, cityName2.Length)];
		}
	}

	public void generateComplainer () {
		string[] name0 = new string[]{ "Old", "Councillor", "Mad", "Crazy-Eyes", "Wide-Eyes"};
		string[] name1 = new string[]{ "Br", "D", "F", "R", "Wr", "M"};
		string[] name2 = new string[]{ "ent", "ont", "onson", "obson", "esley", "ixon"};
		string[] name3 = new string[]{ "the Wise", "Sweemy", "Dandleton", "Dugnutt", "the Bold", "the Feeble"};

		string name = "";
		if (Random.Range (1, 5) == 1) {
			name += name0 [Random.Range (0, name0.Length)] + " ";
		}
		name += name1 [Random.Range (0, name1.Length)] + name2 [Random.Range (0, name2.Length)];
		if (Random.Range (1, 5) == 1) {
			name += " " + name3 [Random.Range (0, name3.Length)];
		}

		complainCitizen = name;
		if (Random.Range (0, 2) == 0) {
			complainPronouns = new string[]{ "he", "him", "his" };
		} else {
			complainPronouns = new string[]{ "she", "her", "her" };
		}
	}

	public static string getOrdinal (int n) {
		string ordinal = "th";
		if (n % 10 == 1 && n != 11) {
			ordinal = "st";
		} else if (n % 10 == 3 && n != 13) {
			ordinal = "rd";
		}
		return n.ToString () + ordinal;
	}
	public string getAgeOrdinal () {
		return getOrdinal (playerAge);
	}
}
