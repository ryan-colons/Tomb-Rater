using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class handles UI events for work sites.
 * It doesn't hold any game information - ResourceSite does that.
 */

[RequireComponent(typeof(SpriteDragDestination))]
public class Site : MonoBehaviour {

	private SpriteMouseOver mouseOver;
	private ResourceSite resourceSite;
	private GameObject helpSprite;
	private float helpSpriteOffset = 2f;
	public Sprite sprite;

	private void Start () {
		//make sure the SpriteDragDestination component knows this gameobject is a Site
		this.GetComponent<SpriteDragDestination> ().setSite (true);
		mouseOver = this.gameObject.AddComponent<SpriteMouseOver> ();
		mouseOver.setMessage ("0");

		SpriteRenderer sprRenderer = this.GetComponent<SpriteRenderer> ();
		sprRenderer.sprite = this.sprite;
	}

	private void Awake () {
		//set help sprite - this must be done in Awake, not Start (otherwise moveTo gets called first, resulting in NullPointerException problems
		helpSprite = (GameObject)Instantiate (Resources.Load ("HelpSprite"), 
			new Vector3 (transform.position.x + helpSpriteOffset, transform.position.y + helpSpriteOffset, transform.position.z), 
			Quaternion.identity);
		//unfortunately, we can't easily make helpSprite a child of the site, because then mouseover doesn't work for both
		helpSprite.transform.parent = this.transform.parent;
	}

	public void moveTo (float x, float y) {
		transform.position = new Vector3 (x, y, this.transform.position.z);
		helpSprite.transform.position = new Vector3 (x + helpSpriteOffset, y + helpSpriteOffset, this.transform.position.z);
	}

	public void handleAvailability () {
		bool isAvailable = resourceSite.isAvailable ();
		gameObject.SetActive (isAvailable);
		helpSprite.SetActive (isAvailable);
	}

	public void setResourceSite (ResourceSite r) {
		this.resourceSite = r;
		SpriteMouseOver helpMouseOver = helpSprite.GetComponent<SpriteMouseOver>();
		helpMouseOver.setMessage (resourceSite.getMessage());
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
