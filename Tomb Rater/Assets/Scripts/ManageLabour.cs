using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageLabour {

	//this is where all the possible (available/unavailable) sites are stored
	private ResourceSite[] sites;
	//how much does each worker expect to be paid?
	private int workerExpectationThisYear;
	//how much you're actually paying this year
	private int workerPaymentThisYear;
	//how many more citizens can be assigned?
	//WorkMenu needs to know this when it opens, so it can lay out enough tokens
	private int citizens;

	public ManageLabour () {
		sites = new ResourceSite[4];
		setSite (new Site_Farm (), 		0);
		setSite (new Site_Mine (), 		1);
		setSite (new Site_Woods (), 	2);
		setSite (new Site_Quarry (),	3);

		citizens = 5;
	}

	public void setSite (ResourceSite site, int index) {
		sites [index] = site;
	}
	public ResourceSite[] getSites () {
		//this should maybe return a copy of the array
		//it would need to copy all the objects, not just references, though
		//otherwise the original objects could still be modified via the references
		return sites;
	}

	public int totalWorkersThisYear () {
		int count = 0;
		foreach (ResourceSite site in sites) {
			count += site.getNumWorkers ();
		}
		return count;
	}

	public string collectAllResources () {
		string report = "";
		foreach (ResourceSite site in sites) {
			report += site.collectResources (site.getNumWorkers());
		}
		return report;
	}

	public int getNumAvailableCitizens () {
		int count = 0;
		foreach (ResourceSite site in sites) {
			if (site.isAvailable ()) {
				count += site.getNumWorkers ();
			}
		}
		int availableCitizens = citizens - count;
		return availableCitizens;
	}

	/* this is not finished! */
	public void incrementCitizens (int num) {
		int newNumberOfCitizens = citizens + num;
		int availableCitizens = getNumAvailableCitizens ();

		while (newNumberOfCitizens - availableCitizens - totalWorkersThisYear () < 0) {
			//we can't end up with negative citizens
		}

		citizens = newNumberOfCitizens;
	}


}