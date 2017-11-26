using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSite {
	private bool active;
	private int numWorkers;
	public static GameController gameController;

	public bool isActive () {
		return active;
	}
	public void setActive (bool b) {
		active = b;
	}

	public int getNumWorkers () {
		return numWorkers;
	}
	public void setNumWorkers (int n) {
		numWorkers = n;
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

public class Site_Woods : ResourceSite {
	public override string collectResources (int numWorkers) {
		return addAndRecord(new Res_Oak(), numWorkers);
	}
}
