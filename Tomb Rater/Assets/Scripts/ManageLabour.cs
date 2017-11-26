using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageLabour {

	/* This class holds all the information used to build the Labour Management scene.
	 * It doesn't do any building by itself though - that's the WorkMenu
	 */

	private List<ResourceSite> sites;
	//how much does each worker expect to be paid?
	private int workerExpectationThisYear;
	//how much you're actually paying this year
	private int workerPaymentThisYear;
	//how many more citizens can be assigned?
	//WorkMenu needs to know this when it opens, so it can lay out enough tokens
	private int availableCitizens;

	public ManageLabour () {
		sites = new List<ResourceSite> ();
		sites.Add (new Site_Woods ());

		availableCitizens = 100;
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


}