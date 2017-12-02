using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSite {
	public bool available;
	private string message;
	private int numWorkers;

	//value for placing the site on the map
	private int pos;

	public int getPos () {
		return this.pos;
	}
	public void setPos (int n) {
		this.pos = n;
	}

	/*this is here so all ResourceSite coords can be kept in one place
	 *each ResourceSite looks up coords according to which 'position' it should be in
	 * e.g.
	 *	 0	 1	 2	 3
	 * 	 4	 5	 6	 7
	 * 	 8	 9	10	11
	 */
	private static Dictionary<int, float[]> coordLookUp = new Dictionary<int, float[]>(){
		{0, new float[] {-10f, 5f}},
		{1, new float[] {0f, 5f}},
		{2, new float[] {10f, 5f}},
		{3, new float[] {-10f, 0f}},
		{4, new float[] {0f, 0f}}
	};

	public static float[] accessCoordLookUp (ResourceSite site) {
		return coordLookUp [site.pos];
	}


	public static GameController gameController;

	public bool isAvailable () {
		return available;
	}
	public void setAvailable (bool b) {
		available = b;
	}

	public int getNumWorkers () {
		return numWorkers;
	}
	public void setNumWorkers (int n) {
		numWorkers = n;
	}

	public string getMessage () {
		return message;
	}
	public void setMessage (string str) {
		this.message = str;
	}

	public virtual string collectResources (int numWorkers) {
		//this should call addAndRecord for the relevant resources, and return the result
		Debug.Log ("ERROR: Default resource collection called!");
		return "If you can see this, something has gone wrong! default collectResources()";
	}
	public string addAndRecord (Resource resource, int n) {
		ManageResources resourceManagement = gameController.getResourceManagement ();
		resourceManagement.incrementResource (resource, n);
		if (n == 0) {
			return "";
		} else {
			return "+" + n + " " + resource.name + "\n";
		}
	}
}
