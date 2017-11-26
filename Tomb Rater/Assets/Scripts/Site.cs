using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteDragDestination))]
public class Site : MonoBehaviour {

	private SpriteMouseOver mouseOver;
	private ResourceSite resourceSite;

	private void Start () {
		//make sure the SpriteDragDestination component knows this gameobject is a Site
		this.GetComponent<SpriteDragDestination> ().setSite (true);
		mouseOver = this.gameObject.AddComponent<SpriteMouseOver> ();
		mouseOver.setMessage ("0");
		//need a way to decide (and set) the resourceSite properly
		//just doing this for now - BAD!
		setResourceSite (new Site_Woods());
	}

	public void setResourceSite (ResourceSite r) {
		this.resourceSite = r;
	}
	public ResourceSite getResourceSite () {
		return this.resourceSite;
	}

	public void handleDrop (GameObject obj) {
		int prevWorkerCount = this.resourceSite.getNumWorkers ();
		this.resourceSite.setNumWorkers (prevWorkerCount + 1);
		mouseOver.setMessage (this.resourceSite.getNumWorkers().ToString());
	}

}
