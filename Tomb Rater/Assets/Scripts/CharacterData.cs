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
