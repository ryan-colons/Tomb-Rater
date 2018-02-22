using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageOpinion {

	//number of times you've done something to make the public like you
	private int publicFavour;
	//number of times you've done something to make the public dislike you
	private int publicDisfavour;
	//number of times you've done something to make the public think you're awesome
	private int publicAwe;
	//number of times you've done something to make the public fear you
	private int publicFear;


	public ManageOpinion () {
		publicFavour = 0;
		publicDisfavour = 0;
		publicAwe = 0;
		publicFear = 0;
	}

	public int getNetFavour () {
		return publicFavour - publicDisfavour;
	}

	public void incrementFavour (int n) {
		if (n > 0) {
			publicFavour += n;
		} else {
			publicDisfavour -= n;
		}
	}

	public void setPublicFavour (int n) {
		publicFavour = n;
	}
	public int getPublicFavour () {
		return publicFavour;
	}

	public void setPublicDisfavour (int n) {
		publicDisfavour = n;
	}
	public int getPublicDisfavour () {
		return publicDisfavour;
	}

	public void setPublicFear (int n) {
		publicFear = n;
	}
	public int getPublicFear () {
		return publicFear;
	}

	public void setPublicAwe (int n) {
		publicAwe = n;
	}
	public int getPublicAwe () {
		return publicAwe;
	}
}
